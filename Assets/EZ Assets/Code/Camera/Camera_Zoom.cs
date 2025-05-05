using UnityEngine;

public class Camera_Zoom : MonoBehaviour
{
    private Camera cam;

    [SerializeField] private float nonzoom;
    [SerializeField] private float zoom;
    [SerializeField] private float zoomSpeed = 5f;

    private float targetZoom;

    void Start()
    {
        cam = GetComponent<Camera>();
        targetZoom = cam.fieldOfView;
    }

    void Update()
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetZoom, Time.deltaTime * zoomSpeed);
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

