using DG.Tweening;
using GameCore.Data;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
    }

    [SerializeField]
    private List<MessageData> m_MessageData;

    [SerializeField]
    private TextMeshProUGUI m_Label;

    [SerializeField]
    private RectTransform m_LabelTransform;

    private const float k_PunchAnimationTime = 0.8f;
    private const float k_PunchAnimationSize = -0.1f;
    private const int k_PunchAnimationVibrtion = 3;
    private const int k_PunchAnimationElasticy = 5;
    private const float k_ScaleAnimationTime = 0.6f;

    private const float k_StartBackGroundScaleAnimationTime = 0f;
    private const float k_StartMessagePunchAnimationTime = 0.7f;
    private const float k_StartBackGroundPunchAnimationTime = 0.8f;

    private const float k_StartBackGroundCloseAnimationTime = 2.7f;

    public void Active(bool state)
    {
        gameObject.SetActive(state);
    }

    private void OnEnable()
    {
        var punchSize = new Vector3(k_PunchAnimationSize, k_PunchAnimationSize, k_PunchAnimationSize);
        var tweenFlow = DOTween.Sequence();

        tweenFlow.Insert(k_StartBackGroundScaleAnimationTime, gameObject.transform.DOScale(1f, k_ScaleAnimationTime));
        tweenFlow.Insert(k_StartMessagePunchAnimationTime, m_LabelTransform.DOPunchScale(-punchSize, k_PunchAnimationTime, k_PunchAnimationVibrtion, k_PunchAnimationElasticy));
        tweenFlow.Insert(k_StartBackGroundPunchAnimationTime, gameObject.transform.DOPunchScale(punchSize, k_PunchAnimationTime, k_PunchAnimationVibrtion, k_PunchAnimationElasticy));
        tweenFlow.Insert(k_StartBackGroundCloseAnimationTime + k_PunchAnimationTime, gameObject.transform.DOScale(0f, k_ScaleAnimationTime));
        tweenFlow.Play();
    }

    private void OnDisable()
    {
        gameObject.transform.localScale = Vector3.zero;
    }

    public void MatchMassage(MatchState matchState)
    {
        foreach (var message in m_MessageData)
        {
            if (message.MatchState == matchState)
            {
                m_Label.text = message.PopUpMessage;
            }
        }
    }
}