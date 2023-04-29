using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PopUpButtons : MonoBehaviour
{
    private const float k_ScaleAnimationTime = 0.5f;
    private const float k_PunchAnimation = 0.5f;

    private const float k_OpenScaleAnimationSize = 1f;
    private const float k_CloseScaleAnimationSize = 0f;
    private const float k_PunchAnimationSize = -0.1f;

    private const int k_PunchAnimationVibrtion = 3;
    private const int k_PunchAnimationElasticy = 5;

    private const float k_StartButtonScaleAnimation = 0f;
    private const float k_StartButtonPunchAnimation = 0.5f;
    private const float k_StartImagePunchAnimation = 0.6f;

    [Serializable]
    public class ButtonData
    {
        [SerializeField]
        private RectTransform m_ButtonTransform;

        public RectTransform ButtonTransform => m_ButtonTransform;

        [SerializeField]
        private RectTransform m_ImageTransform;

        public RectTransform ImageTransform => m_ImageTransform;
    }

    [SerializeField]
    private List<ButtonData> m_buttons;

    private void OnEnable()
    {
        ScaleAllButtons(k_OpenScaleAnimationSize, -k_PunchAnimationSize);
    }

    private void OnDisable()
    {
        foreach (var ButtonData in m_buttons)
        {
            ButtonData.ButtonTransform.DOScale(k_CloseScaleAnimationSize, k_ScaleAnimationTime);
        }
    }

    private void ScaleAllButtons(float scaleSize, float punchSize)
    {
        var tweenSequance = DOTween.Sequence();
        var punch = new Vector3(punchSize, punchSize, punchSize);

        foreach (var buttonData in m_buttons)
        {
            tweenSequance.Insert(k_StartButtonScaleAnimation, buttonData.ButtonTransform.DOScale(scaleSize, k_ScaleAnimationTime));
            tweenSequance.Insert(k_StartButtonPunchAnimation, buttonData.ButtonTransform.DOPunchScale(punch, k_PunchAnimation, k_PunchAnimationVibrtion, k_PunchAnimationElasticy));
            tweenSequance.Insert(k_StartImagePunchAnimation, buttonData.ImageTransform.DOPunchScale(punch, k_PunchAnimation, k_PunchAnimationVibrtion, k_PunchAnimationElasticy));
            tweenSequance.Play();
        }
    }
}