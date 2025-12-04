using UnityEngine;

[ExecuteAlways]  // Editör ve oyun sırasında çalışır
[RequireComponent(typeof(RectTransform))]
public class AutoStretchFullScreen : MonoBehaviour
{
    void Update()
    {
        RectTransform rt = GetComponent<RectTransform>();

        // Canvas boyutuna göre stretch ayarla
        rt.anchorMin = Vector2.zero;   // sol-alt (0,0)
        rt.anchorMax = Vector2.one;    // sağ-üst (1,1)
        rt.offsetMin = Vector2.zero;   // sol-alt offset = 0
        rt.offsetMax = Vector2.zero;   // sağ-üst offset = 0
        rt.localPosition = Vector3.zero;
    }
}
