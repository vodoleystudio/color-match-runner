using System.Collections;
using System.Collections.Generic;
using HyperCasual.Core;
using UnityEngine;
using UnityEngine.UI;
using GameCore.Data;
using System.Linq;
using TMPro;
using System;
using DG.Tweening;

namespace HyperCasual.Runner
{
    /// <summary>
    /// This View contains Game-over Screen functionalities
    /// </summary>
    public class GameoverScreen : View
    {
        [SerializeField]
        private RectTransform m_SliderRectTransform;

        [SerializeField]
        private Animator m_SliderAnimator;

        [SerializeField]
        private TextMeshProUGUI m_BarText;

        [SerializeField]
        private PopUp m_PopUpMassage;

        [SerializeField]
        private GameObject m_Buttons;

        public PopUp PopUpMassage => m_PopUpMassage;

        public Animator SliderAnimator
        {
            get => m_SliderAnimator;
            set
            {
                m_SliderAnimator = value;
            }
        }

        public RectTransform SliderMask
        {
            get => m_SliderRectTransform;
            set
            {
                m_SliderRectTransform = value;
            }
        }

        public int MatchInProcentText
        {
            get => Convert.ToInt32(m_BarText.text);
            set
            {
                m_BarText.text = $"{value}%";
            }
        }

        [SerializeField]
        private HyperCasualButton m_PlayAgainButton;

        [SerializeField]
        private HyperCasualButton m_GoToMainMenuButton;

        [SerializeField]
        private HyperCasualButton m_NextLevelButton;

        [SerializeField]
        private AbstractGameEvent m_PlayAgainEvent;

        [SerializeField]
        private AbstractGameEvent m_GoToMainMenuEvent;

        [SerializeField]
        private AbstractGameEvent m_NextLevelEvent;

        public void ActivePhaseTwo(bool state)
        {
            m_Buttons.SetActive(state);
        }

        private void OnEnable()
        {
            m_PlayAgainButton.AddListener(OnPlayAgainButtonClick);
            m_GoToMainMenuButton.AddListener(OnGoToMainMenuButtonClick);
            m_NextLevelButton.AddListener(OnNextLevelButtonClick);
        }

        private void OnDisable()
        {
            m_PlayAgainButton.RemoveListener(OnPlayAgainButtonClick);
            m_GoToMainMenuButton.RemoveListener(OnGoToMainMenuButtonClick);
            m_NextLevelButton.RemoveListener(OnNextLevelButtonClick);
        }

        private void OnPlayAgainButtonClick()
        {
            m_PlayAgainEvent.Raise();
        }

        private void OnGoToMainMenuButtonClick()
        {
            m_GoToMainMenuEvent.Raise();
        }

        private void OnNextLevelButtonClick()
        {
            m_NextLevelEvent.Raise();
        }
    }
}