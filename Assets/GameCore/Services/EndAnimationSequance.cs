using UnityEngine;

public class EndAnimationSequance : MonoBehaviour
{
    public static EndAnimationSequance Instance => s_Instance;
    private static EndAnimationSequance s_Instance;

    [SerializeField]
    private Camera m_MainCamera;
    [SerializeField]
    private Camera m_EndSceneCamera;
    public Transform GetEndSceneCameraTransform() => m_EndSceneCamera.transform;

    [SerializeField]
    private float m_MaxRotationOfXAngel;
    [SerializeField]
    private Transform m_ParentTransform;
    [SerializeField]
    public float m_Speed;

    private Vector3 XandYRotation = new Vector3(1f, 1f, 0f);
    private Vector3 YRotation = new Vector3(0f, 1f, 0f);

    public Transform GetParentObjectTransform() => m_ParentTransform;

    public bool IsCameraActive()
    {
        return m_EndSceneCamera.gameObject.activeSelf;
    }

    public void HideCamera()
    {
        m_EndSceneCamera.gameObject.SetActive(false);
    }

    public void ActivateCamera()
    {
        m_EndSceneCamera.gameObject.SetActive(true);
    }

    private void Awake()
    {
        SetupInstance();
        HideCamera();      
    }

    private void Update()
    {
        if (IsCameraActive() && transform.rotation.eulerAngles.x < m_MaxRotationOfXAngel)
        {
            RotateBaseOnSpaceWorld(m_ParentTransform, XandYRotation, m_Speed);
        }
        else if (IsCameraActive())
        {
            RotateBaseOnSpaceWorld(m_ParentTransform, YRotation, m_Speed);
        }
        //I don't now why but for some reason the z axis changes when i rotate the object so every frame i set it to zero 
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0f);
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

    private void RotateBaseOnSpaceWorld(Transform transform ,Vector3 direction ,float speed)
    {
        transform.Rotate(direction * speed, Space.World);
    }

    public void SetEndCameraPosition(Transform mainCamera ,Transform endSceneCamera)
    {
        endSceneCamera.transform.position = mainCamera.position;
        endSceneCamera.transform.rotation = mainCamera.rotation;
        endSceneCamera.transform.SetParent(gameObject.transform);
    }
}
