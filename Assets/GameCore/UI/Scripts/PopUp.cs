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
    private const float k_PunchAnimationSize = 0.05f;
    private const int k_PunchAnimationVibrtion = 3;
    private const int k_PunchAnimationElasticy = 5;

    public void Active(bool state)
    {
        gameObject.SetActive(state);
    }

    private void OnEnable()
    {
        gameObject.transform.DOScale(1f, 1f).OnComplete(() =>
        {
            gameObject.transform.DOPunchScale(new Vector3(k_PunchAnimationSize, k_PunchAnimationSize, k_PunchAnimationSize), k_PunchAnimationTime, k_PunchAnimationVibrtion, k_PunchAnimationElasticy).OnComplete(() =>
            {
                m_LabelTransform.DOPunchScale(new Vector3(k_PunchAnimationSize, k_PunchAnimationSize, k_PunchAnimationSize), k_PunchAnimationTime, k_PunchAnimationVibrtion, k_PunchAnimationElasticy);
            });
        });
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