using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class WireSlot : MonoBehaviour, IDropHandler
{
    [Header("Slot Ayarlarý")]
    public Color slotColor;

    void Start()
    {
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        Wire wire = eventData.pointerDrag.GetComponent<Wire>();
        if (wire != null)
        {
            wire.LockWire((RectTransform)transform, slotColor);
        }
    }
}