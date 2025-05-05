using UnityEngine;

public class Camera_Follow : MonoBehaviour
{
    [SerializeField] private GameObject target;

    [SerializeField] private float followSpeed = 2f;

    private Camera cam;
    [SerializeField] private float nonzoom = 60f;
    [SerializeField] private float zoom = 30f;
    [SerializeField] private float zoomSpeed = 5f;
    private float targetZoom;

    void Start()
    {
        cam = GetComponent<Camera>();
        targetZoom = cam.fieldOfView;
    }

    void LateUpdate()
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetZoom, Time.deltaTime * zoomSpeed);
        Follow();
    }

    void Follow()
    {
        if (target)
        {
            Vector3 desiredPosition = target.transform.position + new Vector3(0f, 4f, -4f);
            transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * followSpeed);
        }
    }


    public void Look(Transform targetToLook)
    {
        transform.LookAt(targetToLook);
    }

    public void ZoomIn()
    {
        targetZoom = zoom;
    }

    public void ZoomOut()
    {
        targetZoom = nonzoom;
    }
}
