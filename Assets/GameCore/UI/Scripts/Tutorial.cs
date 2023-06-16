using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [Serializable]
    private class MessageData
    {
        [SerializeField]
        private float m_StartSecond;

        public float StartSecond => m_StartSecond;

        [SerializeField]
        private string m_PopUpMessage;

        public string PopUpMessage => m_PopUpMessage;

        [SerializeField]
        private Vector2 m_Position;

        public Vector2 Position => m_Position;
    }

    [SerializeField]
    private List<MessageData> m_MessageDatas;

    [SerializeField]
    private TextMeshProUGUI m_Label;

    [SerializeField]
    private RectTransform m_LabelTransform;

    private const float k_PunchAnimationTime = 1.5f;
    private const float k_PunchAnimationSize = 0.2f;
    private const int k_PunchAnimationVibrtion = 3;
    private const float k_ScaleInAnimationTime = 0.75f;
    private const float k_ScaleOutAnimationTime = 0.5f;
    private const float k_StartScaleOutTime = 1.5f;
    private const float k_DelayBeforeTimeSlows = 1f;
    private const float k_DelayAfterTimeSlows = 1.5f;
    private const float k_TimeScaleOnTutorialShow = 0.05f;
    private const float k_NormalTimeScale = 1f;

    private RectTransform m_RectTransform;

    private void Awake()
    {
        m_RectTransform = GetComponent<RectTransform>();
    }

    public IEnumerator ShowTutorial()
    {
        float programRuningTime = 0.0f;
        for (int i = 0; i < m_MessageDatas.Count; i++)
        {
            yield return new WaitForSecondsRealtime(m_MessageDatas[i].StartSecond - programRuningTime);
            programRuningTime += m_MessageDatas[i].StartSecond;

            m_RectTransform.anchoredPosition = m_MessageDatas[i].Position;
            m_Label.text = m_MessageDatas[i].PopUpMessage;

            var punchSize = new Vector3(k_PunchAnimationSize, k_PunchAnimationSize, k_PunchAnimationSize);
            var tweenFlow = DOTween.Sequence();

            tweenFlow.Insert(0f, gameObject.transform.DOScale(1f, k_ScaleInAnimationTime).SetEase(Ease.OutBack));
            tweenFlow.Insert(0f, m_LabelTransform.DOPunchScale(punchSize, k_PunchAnimationTime, k_PunchAnimationVibrtion).SetEase(Ease.InBack));
            tweenFlow.Insert(k_StartScaleOutTime, gameObject.transform.DOScale(0f, k_ScaleOutAnimationTime).SetEase(Ease.InBack));
            tweenFlow.Play();

            yield return new WaitForSecondsRealtime(k_StartScaleOutTime - k_DelayBeforeTimeSlows);
            programRuningTime += k_StartScaleOutTime - k_DelayBeforeTimeSlows;
            Time.timeScale = k_TimeScaleOnTutorialShow;

            yield return new WaitForSecondsRealtime(k_ScaleOutAnimationTime + k_DelayAfterTimeSlows);
            programRuningTime += k_ScaleOutAnimationTime;
            Time.timeScale = k_NormalTimeScale;
        }
    }
}