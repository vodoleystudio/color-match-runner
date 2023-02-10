using UnityEngine;

public class EndAnimationSequence : MonoBehaviour
{
    public static EndAnimationSequence Instance => s_Instance;
    private static EndAnimationSequence s_Instance;

    [SerializeField]
    private Camera m_EndSceneCamera;

    [SerializeField]
    private float m_MaxRotationOfXAngel;

    [SerializeField]
    private Transform m_ParentTransform;

    [SerializeField]
    private float m_Speed;

    private Vector3 m_XandYRotation = new Vector3(1f, 1f, 0f);
    private Vector3 m_YRotation = new Vector3(0f, 1f, 0f);

    public void HideCamera()
    {
        m_EndSceneCamera.gameObject.SetActive(false);
    }

    public void ActivateCamera(Transform mainCamera)
    {
        m_EndSceneCamera.transform.position = mainCamera.position;
        m_EndSceneCamera.transform.rotation = mainCamera.rotation;
        m_EndSceneCamera.gameObject.SetActive(true);
    }

    public void SetParentPosition(Transform parentTransform)
    {
        transform.position = parentTransform.position;
        transform.rotation = parentTransform.rotation;
    }

    private void Awake()
    {
        SetupInstance();
        HideCamera();
    }

    private void Update()
    {
        if (m_EndSceneCamera.gameObject.activeSelf)
        {
            if (transform.rotation.eulerAngles.x < m_MaxRotationOfXAngel)
            {
                RotateBaseOnSpaceWorld(m_ParentTransform, m_XandYRotation, m_Speed);

                //I don't now why but for some reason the z axis changes when I rotate the object so every frame i set it to zero
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0f);
            }
            else
            {
                RotateBaseOnSpaceWorld(m_ParentTransform, m_YRotation, m_Speed);
            }
        }
    }

    private void SetupInstance()
    {
        if (s_Instance != null && s_Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        s_Instance = this;
    }

    private void RotateBaseOnSpaceWorld(Transform transform, Vector3 direction, float speed)
    {
        transform.Rotate(direction * speed * Time.deltaTime, Space.World);
    }
}