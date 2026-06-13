using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Wire : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Kablo Ayarlarý")]
    public Color wireColor;

    [Tooltip("Kablonun çýktýðý port noktasý (soket sað kenarýndan offset)")]
    public float portOffsetX = 55f;

    [Tooltip("Sað soketin sol port noktasýna offset (negatif = sola)")]
    public float targetPortOffsetX = -55f;

    [Header("UI Referanslarý")]
    public RectTransform wireLine;

    private Canvas canvas;
    private Image lineImage;
    private bool isConnected = false;
    private Vector2 startLocalPos;

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        lineImage = wireLine.GetComponent<Image>();
        lineImage.color = wireColor;
        ResetWire();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isConnected) return;
        startLocalPos = new Vector2(portOffsetX, 0f);
        wireLine.anchoredPosition = startLocalPos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isConnected) return;
        UpdateWireVisual(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isConnected) return;
        ResetWire();
    }

    // bool döndürüyor — WireSlot baþarý/baþarýsýzlýk bilgisini alýyor
    public bool LockWire(RectTransform targetSlot, Color targetColor)
    {
        if (!ColorsMatch(wireColor, targetColor))
        {
            ResetWire();
            return false;
        }

        isConnected = true;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)transform,
            targetSlot.position,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out Vector2 targetLocalPos);

        

        StretchWire(startLocalPos, targetLocalPos);

        WireTaskManager taskManager = GetComponentInParent<WireTaskManager>();
        if (taskManager != null)
            taskManager.WireConnected();

        Debug.Log("Kablo Kilitlendi: " + wireColor);
        return true;
    }

    private void UpdateWireVisual(Vector2 screenPos)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)transform,
            screenPos,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out Vector2 mouseLocalPos);

        StretchWire(startLocalPos, mouseLocalPos);
    }

    private void StretchWire(Vector2 from, Vector2 to)
    {
        Vector2 delta = to - from;
        float distance = delta.magnitude;
        float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;

        wireLine.anchoredPosition = from;
        wireLine.sizeDelta = new Vector2(distance, wireLine.sizeDelta.y);
        wireLine.localRotation = Quaternion.Euler(0, 0, angle);
    }

    private void ResetWire()
    {
        wireLine.sizeDelta = new Vector2(0, wireLine.sizeDelta.y);
        wireLine.localRotation = Quaternion.Euler(0, 0, 0);
        wireLine.anchoredPosition = Vector2.zero;
    }

    private bool ColorsMatch(Color a, Color b)
    {
        return Mathf.Abs(a.r - b.r) < 0.01f &&
               Mathf.Abs(a.g - b.g) < 0.01f &&
               Mathf.Abs(a.b - b.b) < 0.01f;
    }
}