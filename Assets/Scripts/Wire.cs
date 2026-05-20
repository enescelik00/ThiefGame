using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Wire : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Kablo Ayarlarý")]
    public Color wireColor;

    // Port offseti: soketin sað kenarýndaki port noktasý (local space)
    // Inspector'dan ayarla, örneðin X: 55, Y: 0 (soket geniþliðinin yarýsý kadar)
    [Tooltip("Kablonun çýktýðý port noktasý (soket merkezinden offset, pixel)")]
    public Vector2 portOffset = new Vector2(55f, 0f);

    [Header("LineRenderer Referansý")]
    // Her Node_Left altýna boþ bir child GameObject ekle,
    // ona LineRenderer component'i ver, buraya sürükle
    public LineRenderer lineRenderer;

    [Header("Bezier Eðrisi")]
    [Range(10, 50)]
    public int bezierSegments = 20; // Eðri pürüzsüzlüðü

    private Canvas canvas;
    private bool isConnected = false;
    private Vector3 startWorldPos; // Kablonun çýkýþ noktasý (world space)

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();

        // LineRenderer'ý ayarla
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = bezierSegments;
            lineRenderer.startColor = wireColor;
            lineRenderer.endColor = wireColor;
            lineRenderer.startWidth = 6f;
            lineRenderer.endWidth = 6f;
            lineRenderer.useWorldSpace = true;
            lineRenderer.enabled = false; // Baþta gizli
        }

        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isConnected) return;

        // Çýkýþ noktasýný hesapla: soket merkezi + port offseti
        startWorldPos = GetPortWorldPosition();

        if (lineRenderer != null)
            lineRenderer.enabled = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isConnected) return;

        Vector3 mouseWorldPos = GetMouseWorldPosition(eventData.position);
        DrawBezier(startWorldPos, mouseWorldPos);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isConnected) return;
        ResetWire();
    }

    // WireSlot tarafýndan çaðrýlýr
    public void LockWire(RectTransform targetSlot, Color targetColor)
    {
        // Renk kontrolü
        if (!ColorsMatch(wireColor, targetColor))
        {
            ResetWire();
            return;
        }

        isConnected = true;

        // Hedef soketin port noktasýna kilitle
        // Sað soket için portOffset X'i negatif al (sola bakan port)
        Vector3 targetPortPos = GetSlotPortWorldPosition(targetSlot);
        DrawBezier(startWorldPos, targetPortPos);

        // Görevi bildir
        WireTaskManager taskManager = GetComponentInParent<WireTaskManager>();
        if (taskManager != null)
            taskManager.WireConnected();

        Debug.Log("Kablo Kilitlendi: " + wireColor);
    }

    private void ResetWire()
    {
        if (lineRenderer != null)
            lineRenderer.enabled = false;
    }

    // ---- YARDIMCI FONKSÝYONLAR ----

    // Bu soketin çýkýþ portunu world pozisyonunda döndürür
    private Vector3 GetPortWorldPosition()
    {
        RectTransform rt = (RectTransform)transform;
        // portOffset'i canvas scale'iyle dünya koordinatýna çevir
        Vector2 scaledOffset = portOffset * canvas.transform.localScale;
        return rt.position + new Vector3(scaledOffset.x, scaledOffset.y, 0);
    }

    // Hedef (sað) soketin giriþ portunu world pozisyonunda döndürür
    private Vector3 GetSlotPortWorldPosition(RectTransform targetSlot)
    {
        // Sað soket için port sola bakýyor, X offseti negatif
        Vector2 scaledOffset = new Vector2(-portOffset.x, portOffset.y) * canvas.transform.localScale;
        return targetSlot.position + new Vector3(scaledOffset.x, scaledOffset.y, 0);
    }

    // Fare pozisyonunu world koordinatýna çevirir
    private Vector3 GetMouseWorldPosition(Vector2 screenPos)
    {
        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            // Overlay modda: ekran koordinatýný direkt kullan, Z'yi soketle eþle
            Vector3 worldPos = new Vector3(screenPos.x, screenPos.y, startWorldPos.z);
            // Canvas scale'ini hesaba kat
            // Overlay'da screen pos zaten world pos gibi davranýr
            return worldPos;
        }
        else
        {
            // Camera modda: RectTransform utility kullan
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                (RectTransform)canvas.transform,
                screenPos,
                canvas.worldCamera,
                out Vector3 worldPos);
            return worldPos;
        }
    }

    // Kübik Bezier eðrisi çizer
    // Kontrol noktalarý: baþlangýçtan yatay uzanýr, sondan yatay gelir
    private void DrawBezier(Vector3 start, Vector3 end)
    {
        if (lineRenderer == null) return;

        // Mesafeye göre eðri yoðunluðu (uzaktaysa daha fazla kývrýlsýn)
        float dist = Mathf.Abs(end.x - start.x);
        float curvature = Mathf.Clamp(dist * 0.4f, 30f, 200f);

        // Kontrol noktalarý: çýkýþtan saða, giriþten sola
        Vector3 ctrl1 = start + new Vector3(curvature, 0, 0);
        Vector3 ctrl2 = end - new Vector3(curvature, 0, 0);

        lineRenderer.positionCount = bezierSegments;

        for (int i = 0; i < bezierSegments; i++)
        {
            float t = i / (float)(bezierSegments - 1);
            lineRenderer.SetPosition(i, CubicBezier(start, ctrl1, ctrl2, end, t));
        }
    }

    // Kübik Bezier formülü
    private Vector3 CubicBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float u = 1 - t;
        return u * u * u * p0
             + 3 * u * u * t * p1
             + 3 * u * t * t * p2
             + t * t * t * p3;
    }

    // Renk karþýlaþtýrmasý (float hassasiyet toleransýyla)
    private bool ColorsMatch(Color a, Color b)
    {
        return Mathf.Abs(a.r - b.r) < 0.01f &&
               Mathf.Abs(a.g - b.g) < 0.01f &&
               Mathf.Abs(a.b - b.b) < 0.01f;
    }
}