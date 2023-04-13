using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class PopUpButtons : MonoBehaviour
{
    private const float k_AnimationTime = 0.6f;

    [SerializeField]
    private List<RectTransform> m_buttons;

    private void OnEnable()
    {
        ScaleAllButtons(1f, k_AnimationTime);
    }

    private void OnDisable()
    {
        ScaleAllButtons(1f, k_AnimationTime);
    }

    private void ScaleAllButtons(float size, float duration)
    {
        foreach (var rectTransform in m_buttons)
        {
            rectTransform.DOScale(1f, k_AnimationTime);
        }
    }
}