using DG.Tweening;
using GameCore.Data;
using UnityEngine;
using TMPro;
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
        private string m_Message;

        public string Message => m_Message;
    }

    [SerializeField]
    private float m_AnimationTime = 1f;

    [SerializeField]
    private List<MassageData> m_MassageData;

    [SerializeField]
    private TextMeshProUGUI m_Label;

    public void Active(bool state)
    {
        gameObject.SetActive(state);
    }

    private void OnEnable()
    {
        gameObject.transform.DOScale(1f, m_AnimationTime);
    }

    private void OnDisable()
    {
        gameObject.transform.DOScale(0f, m_AnimationTime);
    }

    public void MatchMassage(MatchState matchState)
    {
        foreach (var massage in m_MassageData)
        {
            if (massage.MatchState == matchState)
            {
                m_Label.text = massage.Message;
            }
        }
    }
}