using DG.Tweening;
using GameCore.Data;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    [SerializeField]
    private float m_AnimationTime = 1f;

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
        switch (matchState)
        {
            //case MatchState.:

        }
    }
}
