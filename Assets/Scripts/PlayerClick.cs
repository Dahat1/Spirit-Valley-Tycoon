using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerClick : MonoBehaviour
{
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        // --- Mouse Tıklama ---
        if (Input.GetMouseButtonDown(0))
        {
            // Eğer UI üzerindeysek → bina tıklaması iptal
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            HandleClick(Input.mousePosition);
        }

        // --- Mobil Dokunma ---
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            // Eğer UI üzerindeysek → bina tıklaması iptal
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                return;

            HandleClick(Input.GetTouch(0).position);
        }
    }

    void HandleClick(Vector2 screenPosition)
    {
        Vector2 worldPos = cam.ScreenToWorldPoint(screenPosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

        if (hit.collider != null)
        {
            Building building = hit.collider.GetComponent<Building>();
            if (building != null)
            {
                building.OnClick();
            }
        }
    }
}
