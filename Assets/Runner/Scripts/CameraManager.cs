using UnityEngine;

namespace HyperCasual.Runner
{
    /// <summary>
    /// A class used to manage camera movement
    /// in a Runner game.
    /// </summary>
    [ExecuteInEditMode]
    public class CameraManager : MonoBehaviour
    {
        /// <summary>
        /// Returns the CameraManager.
        /// </summary>
        public static CameraManager Instance => s_Instance;

        private static CameraManager s_Instance;

        [SerializeField]
        private CameraAnglePreset m_CameraAnglePreset = CameraAnglePreset.Behind;

        [SerializeField]
        private Vector3 m_Offset;

        [SerializeField]
        private Vector3 m_LookAtOffset;

        [SerializeField]
        private bool m_LockCameraPosition;

        [SerializeField]
        private bool m_SmoothCameraFollow;

        [SerializeField]
        private float m_SmoothCameraFollowStrength = 10.0f;

        private enum CameraAnglePreset
        {
            Behind,
            Overhead,
            Side,
            FirstPerson,
            Custom,
        }

        private Vector3[] m_PresetOffsets = new Vector3[]
        {
            new Vector3(0.0f, 5.0f, -9.0f), // Behind
            new Vector3(0.0f, 9.0f, -5.0f), // Overhead
            new Vector3(5.0f, 5.0f, -8.0f), // Side
            new Vector3(0.0f, 1.0f, 0.0f),  // FirstPerson
            Vector3.zero                    // Custom
        };

        private Vector3[] m_PresetLookAtOffsets = new Vector3[]
        {
            new Vector3(0.0f, 2.0f, 6.0f),  // Behind
            new Vector3(0.0f, 0.0f, 4.0f),  // Overhead
            new Vector3(-0.5f, 1.0f, 2.0f), // Side
            new Vector4(0.0f, 1.0f, 2.0f),  // FirstPerson
            Vector3.zero                    // Custom
        };

        private bool[] m_PresetLockCameraPosition = new bool[]
        {
            false, // Behind
            false, // Overhead
            true,  // Side
            false, // FirstPerson
            false  // Custom
        };

        private Transform m_Transform;
        private Vector3 m_PrevLookAtOffset;

        private static readonly Vector3 k_CenteredScale = new Vector3(0.0f, 1.0f, 1.0f);

        private void Awake()
        {
            SetupInstance();
        }

        private void OnEnable()
        {
            SetupInstance();
        }

        private void SetupInstance()
        {
            if (s_Instance != null && s_Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            s_Instance = this;
            m_Transform = transform;
        }

        /// <summary>
        /// Reset the camera to its starting position relative
        /// to the player.
        /// </summary>

        public Transform GetCameraTransform() => m_Transform;

        public void ResetCamera()
        {
            SetCameraPositionAndOrientation(false);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private Vector3 GetCameraOffset()
        {
            return m_PresetOffsets[(int)m_CameraAnglePreset] + m_Offset;
        }

        private Vector3 GetCameraLookAtOffset()
        {
            return m_PresetLookAtOffsets[(int)m_CameraAnglePreset] + m_LookAtOffset;
        }

        private bool GetCameraLockStatus()
        {
            if (m_LockCameraPosition)
            {
                return true;
            }

            return m_PresetLockCameraPosition[(int)m_CameraAnglePreset];
        }

        private Vector3 GetPlayerPosition()
        {
            Vector3 playerPosition = Vector3.up;
            if (PlayerController.Instance != null)
            {
                playerPosition = PlayerController.Instance.GetPlayerTop();
            }

            if (GetCameraLockStatus())
            {
                playerPosition = Vector3.Scale(playerPosition, k_CenteredScale);
            }

            return playerPosition;
        }

        private void LateUpdate()
        {
            if (m_Transform == null)
            {
                return;
            }

            SetCameraPositionAndOrientation(m_SmoothCameraFollow);
        }

        private void SetCameraPositionAndOrientation(bool smoothCameraFollow)
        {
            Vector3 playerPosition = GetPlayerPosition();

            Vector3 offset = playerPosition + GetCameraOffset();
            Vector3 lookAtOffset = playerPosition + GetCameraLookAtOffset();

            if (smoothCameraFollow)
            {
                float lerpAmound = Time.deltaTime * m_SmoothCameraFollowStrength;

                m_Transform.position = Vector3.Lerp(m_Transform.position, offset, lerpAmound);
                m_Transform.LookAt(Vector3.Lerp(m_Transform.position + m_Transform.forward, lookAtOffset, lerpAmound));

                m_Transform.position = new Vector3(m_Transform.position.x, m_Transform.position.y, offset.z);
            }
            else
            {
                m_Transform.position = playerPosition + GetCameraOffset();
                m_Transform.LookAt(lookAtOffset);
            }
        }
    }
}