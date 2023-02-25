using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using GameCore.Services;
using GameCore.Data;
using DG.Tweening;
using System;

namespace HyperCasual.Runner
{
    /// <summary>
    /// A class used to control a player in a Runner
    /// game. Includes logic for player movement as well as
    /// other gameplay logic.
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        /// <summary> Returns the PlayerController. </summary>
        public static PlayerController Instance => s_Instance;

        private static PlayerController s_Instance;

        [SerializeField]
        private Animator m_Animator;

        public Animator Animator => m_Animator;

        [SerializeField]
        private SkinnedMeshRenderer m_SkinnedMeshRenderer;

        [SerializeField]
        private PlayerSpeedPreset m_PlayerSpeed = PlayerSpeedPreset.Medium;

        [SerializeField]
        private float m_CustomPlayerSpeed = 10.0f;

        [SerializeField]
        private float m_AccelerationSpeed = 10.0f;

        [SerializeField]
        private float m_DecelerationSpeed = 20.0f;

        [SerializeField]
        private float m_HorizontalSpeedFactor = 0.5f;

        [SerializeField]
        private float m_ScaleVelocity = 2.0f;

        [SerializeField]
        private bool m_AutoMoveForward = true;

        private Vector3 m_LastPosition;
        private float m_StartHeight;

        private const float k_MinimumScale = 0.1f;
        private static readonly string s_Speed = "Speed";

        private enum PlayerSpeedPreset
        {
            Slow,
            Medium,
            Fast,
            Custom
        }

        private Transform m_Transform;
        private Vector3 m_StartPosition;
        private bool m_HasInput;
        private float m_MaxXPosition;
        private float m_XPos;
        private float m_ZPos;
        private float m_TargetPosition;
        private float m_Speed;
        private float m_TargetSpeed;
        private Vector3 m_Scale;
        private Vector3 m_TargetScale;
        private Vector3 m_DefaultScale;

        private const float k_HalfWidth = 0.5f;

        /// <summary> The player's root Transform component. </summary>
        public Transform Transform => m_Transform;

        /// <summary> The player's current speed. </summary>
        public float Speed => m_Speed;

        /// <summary> The player's target speed. </summary>
        public float TargetSpeed => m_TargetSpeed;

        /// <summary> The player's minimum possible local scale. </summary>
        public float MinimumScale => k_MinimumScale;

        /// <summary> The player's current local scale. </summary>
        public Vector3 Scale => m_Scale;

        /// <summary> The player's target local scale. </summary>
        public Vector3 TargetScale => m_TargetScale;

        /// <summary> The player's default local scale. </summary>
        public Vector3 DefaultScale => m_DefaultScale;

        /// <summary> The player's default local height. </summary>
        public float StartHeight => m_StartHeight;

        /// <summary> The player's default local height. </summary>
        public float TargetPosition => m_TargetPosition;

        /// <summary> The player's maximum X position. </summary>
        public float MaxXPosition => m_MaxXPosition;

        private void Awake()
        {
            if (s_Instance != null && s_Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            s_Instance = this;

            Initialize();
        }

        /// <summary>
        /// Set up all necessary values for the PlayerController.
        /// </summary>
        ///
        public void MoveTo(AnimationType animationType, Transform endPositionTransform, float animationTime, Action onComplete = null)
        {
            AnimationEntityService.Instance.Play(animationType, Animator);
            transform.DOMove(endPositionTransform.position, animationTime).OnComplete(() =>
            {
                AnimationEntityService.Instance.Play(AnimationType.Idle, Animator);
                SetPosition(transform.position);
                onComplete?.Invoke();
            });
        }

        public void Initialize()
        {
            m_Transform = transform;
            m_StartPosition = m_Transform.position;
            m_DefaultScale = m_Transform.localScale;
            m_Scale = m_DefaultScale;
            m_TargetScale = m_Scale;

            if (m_SkinnedMeshRenderer != null)
            {
                m_StartHeight = m_SkinnedMeshRenderer.bounds.size.y;
            }
            else
            {
                m_StartHeight = 1.0f;
            }

            ResetSpeed();
            AnimationEntityService.Instance.Play(AnimationType.Run, m_Animator);
        }

        /// <summary>
        /// Returns the current default speed based on the currently
        /// selected PlayerSpeed preset.
        /// </summary>
        public float GetDefaultSpeed()
        {
            switch (m_PlayerSpeed)
            {
                case PlayerSpeedPreset.Slow:
                    return 5.0f;

                case PlayerSpeedPreset.Medium:
                    return 10.0f;

                case PlayerSpeedPreset.Fast:
                    return 20.0f;
            }

            return m_CustomPlayerSpeed;
        }

        /// <summary>
        /// Adjust the player's current speed
        /// </summary>
        public void AdjustSpeed(float speed)
        {
            m_TargetSpeed += speed;
            m_TargetSpeed = Mathf.Max(0.0f, m_TargetSpeed);
        }

        /// <summary>
        /// Reset the player's current speed to their default speed
        /// </summary>
        public void ResetSpeed()
        {
            m_Speed = 0.0f;
            m_TargetSpeed = GetDefaultSpeed();
        }

        public void Stop()
        {
            Debug.LogError("Stop");
            AnimationEntityService.Instance.Play(AnimationType.Idle, m_Animator);
            m_TargetSpeed = 0.0f;
            CancelMovement();
        }

        public void SetPosition(Vector3 position)
        {
            m_XPos = position.x;
            m_ZPos = position.z;
        }

        /// <summary>
        /// Adjust the player's current scale
        /// </summary>
        public void AdjustScale(float scale)
        {
            m_TargetScale += Vector3.one * scale;
            m_TargetScale = Vector3.Max(m_TargetScale, Vector3.one * k_MinimumScale);
        }

        /// <summary>
        /// Reset the player's current speed to their default speed
        /// </summary>
        public void ResetScale()
        {
            m_Scale = m_DefaultScale;
            m_TargetScale = m_DefaultScale;
        }

        public void SetColor(Color color)
        {
            m_SkinnedMeshRenderer.material.color = color;
            m_SkinnedMeshRenderer.material.SetColor("_1st_ShadeColor", color);
            m_SkinnedMeshRenderer.material.SetColor("_BaseColor", color);
        }

        public Color GetColor()
        {
            return m_SkinnedMeshRenderer.material.color;
        }

        /// <summary>
        /// Returns the player's transform component
        /// </summary>
        public Vector3 GetPlayerTop()
        {
            return m_Transform.position + Vector3.up * (m_StartHeight * m_Scale.y - m_StartHeight);
        }

        /// <summary>
        /// Sets the target X position of the player
        /// </summary>
        public void SetDeltaPosition(float normalizedDeltaPosition)
        {
            if (m_MaxXPosition == 0.0f)
            {
                Debug.LogError("Player cannot move because SetMaxXPosition has never been called or Level Width is 0. If you are in the LevelEditor scene, ensure a level has been loaded in the LevelEditor Window!");
            }

            float fullWidth = m_MaxXPosition * 2.0f;
            m_TargetPosition = m_TargetPosition + fullWidth * normalizedDeltaPosition;
            m_TargetPosition = Mathf.Clamp(m_TargetPosition, -m_MaxXPosition, m_MaxXPosition);
            m_HasInput = true;
        }

        /// <summary>
        /// Stops player movement
        /// </summary>
        public void CancelMovement()
        {
            m_HasInput = false;
        }

        /// <summary>
        /// Set the level width to keep the player constrained
        /// </summary>
        public void SetMaxXPosition(float levelWidth)
        {
            // Level is centered at X = 0, so the maximum player
            // X position is half of the level width
            m_MaxXPosition = levelWidth * k_HalfWidth;
        }

        /// <summary>
        /// Returns player to their starting position
        /// </summary>
        public void ResetPlayer()
        {
            m_Transform.position = m_StartPosition;
            m_XPos = 0.0f;
            m_ZPos = m_StartPosition.z;
            m_TargetPosition = 0.0f;

            m_LastPosition = m_Transform.position;

            m_HasInput = false;

            ResetSpeed();
            ResetScale();
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;

            // Update Scale

            if (!Approximately(m_Transform.localScale, m_TargetScale))
            {
                m_Scale = Vector3.Lerp(m_Scale, m_TargetScale, deltaTime * m_ScaleVelocity);
                m_Transform.localScale = m_Scale;
            }

            // Update Speed

            if (!m_AutoMoveForward && !m_HasInput)
            {
                Decelerate(deltaTime, 0.0f);
            }
            else if (m_TargetSpeed < m_Speed)
            {
                Decelerate(deltaTime, m_TargetSpeed);
            }
            else if (m_TargetSpeed > m_Speed)
            {
                Accelerate(deltaTime, m_TargetSpeed);
            }

            float speed = m_Speed * deltaTime;

            // Update position

            m_ZPos += speed;

            if (m_HasInput)
            {
                float horizontalSpeed = speed * m_HorizontalSpeedFactor;

                float newPositionTarget = Mathf.Lerp(m_XPos, m_TargetPosition, horizontalSpeed);
                float newPositionDifference = newPositionTarget - m_XPos;

                newPositionDifference = Mathf.Clamp(newPositionDifference, -horizontalSpeed, horizontalSpeed);

                m_XPos += newPositionDifference;
            }

            m_Transform.position = new Vector3(m_XPos, m_Transform.position.y, m_ZPos);

            if (m_Animator != null && deltaTime > 0.0f)
            {
                float distanceTravelledSinceLastFrame = (m_Transform.position - m_LastPosition).magnitude;
                float distancePerSecond = distanceTravelledSinceLastFrame / deltaTime;

                //m_Animator.SetFloat(s_Speed, distancePerSecond);
            }

            if (m_Transform.position != m_LastPosition)
            {
                m_Transform.forward = Vector3.Lerp(m_Transform.forward, (m_Transform.position - m_LastPosition).normalized, speed);
            }

            m_LastPosition = m_Transform.position;
        }

        private void Accelerate(float deltaTime, float targetSpeed)
        {
            m_Speed += deltaTime * m_AccelerationSpeed;
            m_Speed = Mathf.Min(m_Speed, targetSpeed);
        }

        private void Decelerate(float deltaTime, float targetSpeed)
        {
            m_Speed -= deltaTime * m_DecelerationSpeed;
            m_Speed = Mathf.Max(m_Speed, targetSpeed);
        }

        private bool Approximately(Vector3 a, Vector3 b)
        {
            return Mathf.Approximately(a.x, b.x) && Mathf.Approximately(a.y, b.y) && Mathf.Approximately(a.z, b.z);
        }
    }
}