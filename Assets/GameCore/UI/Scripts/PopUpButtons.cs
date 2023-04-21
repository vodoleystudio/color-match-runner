using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpButtons : MonoBehaviour
{
    private const float k_ScaleAnimationTime = 0.6f;
    private const float k_PunchAnimation = 0.6f;

    private const float k_OpenScalehAnimationSize = 1f;
    private const float k_CloseScaleAnimationSize = 0f;
    private const float k_PunchAnimationSize = -0.1f;

    private const int k_PunchAnimationVibrtion = 3;
    private const int k_PunchAnimationElasticy = 5;

    private const float k_StartButtonScaleAnimation = 0f;
    private const float k_StattButtonPunchAnimation = 0.5f;
    private const float k_StattImagePunchAnimation = 0.6f;

    [Serializable]
    public class ButtonData
    {
        [SerializeField]
        private RectTransform m_ButtonTransform;

        public RectTransform ButtonTransform => m_ButtonTransform;

        [SerializeField]
        private RectTransform m_ImageTransform;

        public RectTransform ImageTransform => m_ImageTransform;

        [SerializeField]
        private Button m_ButtonComponent;

        public Button ButtonComponent => m_ButtonComponent;
    }

    [SerializeField]
    private List<ButtonData> m_buttons;

    private void OnEnable()
    {
        ScaleAllButtons(k_OpenScalehAnimationSize, -k_PunchAnimationSize);
    }

    private void OnDisable()
    {
        foreach (var ButtonData in m_buttons)
        {
            ButtonData.ButtonTransform.DOScale(k_CloseScaleAnimationSize, k_ScaleAnimationTime);
            ActivateButtonComponent(ButtonData, false);
        }
    }

    private void ScaleAllButtons(float scaleSize, float punchSize)
    {
        var tweenSequance = DOTween.Sequence();
        var punch = new Vector3(punchSize, punchSize, punchSize);

        foreach (var buttonData in m_buttons)
        {
            tweenSequance.Insert(k_StartButtonScaleAnimation, buttonData.ButtonTransform.DOScale(scaleSize, k_ScaleAnimationTime));
            tweenSequance.Insert(k_StattButtonPunchAnimation, buttonData.ButtonTransform.DOPunchScale(punch, k_PunchAnimation, k_PunchAnimationVibrtion, k_PunchAnimationElasticy));
            tweenSequance.Insert(k_StattImagePunchAnimation, buttonData.ImageTransform.DOPunchScale(punch, k_PunchAnimation, k_PunchAnimationVibrtion, k_PunchAnimationElasticy).OnComplete(() =>
            {
                ActivateButtonComponent(buttonData, true);
            }));
            tweenSequance.Play();
        }
    }

    private void ActivateButtonComponent(ButtonData buttonData, bool state)
    {
        buttonData.ButtonComponent.enabled = state;
    }
}