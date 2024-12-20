using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using HyperCasual.Core;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace HyperCasual.Gameplay
{
    /// <summary>
    /// This View contains the splash screen functionalities
    /// </summary>
    public class SplashScreen : View
    {
        private const float k_FadeCloudsDuration = 1.5f;

        [SerializeField]
        private List<Image> _clouds;

        [SerializeField]
        private Transform _leftAnchor;

        [SerializeField]
        private Transform _rightAnchor;

        [SerializeField]
        private Dictionary<Image, Vector3> _cloudsStartPosition = new Dictionary<Image, Vector3>();

        public override void Initialize()
        {
            base.Initialize();
            foreach (var cloud in _clouds)
            {
                _cloudsStartPosition.Add(cloud, cloud.transform.position);
            }
        }

        public override void Show()
        {
            base.Show();
            foreach (var cloud in _clouds)
            {
                var isLeftAnchor = Convert.ToBoolean(Random.Range(0, 2));
                cloud.transform.DOMoveX(isLeftAnchor ? _leftAnchor.position.x : _rightAnchor.position.x, Random.Range(2f, 5f));
            }
        }

        public override void Hide()
        {
            if (gameObject.activeSelf)
            {
                StartCoroutine(HideCoroutine());
            }
            else
            {
                base.Hide();
            }
        }

        private IEnumerator HideCoroutine()
        {
            foreach (var cloud in _clouds)
            {
                cloud.CrossFadeAlpha(0f, k_FadeCloudsDuration, false);
            }

            yield return new WaitForSeconds(k_FadeCloudsDuration);

            base.Hide();

            foreach (var cloud in _clouds)
            {
                if (_cloudsStartPosition.ContainsKey(cloud))
                {
                    cloud.transform.position = _cloudsStartPosition[cloud];
                }

                cloud.color = new Color(cloud.color.r, cloud.color.g, cloud.color.b, 1f);
            }
        }
    }
}