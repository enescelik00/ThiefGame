using UnityEngine;

public class MiniGameController : MonoBehaviour
{
    private HirsizKontrol hirsiz;

    void Awake()
    {
        // Sahnedeki hýrsýzý otomatik olarak bulur (Sürükle-býrak yapmana gerek yok!)
        hirsiz = FindFirstObjectByType<HirsizKontrol>();
    }

    void OnEnable()
    {
        // 1. Zaman ARTIK DURMUYOR! Dünya yaþamaya devam ediyor.
        Time.timeScale = 1f;

        // 2. Fareyi serbest býrak ve görünür yap (Minigame oynamak için)
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // 3. Karakteri dondur
        if (hirsiz != null)
        {
            hirsiz.hackYapiyorMu = true;
        }

        Debug.Log("Sisteme Girildi: Karakter kilitlendi, NPC'ler hala hareket ediyor!");
    }

    void OnDisable()
    {
        // 1. Karakterin kilidini aç
        if (hirsiz != null)
        {
            hirsiz.hackYapiyorMu = false;
        }

        // 2. Fareyi gizle ve kilitle (Normal oyun moduna dönüþ)
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;

        Debug.Log("Sistemden Çýkýldý: Hýrsýz harekete hazýr!");
    }
}