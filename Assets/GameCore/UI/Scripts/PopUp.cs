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

    private float m_PunchAnimationTime = 1f;
    private float m_PunchAnimationSize = 0.05f;

    public void Active(bool state)
    {
        gameObject.SetActive(state);
    }

    private void OnEnable()
    {
        gameObject.transform.DOScale(1f, 1f).OnComplete(() =>
        {
            gameObject.transform.DOPunchScale(new Vector3(m_PunchAnimationSize, m_PunchAnimationSize, m_PunchAnimationSize), m_PunchAnimationTime, 1, 10);
        });
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