using DG.Tweening;
using UnityEngine;

public class EndAnimationSequance : MonoBehaviour
{
    public static EndAnimationSequance Instance => s_Instance;
    private static EndAnimationSequance s_Instance;

    [SerializeField]
    private Camera m_MainCamera;
    [SerializeField]
    private Camera m_EndSceneCamera;

    [SerializeField]
    private Transform m_ParentTransform;
    [SerializeField]
    public float m_RotaionSpeedOnYAxses;
    [SerializeField]
    public float m_RotationOfXAngel;

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

    private void SetupInstance()
    {
        if (s_Instance != null && s_Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        s_Instance = this;
    }

    private void Start()
    {
        RotateCameraBaseOnSpaceWorld(m_RotationOfXAngel, 0f, 0f);
    }

    public void Update()
    {
        if (IsCameraActive())
        {
            RotateCameraBaseOnSpaceWorld(0f, m_RotaionSpeedOnYAxses, 0f);
        }
    }

    private void RotateCameraBaseOnSpaceWorld(float xAxsis, float yAxsis, float zAxsis)
    {
        m_ParentTransform.Rotate(xAxsis, yAxsis, zAxsis, Space.World);
    }

    public void MoveCamera(Transform targettPosition, Transform endPosition, Transform cameraTransform, float time)
    {
        cameraTransform.DOMove(endPosition.position, time);
        cameraTransform.LookAt(targettPosition.position);
    }
}
