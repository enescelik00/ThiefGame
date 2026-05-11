using UnityEngine;

public class MiniGameController : MonoBehaviour
{
    // Bu fonksiyon, bu scriptin baðlý olduðu obje AKTÝF (Visible) edildiðinde otomatik çalýþýr.
    void OnEnable()
    {
        // 1. Oyunu Dondur (Arka plandaki fizik ve hareketler durur)
        Time.timeScale = 0f;

        // 2. Fareyi serbest býrak ve görünür yap (Arka plandaki kodlar fareyi gizlediyse diye)
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Debug.Log("Mini-Game Açýldý: Zaman Durdu!");
    }

    // Bu fonksiyon, obje KAPATILDIÐINDA veya SÝLÝNDÝÐÝNDE otomatik çalýþýr.
    void OnDisable()
    {
        // 1. Oyunu geri baþlat
        Time.timeScale = 1f;

        // Not: Eðer oyununda normalde fare gizliyse buraya Cursor.visible = false; ekleyebilirsin.
        Debug.Log("Mini-Game Kapandý: Oyun Akýyor!");
    }
}