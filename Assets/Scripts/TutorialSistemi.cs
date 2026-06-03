using UnityEngine;

public class TutorialSistemi : MonoBehaviour
{
    [Header("Oyun Baþlayýnca Açýlacak Arayüzler")]
    public GameObject saatCanvasi;
    public GameObject gorevCanvasi;

    void Start()
    {
        // 1. Oyun baþladýðý an diðer arayüzleri GÝZLE
        if (saatCanvasi != null) saatCanvasi.SetActive(false);
        if (gorevCanvasi != null) gorevCanvasi.SetActive(false);

        // 2. Zamaný dondur ve fareyi serbest býrak
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OyunaBasla()
    {
        // 1. Zamaný normal akýþýna döndür ve fareyi gizle
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 2. Oyun baþladýðý için arayüzleri (Saat ve Görevler) GÖRÜNÜR YAP
        if (saatCanvasi != null) saatCanvasi.SetActive(true);
        if (gorevCanvasi != null) gorevCanvasi.SetActive(true);

        // 3. Tutorial Canvas'ýný (kendisini) tamamen kapat
        gameObject.SetActive(false);
    }
}