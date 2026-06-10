using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("References")]
    public RectTransform handle;

    [Header("Settings")]
    public float handleRange = 100f;

    private RectTransform backgroundRect;
    private Vector2 inputVector = Vector2.zero;

    public Vector2 InputVector => inputVector;

    void Awake()
    {
        backgroundRect = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Joystick: OnPointerDown tetiklendi");
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {

        Debug.Log("Joystick: OnDrag tetiklendi, pos: " + eventData.position);
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            backgroundRect,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint
        );

        // -1 ile 1 arasýna normalize et
        inputVector = new Vector2(
            localPoint.x / (backgroundRect.sizeDelta.x * 0.5f),
            localPoint.y / (backgroundRect.sizeDelta.y * 0.5f)
        );
        inputVector = Vector2.ClampMagnitude(inputVector, 1f);

        // Handle'ý görsel olarak taþý
        handle.anchoredPosition = new Vector2(
            inputVector.x * handleRange,
            inputVector.y * handleRange
        );
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }
}
