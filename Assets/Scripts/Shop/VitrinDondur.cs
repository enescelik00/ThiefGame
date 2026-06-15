using UnityEngine;
using UnityEngine.EventSystems; // UI etkileþimleri için þart

public class VitrinDondur : MonoBehaviour, IDragHandler
{
    [Header("Baðlantýlar")]
    [Tooltip("Stüdyodaki Hýrsýz modelinin Transform'unu buraya sürükle")]
    public Transform donenModel;

    [Header("Ayarlar")]
    public float donusHizi = -0.5f; // Ters yöne dönmemesi için eksi deðer, hýzý buradan ayarlayabilirsin

    // Oyuncu bu objenin üzerinde parmaðýný/mouse'unu kaydýrdýkça bu fonksiyon tetiklenir
    public void OnDrag(PointerEventData eventData)
    {
        if (donenModel != null)
        {
            // Parmaðýn ekrandaki yatay (X) hareket miktarýný al
            float kaydirmaMiktari = eventData.delta.x;

            // Modeli kendi Y ekseni (yukarýdan aþaðýya geçen direk) etrafýnda döndür
            donenModel.Rotate(Vector3.up, kaydirmaMiktari * donusHizi, Space.World);
        }
    }
}
