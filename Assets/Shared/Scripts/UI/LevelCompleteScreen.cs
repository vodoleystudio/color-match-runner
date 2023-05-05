using HyperCasual.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HyperCasual.Runner
{
    /// <summary>
    /// This View contains celebration screen functionalities
    /// </summary>
    public class LevelCompleteScreen : View
    {
        [SerializeField]
        private HyperCasualButton m_NextButton;

        [SerializeField]
        private Image[] m_Stars;

        [SerializeField]
        private AbstractGameEvent m_NextLevelEvent;

        [SerializeField]
        private TextMeshProUGUI m_GoldText;

        [SerializeField]
        private Slider m_XpSlider;

        /// <summary>
        /// The slider that displays the XP value
        /// </summary>
        public Slider XpSlider => m_XpSlider;

        private int m_GoldValue;

        /// <summary>
        /// The amount of gold to display on the celebration screen.
        /// The setter method also sets the celebration screen text.
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
        /// The amount of XP to display on the celebration screen.
        /// The setter method also sets the celebration screen slider value.
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

        private int m_StarCount = -1;

        /// <summary>
        /// The number of stars to display on the celebration screen.
        /// </summary>
        public int StarCount
        {
            get => m_StarCount;
            set
            {
                if (m_StarCount != value)
                {
                    m_StarCount = value;
                    DisplayStars(m_StarCount);
                }
            }
        }

        private void OnEnable()
        {
            m_NextButton.AddListener(OnNextButtonClicked);
        }

        private void OnDisable()
        {
            m_NextButton.RemoveListener(OnNextButtonClicked);
        }

        private void OnNextButtonClicked()
        {
            m_NextLevelEvent.Raise();
        }

        private void DisplayStars(int count)
        {
            count = Mathf.Clamp(count, 0, m_Stars.Length);

            if (m_Stars.Length > 0 && count >= 0 && count <= m_Stars.Length)
            {
                for (int i = 0; i < m_Stars.Length; i++)
                {
                    m_Stars[i].gameObject.SetActive(i < count);
                }
            }
        }
    }
}