using System;
using System.Collections;
using System.Collections.Generic;
using GameCore.UI;
using HyperCasual.Core;
using HyperCasual.Runner;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HyperCasual.Gameplay
{
    /// <summary>
    /// This View contains head-up-display functionalities
    /// </summary>
    public class Hud : View
    {
        [SerializeField]
        private TextMeshProUGUI m_GoldText;

        [SerializeField]
        private Slider m_XpSlider;

        [SerializeField]
        private Tutorial m_Tutorial;

        [SerializeField]
        private HyperCasualButton m_PauseButton;

        [SerializeField]
        private AbstractGameEvent m_PauseEvent;

        [SerializeField]
        private AbstractGameEvent m_HudEnableEvent;

        /// <summary>
        /// The slider that displays the XP value
        /// </summary>
        public Slider XpSlider => m_XpSlider;

        private int m_GoldValue;

        /// <summary>
        /// The amount of gold to display on the hud.
        /// The setter method also sets the hud text.
        /// </summary>
        public int GoldValue
        {
            get => m_GoldValue;
            set
            {
                if (m_GoldValue != value)
                {
                    m_GoldValue = value;
                    m_GoldText.text = GoldValue.ToString();
                }
            }
        }

        private float m_XpValue;

        /// <summary>
        /// The amount of XP to display on the hud.
        /// The setter method also sets the hud slider value.
        /// </summary>
        public float XpValue
        {
            get => m_XpValue;
            set
            {
                if (!Mathf.Approximately(m_XpValue, value))
                {
                    m_XpValue = value;
                    m_XpSlider.value = m_XpValue;
                }
            }
        }

        public void ShowTutorial()
        {
            m_Tutorial.ShowTutorial();
        }

        private void OnEnable()
        {
            m_PauseButton.AddListener(OnPauseButtonClick);
            m_HudEnableEvent.Raise();
        }

        private void OnDisable()
        {
            m_PauseButton.RemoveListener(OnPauseButtonClick);
        }

        private void OnPauseButtonClick()
        {
            m_PauseEvent.Raise();
        }
    }
}