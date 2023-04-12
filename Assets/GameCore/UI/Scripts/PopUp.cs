using DG.Tweening;
using GameCore.Data;
using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;

public class PopUp : MonoBehaviour
{
    [Serializable]
    private class MassageData
    {
        [SerializeField]
        private MatchState m_MatchState;

        public MatchState MatchState => m_MatchState;

        [SerializeField]
        private string m_PopUpMessage;

        public string PopUpMessage => m_PopUpMessage;
    }

    [SerializeField]
    private float m_AnimationTime = 1f;

    [SerializeField]
    private List<MassageData> m_MassageData;

    [SerializeField]
    private TextMeshProUGUI m_Label;

    [SerializeField]
    private RectTransform m_LabelTransform;

    private const float k_PunchAnimationTime = 0.8f;
    private const float k_PunchAnimationSize = -0.1f;
    private const int k_PunchAnimationVibrtion = 3;
    private const int k_PunchAnimationElasticy = 5;
    private const float k_ScaleAnimationTime = 0.6f;

    public void Active(bool state)
    {
        gameObject.SetActive(state);
    }

    private void OnEnable()
    {
        var punchSize = new Vector3(k_PunchAnimationSize, k_PunchAnimationSize, k_PunchAnimationSize);
        var tweenFlow = DOTween.Sequence();

        tweenFlow.Insert(0, gameObject.transform.DOScale(1f, k_ScaleAnimationTime));
        tweenFlow.Insert(0.8f, gameObject.transform.DOPunchScale(punchSize, k_PunchAnimationTime, k_PunchAnimationVibrtion, k_PunchAnimationElasticy));
        tweenFlow.Insert(0.7f, m_LabelTransform.DOPunchScale(-punchSize, k_PunchAnimationTime, k_PunchAnimationVibrtion, k_PunchAnimationElasticy));
        tweenFlow.Insert(2.7f + k_PunchAnimationTime, gameObject.transform.DOScale(0f, k_ScaleAnimationTime));
        tweenFlow.Play();
    }

    private void OnDisable()
    {
        gameObject.transform.localScale = Vector3.zero;
    }

    public void MatchMassage(MatchState matchState)
    {
        foreach (var massage in m_MassageData)
        {
            if (massage.MatchState == matchState)
            {
                m_Label.text = massage.PopUpMessage;
            }
        }
    }
}