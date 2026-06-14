using UnityEngine;
using UnityEngine.Events;

public class MinigameTrigger : MonoBehaviour
{
    [Header("Minigame Ayarlarý")]
    public GameObject minigamePrefab;
    public MinigameDifficulty difficulty;

    [Header("Sonuçlar")]
    public UnityEvent OnUnlock;

    void StartMinigame()
    {
        // Butona basýldýðýnda butonun kendisini gizleyelim ki oyun oynanýrken ekranda kalmasýn
        InteractButtonManager.Instance.HideButton();

        Canvas mainCanvas = FindFirstObjectByType<Canvas>();
        GameObject spawnedMinigame;

        if (mainCanvas != null)
        {
            spawnedMinigame = Instantiate(minigamePrefab, mainCanvas.transform);

            RectTransform rect = spawnedMinigame.GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.anchoredPosition = Vector2.zero;
                rect.localScale = Vector3.one;
            }
        }
        else
        {
            spawnedMinigame = Instantiate(minigamePrefab);
        }

        MinigameBase minigameScript = spawnedMinigame.GetComponent<MinigameBase>();

        if (minigameScript != null)
        {
            minigameScript.SetupDifficulty(difficulty);

            // Baþarý durumunu dinle
            minigameScript.OnMinigameSuccess += HandleSuccess;

            // YENÝ EKLENEN KISIM: Çýkýþ (iptal) durumunu dinle
            minigameScript.OnMinigameCancel += HandleCancel;
        }
        else
        {
            Debug.LogError("Prefab içinde MinigameBase'den türeyen bir script yok!");
        }
    }

    void HandleSuccess()
    {
        Debug.Log("Minigame baþarýyla tamamlandý! Kasa/Kapý açýlýyor...");

        OnUnlock?.Invoke();

        this.enabled = false;

        // Obje açýldýðý için collider'ýný da kapatabiliriz (isteðe baðlý)
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;
    }

    // YENÝ EKLENEN FONKSÝYON: Minigame X tuþuyla kapatýldýðýnda çalýþýr
    void HandleCancel()
    {
        Debug.Log("Minigame iptal edildi, etkileþim butonu geri geliyor...");

        // Oyuncu hala etkileþim alanýnýn (trigger) içinde olduðu için butonu ve StartMinigame'i tekrar çaðýr
        InteractButtonManager.Instance.ShowButton(StartMinigame);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Oyuncu alana girdiðinde butonu göster ve butona basýldýðýnda StartMinigame fonksiyonunu çalýþtýr de
            InteractButtonManager.Instance.ShowButton(StartMinigame);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Oyuncu alandan çýkarsa butonu gizle
            InteractButtonManager.Instance.HideButton();
        }
    }
}