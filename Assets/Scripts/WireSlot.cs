using UnityEngine;
using UnityEngine.EventSystems; // IDropHandler için þart
using UnityEngine.UI;

// IDropHandler interfacini ekliyoruz.
// Bu interface, Raycast Target'ý açýk olan bu objenin üstüne bir þey 
// býrakýldýðýnda (OnEndDrag) tetiklenir.
public class WireSlot : MonoBehaviour, IDropHandler
{
    [Header("Slot Ayarlarý")]
    public Color slotColor; // Bu soketin kimlik rengi

    public void OnDrop(PointerEventData eventData)
    {
        // Býrakýlan objenin (sol soketin) içindeki Wire scriptini yakala
        if (eventData.pointerDrag != null)
        {
            Wire wire = eventData.pointerDrag.GetComponent<Wire>();

            if (wire != null)
            {
                // Yakaladýðýmýz scripte "Beni kilitle, renklerim de bu" sinyalini gönder.
                wire.LockWire((RectTransform)transform, slotColor);
            }
        }
    }
}