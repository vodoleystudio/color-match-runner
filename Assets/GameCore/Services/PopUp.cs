using DG.Tweening;
using GameCore.Data;
using UnityEngine;
using System;
using System.Collections.Generic;

public class PopUp : MonoBehaviour
{
    [Serializable]
    private class MassageData
    {
        [SerializeField]
        private MatchState m_MatchState;

        public MatchState MatchState => m_MatchState;

        [SerializeField]
        private GameObject m_PopUpMessage;

        public GameObject PopUpMessage => m_PopUpMessage;
    }

    [SerializeField]
    private float m_AnimationTime = 1f;

    [SerializeField]
    private float m_PunchAnimationTime = 10f;

    [SerializeField]
    private float m_PunchAnimationSize = 1.05f;

    [SerializeField]
    private List<MassageData> m_MassageData;

    public void Active(bool state)
    {
        gameObject.SetActive(state);
    }

    private void OnEnable()
    {
        gameObject.transform.DOScale(1f, m_AnimationTime);
        gameObject.transform.DOPunchScale(new Vector3(m_PunchAnimationSize, m_PunchAnimationSize, m_PunchAnimationSize), m_PunchAnimationTime);
    }

    public void MatchMassage(MatchState matchState)
    {
        foreach (var massage in m_MassageData)
        {
            if (massage.MatchState == matchState)
            {
                massage.PopUpMessage.SetActive(true);
            }
        }
    }
}