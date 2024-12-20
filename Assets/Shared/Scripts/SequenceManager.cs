using System;
using System.Collections;
using System.Collections.Generic;
using HyperCasual.Core;
using HyperCasual.Runner;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HyperCasual.Gameplay
{
    /// <summary>
    /// This singleton determines the state of the game based on observed game events.
    /// </summary>
    [Serializable]
    public class SequenceManager : AbstractSingleton<SequenceManager>
    {
        [SerializeField]
        private GameObject[] m_PreloadedAssets;

        [SerializeField]
        private AbstractLevelData[] m_Levels;

        [SerializeField]
        private GameObject[] m_LevelManagers;

        public AbstractLevelData[] Levels => m_Levels;

        [Header("Events")]
        [SerializeField]
        private AbstractGameEvent m_ContinueEvent;

        [SerializeField]
        private AbstractGameEvent m_PlayAgainEvent;

        [SerializeField]
        private AbstractGameEvent m_BackEvent;

        [SerializeField]
        private AbstractGameEvent m_WinEvent;

        [SerializeField]
        private AbstractGameEvent m_LoseEvent;

        [SerializeField]
        private AbstractGameEvent m_PauseEvent;

        [Header("Other")]
        [SerializeField]
        private float m_SplashDelay = 2f;

        private readonly StateMachine m_StateMachine = new();
        private IState m_SplashScreenState;
        private IState m_MainMenuState;
        private IState m_LevelSelectState;
        private readonly List<IState> m_LevelStates = new();
        public IState m_CurrentLevel;
        private const int k_TargetFrameRate = 60;

        private SceneController m_SceneController;

        /// <summary>
        /// Initializes the SequenceManager
        /// </summary>
        public void Initialize()
        {
            Application.targetFrameRate = k_TargetFrameRate;
            m_SceneController = new SceneController(SceneManager.GetActiveScene());

            InstantiatePreloadedAssets();

            m_SplashScreenState = new State(ShowUI<SplashScreen>);
            m_StateMachine.Run(m_SplashScreenState);

            CreateMenuNavigationSequence();
            CreateLevelSequences();
            SetStartingLevel(0);
        }

        private void InstantiatePreloadedAssets()
        {
            foreach (var asset in m_PreloadedAssets)
            {
                Instantiate(asset);
            }
        }

        private void CreateMenuNavigationSequence()
        {
            //Create states
            var splashDelay = new DelayState(m_SplashDelay);
            m_MainMenuState = new State(OnMainMenuDisplayed);
            m_LevelSelectState = new State(OnLevelSelectionDisplayed);

            //Connect the states
            m_SplashScreenState.AddLink(new Link(splashDelay));
            splashDelay.AddLink(new Link(m_MainMenuState));
            m_MainMenuState.AddLink(new EventLink(m_ContinueEvent, m_LevelSelectState));
            m_LevelSelectState.AddLink(new EventLink(m_BackEvent, m_MainMenuState));
        }

        private void CreateLevelSequences()
        {
            m_LevelStates.Clear();

            //Create and connect all level states
            IState lastState = null;
            foreach (var level in m_Levels)
            {
                IState state = null;
                if (level is SceneRef sceneLevel)
                {
                    state = CreateLevelState(sceneLevel.m_ScenePath);
                }
                else
                {
                    state = CreateLevelState(level);
                }
                lastState = AddLevelPeripheralStates(state, m_LevelSelectState, lastState);
            }

            //Closing the loop: connect the last level to the level-selection state
            var unloadLastScene = new UnloadLastSceneState(m_SceneController);
            lastState?.AddLink(new EventLink(m_ContinueEvent, unloadLastScene));
            unloadLastScene.AddLink(new Link(m_LevelSelectState));
        }

        /// <summary>
        /// Creates a level state from a scene
        /// </summary>
        /// <param name="scenePath"></param>
        /// <returns></returns>
        private IState CreateLevelState(string scenePath)
        {
            return new LoadSceneState(m_SceneController, scenePath);
        }

        /// <summary>
        /// Creates a level state from a level data
        /// </summary>
        /// <param name="levelData"></param>
        /// <returns></returns>
        private IState CreateLevelState(AbstractLevelData levelData)
        {
            return new LoadLevelFromDef(m_SceneController, levelData, m_LevelManagers);
        }

        private IState AddLevelPeripheralStates(IState loadLevelState, IState quitState, IState lastState)
        {
            //Create states
            m_LevelStates.Add(loadLevelState);
            var gameplayState = new State(() => OnGamePlayStarted(loadLevelState));
            var winState = new State(() => OnEndGameDisplayed(loadLevelState));
            var loseState = new State(() => OnEndGameDisplayed(loadLevelState));
            var pauseState = new PauseState(ShowUI<PauseMenu>);
            var unloadLose = new UnloadLastSceneState(m_SceneController);
            var unloadPause = new UnloadLastSceneState(m_SceneController);

            //Connect the states
            lastState?.AddLink(new EventLink(m_ContinueEvent, loadLevelState));
            loadLevelState.AddLink(new Link(gameplayState));

            gameplayState.AddLink(new EventLink(m_WinEvent, winState));
            gameplayState.AddLink(new EventLink(m_LoseEvent, loseState));
            gameplayState.AddLink(new EventLink(m_PauseEvent, pauseState));

            winState.AddLink(new EventLink(m_PlayAgainEvent, loadLevelState));
            winState.AddLink(new EventLink(m_BackEvent, unloadLose));

            loseState.AddLink(new EventLink(m_ContinueEvent, loadLevelState));
            loseState.AddLink(new EventLink(m_BackEvent, unloadLose));
            unloadLose.AddLink(new Link(quitState));

            pauseState.AddLink(new EventLink(m_ContinueEvent, gameplayState));
            pauseState.AddLink(new EventLink(m_BackEvent, unloadPause));
            unloadPause.AddLink(new Link(m_MainMenuState));

            return winState;
        }

        /// <summary>
        /// Changes the starting gameplay level in the sequence of levels by making a slight change to its links
        /// </summary>
        /// <param name="index">Index of the level to set as starting level</param>
        public void SetStartingLevel(int index)
        {
            m_LevelSelectState.RemoveAllLinks();
            m_LevelSelectState.AddLink(new EventLink(m_ContinueEvent, m_LevelStates[index]));
            m_LevelSelectState.AddLink(new EventLink(m_BackEvent, m_MainMenuState));
            m_LevelSelectState.EnableLinks();
        }

        private void ShowUI<T>() where T : View
        {
            UIManager.Instance.Show<T>();
        }

        private void OnMainMenuDisplayed()
        {
            ShowUI<MainMenu>();
            AudioManager.Instance.StopMusicEffect();
            AudioManager.Instance.PlayMusic(SoundID.MenuMusic);
            FindObjectOfType<UIGameOnSimulator>(true).gameObject.SetActive(true);
        }

        private void OnEndGameDisplayed(IState currentLevel)
        {
            ShowUI<GameoverScreen>();
            SaveLevel(currentLevel);
        }

        private void OnWinScreenDisplayed(IState currentLevel)
        {
            UIManager.Instance.Show<LevelCompleteScreen>();
            SaveLevel(currentLevel);
        }

        private void SaveLevel(IState currentLevel)
        {
            var currentLevelIndex = m_LevelStates.IndexOf(currentLevel);

            if (currentLevelIndex == -1)
                throw new Exception($"{nameof(currentLevel)} is invalid!");

            var levelProgress = SaveManager.Instance.LevelProgress;
            if (currentLevelIndex == levelProgress && currentLevelIndex < m_LevelStates.Count - 1)
                SaveManager.Instance.LevelProgress = levelProgress + 1;
        }

        private void OnLevelSelectionDisplayed()
        {
            ShowUI<LevelSelectionScreen>();
            AudioManager.Instance.StopMusicEffect();
            AudioManager.Instance.StopMusic();
            AudioManager.Instance.PlayMusic(SoundID.MenuMusic);
            FindObjectOfType<UIGameOnSimulator>(true).gameObject.SetActive(true);
        }

        private void OnGamePlayStarted(IState current)
        {
            m_CurrentLevel = current;
            ShowUI<Hud>();
            AudioManager.Instance.PlayMusicEffect(SoundID.WindSFX);
            AudioManager.Instance.ReplayMusic(SoundID.GameMusic);
            FindObjectOfType<UIGameOnSimulator>(true).gameObject.SetActive(false);
        }
    }
}