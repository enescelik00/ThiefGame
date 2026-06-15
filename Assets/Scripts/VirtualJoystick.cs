using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("Referanslar")]
    public RectTransform joystickBackground; // Joystick'in dýþ çemberi
    public RectTransform handle;             // Ýçteki hareketli top

    [Header("Ayarlar")]
    public float handleRange = 100f;
    public bool gizleKullanilmadiginda = true; // Býrakýlýnca ekrandan kaybolsun mu?

    private Vector2 inputVector = Vector2.zero;
    private CanvasGroup canvasGroup;

    public Vector2 InputVector => inputVector;

    void Awake()
    {
        // Gizleme efekti için CanvasGroup kullanýyoruz
        canvasGroup = joystickBackground.GetComponent<CanvasGroup>();

        if (gizleKullanilmadiginda && canvasGroup != null)
        {
            canvasGroup.alpha = 0f; // Baþlangýçta görünmez yap
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // 1. Dokunduðun yere joystick arka planýný ýþýnla
        joystickBackground.position = eventData.position;

        // 2. Görünür yap
        if (gizleKullanilmadiginda && canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
        }

        // 3. Ýlk dokunuþta direkt hareketi algýla
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        // Týklanan ekran koordinatýný, joystickBackground'un yerel koordinatýna çevir
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickBackground,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint
        );

        // Normalize et (-1 ile 1 arasýna çek)
        inputVector = new Vector2(
            localPoint.x / (joystickBackground.sizeDelta.x * 0.5f),
            localPoint.y / (joystickBackground.sizeDelta.y * 0.5f)
        );

        inputVector = Vector2.ClampMagnitude(inputVector, 1f);

        // Handle'ý (iç topu) görsel olarak taþý
        handle.anchoredPosition = new Vector2(
            inputVector.x * handleRange,
            inputVector.y * handleRange
        );
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Her þeyi sýfýrla
        inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;

        // Ýstersen tekrar görünmez yap
        if (gizleKullanilmadiginda && canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
        }
    }
}