using System;
using System.Collections;
using System.Collections.Generic;
using HyperCasual.Core;
using HyperCasual.Gameplay;
using UnityEngine;

namespace HyperCasual.Runner
{
    /// <summary>
    /// A simple inventory class that listens to game events and keeps track of the amount of in-game currencies
    /// collected by the player
    /// </summary>
    public class Inventory : AbstractSingleton<Inventory>
    {
        [SerializeField]
        private GenericGameEventListener m_GoldEventListener;

        [SerializeField]
        private GenericGameEventListener m_KeyEventListener;

        [SerializeField]
        private GenericGameEventListener m_WinEventListener;

        [SerializeField]
        private GenericGameEventListener m_LoseEventListener;

        private int m_TempGold;
        private int m_TotalGold;
        private float m_TempXp;
        private float m_TotalXp;
        private int m_TempKeys;

        /// <summary>
        /// Temporary const
        /// Users keep accumulating XP when playing the game and they're rewarded as they hit a milestone.
        /// Milestones are simply a threshold to reward users for playing the game. We need to come up with
        /// a proper formula to calculate milestone values but because we don't have a plan for the milestone
        /// rewards yet, we have simple set the value to something users can never reach.
        /// </summary>
        private const float k_MilestoneFactor = 1.2f;

        private Hud m_Hud;
        private LevelCompleteScreen m_LevelCompleteScreen;

        private void Start()
        {
            m_GoldEventListener.EventHandler = OnGoldPicked;
            m_KeyEventListener.EventHandler = OnKeyPicked;
            m_WinEventListener.EventHandler = OnWin;
            m_LoseEventListener.EventHandler = OnLose;

            m_TempGold = 0;
            m_TotalGold = SaveManager.Instance.Currency;
            m_TempXp = 0;
            m_TotalXp = SaveManager.Instance.XP;
            m_TempKeys = 0;

            m_LevelCompleteScreen = UIManager.Instance.GetView<LevelCompleteScreen>();
            m_Hud = UIManager.Instance.GetView<Hud>();
        }

        private void OnEnable()
        {
            m_GoldEventListener.Subscribe();
            m_KeyEventListener.Subscribe();
            m_WinEventListener.Subscribe();
            m_LoseEventListener.Subscribe();
        }

        private void OnDisable()
        {
            m_GoldEventListener.Unsubscribe();
            m_KeyEventListener.Unsubscribe();
            m_WinEventListener.Unsubscribe();
            m_LoseEventListener.Unsubscribe();
        }

        private void OnGoldPicked()
        {
            if (m_GoldEventListener.m_Event is ItemPickedEvent goldPickedEvent)
            {
                m_TempGold += goldPickedEvent.Count;
                if (m_Hud != null)
                {
                    m_Hud.GoldValue = m_TempGold;
                }
            }
            else
            {
                throw new Exception($"Invalid event type!");
            }
        }

        private void OnKeyPicked()
        {
            if (m_KeyEventListener.m_Event is ItemPickedEvent keyPickedEvent)
            {
                m_TempKeys += keyPickedEvent.Count;
            }
            else
            {
                throw new Exception($"Invalid event type!");
            }
        }

        private void OnWin()
        {
            m_TotalGold += m_TempGold;
            m_TempGold = 0;
            SaveManager.Instance.Currency = m_TotalGold;

            m_LevelCompleteScreen.GoldValue = m_TotalGold;
            m_LevelCompleteScreen.XpSlider.minValue = m_TotalXp;
            m_LevelCompleteScreen.XpSlider.maxValue = k_MilestoneFactor * (m_TotalXp + m_TempXp);
            m_LevelCompleteScreen.XpValue = m_TotalXp + m_TempXp;

            m_LevelCompleteScreen.StarCount = m_TempKeys;

            m_TotalXp += m_TempXp;
            m_TempXp = 0f;
            SaveManager.Instance.XP = m_TotalXp;
        }

        private void OnLose()
        {
            m_TempGold = 0;
            m_TotalXp += m_TempXp;
            m_TempXp = 0f;
            SaveManager.Instance.XP = m_TotalXp;
        }

        private void Update()
        {
            if (m_Hud != null && m_Hud.gameObject.activeSelf)
            {
                m_TempXp += PlayerController.Instance.Speed * Time.deltaTime;
                m_Hud.XpValue = m_TempXp;

                if (SequenceManager.Instance.m_CurrentLevel is LoadLevelFromDef loadLevelFromDef)
                {
                    m_Hud.XpSlider.minValue = 0;
                    m_Hud.XpSlider.maxValue = loadLevelFromDef.m_LevelDefinition.LevelLength;
                }
            }
        }
    }
}