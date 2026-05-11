using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; // Renk kontrolü için þart

public class Wire : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Kablo Ayarlarý")]
    public RectTransform wireLine; // Sündürdüðümüz Image
    public Color wireColor; // Bu kablonun kimlik rengi

    [Header("UI Referanslarý")]
    private Canvas canvas;
    private Image lineImage; // Kablo çizgisinin Image bileþeni

    private bool isConnected = false;

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        lineImage = wireLine.GetComponent<Image>();
        // Oyuna baþlarken kablo rengini, soket rengine eþitle
        lineImage.color = wireColor;
        ResetWire(); // Baþlangýçta kapalý
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isConnected) return;

        Debug.Log("<color=cyan>FARE KAREYÝ TUTTU VE ÇEKÝYOR!</color>");
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isConnected) return;

        // Koordinat Dönüþümü
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)transform,
            eventData.position,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out Vector2 localMousePos);

        // UZUNLUK (Pisagor)
        float distance = Vector2.Distance(Vector2.zero, localMousePos);
        wireLine.sizeDelta = new Vector2(distance, wireLine.sizeDelta.y);

        // AÇI (Trigonometri)
        float angle = Mathf.Atan2(localMousePos.y, localMousePos.x) * Mathf.Rad2Deg;
        wireLine.localRotation = Quaternion.Euler(0, 0, angle);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
        
        if (isConnected)
        {
            return; // Kilitlendiyse hiçbir þey yapma, býrak orada kalsýn.
        }

        
        ResetWire();
    }

    // --- DIÞARIDAN ÇAÐRILACAK KÝLÝTLEME FONKSÝYONU ---
    public void LockWire(RectTransform targetSlot, Color targetColor)
    {
        // 1. Renk Kontrolü: Renkler tutuyor mu?
        if (wireColor != targetColor)
        {
            ResetWire(); // Yanlýþ renkse geri kaç
            return;
        }

        // 2. Kilitleme: Renkler doðru
        isConnected = true;

        WireTaskManager taskManager = GetComponentInParent<WireTaskManager>();
        if (taskManager != null)
        {
            taskManager.WireConnected();
        }

        // 3. Görsel Kilitlenme: Kabloyu tam karþý soketin merkezine sündür
        Vector2 localTargetPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)transform,
            targetSlot.position, // Fare konumu yerine soket konumu
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out localTargetPos);

        float distance = Vector2.Distance(Vector2.zero, localTargetPos);
        wireLine.sizeDelta = new Vector2(distance, wireLine.sizeDelta.y);

        float angle = Mathf.Atan2(localTargetPos.y, localTargetPos.x) * Mathf.Rad2Deg;
        wireLine.localRotation = Quaternion.Euler(0, 0, angle);

        // Buraya ileride "Bip!" veya "Görev Baþarýlý" sesi ekleyebiliriz.
        Debug.Log("Kablo Kilitlendi: " + wireColor);
    }

    private void ResetWire()
    {
        wireLine.sizeDelta = new Vector2(0, wireLine.sizeDelta.y);
        wireLine.localRotation = Quaternion.Euler(0, 0, 0);
    }
}