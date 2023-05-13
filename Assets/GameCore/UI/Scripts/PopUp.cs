using DG.Tweening;
using GameCore.Data;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour
{
    [Serializable]
    private class MessageData
    {
        [SerializeField]
        private MatchState m_MatchState;

        public MatchState MatchState => m_MatchState;

        [SerializeField]
        private string m_PopUpMessage;

        public string PopUpMessage => m_PopUpMessage;

        [SerializeField]
        private Color m_Color;

        public Color Color => m_Color;
    }

    [SerializeField]
    private List<MessageData> m_MessageData;

    [SerializeField]
    private TextMeshProUGUI m_Label;

    [SerializeField]
    private RectTransform m_LabelTransform;

    [SerializeField]
    private Image m_Background;

    private const float k_PunchAnimationTime = 1.5f;
    private const float k_PunchAnimationSize = 0.2f;
    private const int k_PunchAnimationVibrtion = 3;
    private const float k_ScaleInAnimationTime = 0.75f;
    private const float k_ScaleOutAnimationTime = 0.5f;

    public void Active(bool state)
    {
        gameObject.SetActive(state);
    }

    private void OnEnable()
    {
        var punchSize = new Vector3(k_PunchAnimationSize, k_PunchAnimationSize, k_PunchAnimationSize);
        var tweenFlow = DOTween.Sequence();

        tweenFlow.Insert(0f, gameObject.transform.DOScale(1f, k_ScaleInAnimationTime).SetEase(Ease.OutBack));
        tweenFlow.Insert(0f, m_LabelTransform.DOPunchScale(punchSize, k_PunchAnimationTime, k_PunchAnimationVibrtion).SetEase(Ease.InBack));
        tweenFlow.Insert(0f, m_Background.transform.DOPunchScale(-punchSize, k_PunchAnimationTime, k_PunchAnimationVibrtion).SetEase(Ease.OutBack));
        tweenFlow.Insert(k_ScaleInAnimationTime + k_PunchAnimationTime, gameObject.transform.DOScale(0f, k_ScaleOutAnimationTime).SetEase(Ease.InBack));
        tweenFlow.Play();
    }

    private void OnDisable()
    {
        gameObject.transform.localScale = Vector3.zero;
    }

    public void MatchMessage(MatchState matchState)
    {
        foreach (var message in m_MessageData)
        {
            if (message.MatchState == matchState)
            {
                m_Label.text = message.PopUpMessage;
                m_Background.color = message.Color;
            }
        }
    }
}