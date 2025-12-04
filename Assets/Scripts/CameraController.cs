using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float dragSpeed = 1f;
    public float zoomSpeed = 10f;
    public float minZoom = 5f;
    public float maxZoom = 9f;

    // Harita sınırları
    public float minX = -5.1f;
    public float maxX = 5.1f;
    public float minY = -9f;
    public float maxY = 9f;

    private Vector3 dragOrigin;

    void Update()
    {
        HandleDrag();
        HandleZoom();
        ClampPosition();
    }

    void HandleDrag()
    {
        // Fare
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 difference = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
            Vector3 move = new Vector3(-difference.x * dragSpeed * Camera.main.orthographicSize,
                                       -difference.y * dragSpeed * Camera.main.orthographicSize, 0);
            transform.position += move;
            dragOrigin = Input.mousePosition;
        }

        // Dokunma
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                Vector3 move = new Vector3(-touch.deltaPosition.x * dragSpeed * Camera.main.orthographicSize,
                                           -touch.deltaPosition.y * dragSpeed * Camera.main.orthographicSize, 0);
                transform.position += move;
            }
        }
    }

    void HandleZoom()
    {
        // Fare
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f)
        {
            Camera.main.orthographicSize -= scroll * zoomSpeed * 100 * Time.deltaTime;
        }

        // Mobil pinch
        if (Input.touchCount == 2)
        {
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            Vector2 t0Prev = t0.position - t0.deltaPosition;
            Vector2 t1Prev = t1.position - t1.deltaPosition;

            float prevMag = (t0Prev - t1Prev).magnitude;
            float currMag = (t0.position - t1.position).magnitude;
            float difference = currMag - prevMag;

            Camera.main.orthographicSize -= difference * zoomSpeed * Time.deltaTime;
        }

        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minZoom, maxZoom);
    }

    void ClampPosition()
    {
        float camHeight = Camera.main.orthographicSize;
        float camWidth = Camera.main.orthographicSize * Camera.main.aspect;

        float clampedX = Mathf.Clamp(transform.position.x, minX + camWidth, maxX - camWidth);
        float clampedY = Mathf.Clamp(transform.position.y, minY + camHeight, maxY - camHeight);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}
