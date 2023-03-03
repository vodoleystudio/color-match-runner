using System;
using System.Collections;
using System.Collections.Generic;
using HyperCasual.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using GameCore.Data;
using System.Diagnostics;

namespace HyperCasual.Runner
{
    /// <summary>
    /// A button used by LevelSelectionScreen to dynamically populate the list of levels to select from
    /// </summary>
    public class LevelSelectButton : HyperCasualButton
    {
        private const string k_FolderName = "UiImages";
        private const string k_TargetName = "Target";

        [SerializeField]
        private Image m_MatchIcon;

        [SerializeField]
        private Image m_PartialMatchIcon;

        [SerializeField]
        private Image m_NotMatchIcon;

        [SerializeField]
        private Image m_QuestionIcon;

        private int m_Index = -1;
        private Action<int> m_OnClick;
        private bool m_IsUnlocked;

        /// <param name="index">The index of the associated level</param>
        /// <param name="unlocked">Is the associated level locked?</param>
        /// <param name="onClick">callback method for this button</param>
        public void SetData(int index, bool unlocked, Action<int> onClick, LevelData levelData)
        {
            m_Index = index;
            m_OnClick = onClick;
            m_IsUnlocked = unlocked;
            m_Button.interactable = m_IsUnlocked;
            ActivateMatchState(levelData);
            m_Button.image.sprite = Resources.Load<Sprite>($"{k_FolderName}/{k_TargetName}{index}");
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            AddListener(OnClick);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            RemoveListener(OnClick);
        }

        protected override void OnClick()
        {
            m_OnClick?.Invoke(m_Index);
            PlayButtonSound();
        }

        private void ActivateMatchState(LevelData levelData)
        {
            if (levelData?.MatchData != null)
            {
                switch (levelData.MatchData.MatchState)
                {
                    case MatchState.Match:
                        ActivateAllTheViewsExeptOne(m_MatchIcon);
                        break;

                    case MatchState.PartialMatch:
                        ActivateAllTheViewsExeptOne(m_PartialMatchIcon);
                        break;

                    case MatchState.NotMatch:
                        ActivateAllTheViewsExeptOne(m_NotMatchIcon);
                        break;
                }
            }
            else
            {
                ActivateAllTheViewsExeptOne(m_QuestionIcon);
            }
        }

        private void ActivateAllTheViewsExeptOne(Image icon)
        {
            m_MatchIcon.gameObject.SetActive(false);
            m_PartialMatchIcon.gameObject.SetActive(false);
            m_NotMatchIcon.gameObject.SetActive(false);
            m_QuestionIcon.gameObject.SetActive(false);

            if (icon == m_MatchIcon)
            {
                m_MatchIcon.gameObject.SetActive(true);
            }
            if (icon == m_PartialMatchIcon)
            {
                m_PartialMatchIcon.gameObject.SetActive(true);
            }
            if (icon == m_NotMatchIcon)
            {
                m_NotMatchIcon.gameObject.SetActive(true);
            }
            if (icon == m_QuestionIcon)
            {
                m_QuestionIcon.gameObject.SetActive(true);
            }
        }
    }
}