using UnityEngine;

public class TutorialSistemi : MonoBehaviour
{
    [Header("Oyun Başlayınca Açılacak Arayüzler")]
    public GameObject saatCanvasi;
    public GameObject gorevCanvasi;

    void Start()
    {
        // 1. Oyun başladığı an diğer arayüzleri GİZLE
        if (saatCanvasi != null) saatCanvasi.SetActive(false);
        if (gorevCanvasi != null) gorevCanvasi.SetActive(false);

        // 2. Zamanı dondur ve fareyi serbest bırak
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OyunaBasla()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;  // ← değişti
        Cursor.visible = true;                    // ← değişti

        if (saatCanvasi != null) saatCanvasi.SetActive(true);
        if (gorevCanvasi != null) gorevCanvasi.SetActive(true);

        gameObject.SetActive(false);
    }
}