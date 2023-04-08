using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameCore.Data;

namespace HyperCasual.Runner
{
    /// <summary>
    /// A button used by LevelSelectionScreen to dynamically populate the list of levels to select from
    /// </summary>
    public class LevelSelectButton : HyperCasualButton
    {
        private const string k_FolderName = "UiImages";
        private const string k_TargetName = "Target";
        private List<Image> m_Icons = new();

        [SerializeField]
        private Image m_MainImage;

        [SerializeField]
        private Image m_LockerImage;

        [SerializeField]
        private Image m_HeartIcon;

        [SerializeField]
        private Image m_LikeIcon;

        [SerializeField]
        private Image m_BrokenHeartIcon;

        [SerializeField]
        private Image m_DisLikeIcon;

        [SerializeField]
        private Image m_QuestionIcon;

        [SerializeField]
        private GameObject m_MatchIconsContainer;

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
            m_LockerImage.gameObject.SetActive(!m_IsUnlocked);
            m_MatchIconsContainer.SetActive(m_IsUnlocked);
            ActivateMatchState(levelData);
            m_MainImage.sprite = Resources.Load<Sprite>($"{k_FolderName}/{k_TargetName}{index}");
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

        private void Awake()
        {
            m_Icons.Add(m_HeartIcon);
            m_Icons.Add(m_LikeIcon);
            m_Icons.Add(m_DisLikeIcon);
            m_Icons.Add(m_BrokenHeartIcon);
            m_Icons.Add(m_QuestionIcon);
        }

        private void ActivateMatchState(LevelData levelData)
        {
            if (levelData?.MatchData != null)
            {
                switch (levelData.MatchData.MatchState)
                {
                    case MatchState.Heart:
                        ActivateIcon(m_HeartIcon);
                        break;

                    case MatchState.Like:
                        ActivateIcon(m_LikeIcon);
                        break;

                    case MatchState.BrokenHeart:
                        ActivateIcon(m_BrokenHeartIcon);
                        break;

                    case MatchState.DisLike:
                        ActivateIcon(m_DisLikeIcon);
                        break;
                }
            }
            else
            {
                ActivateIcon(m_QuestionIcon);
            }
        }

        private void ActivateIcon(Image icon)
        {
            foreach (var image in m_Icons)
            {
                image.gameObject.SetActive(image == icon);
            }
        }
    }
}