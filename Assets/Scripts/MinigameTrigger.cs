using UnityEngine;
using UnityEngine.Events;

public class MinigameTrigger : MonoBehaviour
{
    [Header("Minigame Ayarlarý")]
    public GameObject minigamePrefab; // Hangi minigame açýlacak? (Inspector'dan sürükle)
    public MinigameDifficulty difficulty; // Zorluk ne olacak? (Inspector'dan seç)

    [Header("Sonuçlar")]
    public UnityEvent OnUnlock; // Minigame geçilince ne olsun? (Kapý açýlsýn animasyonu vb.)

    private bool isPlayerNear = false;

    void Update()
    {
        // Oyuncu yakýndaysa ve E tuþuna basarsa
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            StartMinigame();
        }
    }

    void StartMinigame()
    {
        // 1. Sahnede var olan ana UI Canvas'ý bul
        Canvas mainCanvas = FindFirstObjectByType<Canvas>();
        GameObject spawnedMinigame;

        // 2. Prefabý Canvas'ýn içine (alt objesi olarak) üret
        if (mainCanvas != null)
        {
            spawnedMinigame = Instantiate(minigamePrefab, mainCanvas.transform);

            // UI'ýn ekranýn ortasýna tam oturmasý için pozisyonunu sýfýrla
            RectTransform rect = spawnedMinigame.GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.anchoredPosition = Vector2.zero;
                rect.localScale = Vector3.one;
            }
        }
        else
        {
            // Eðer sahnede henüz hiç Canvas yoksa normal üret (Güvenlik önlemi)
            spawnedMinigame = Instantiate(minigamePrefab);
        }

        // 3. Ýçindeki MinigameBase scriptini bul
        MinigameBase minigameScript = spawnedMinigame.GetComponent<MinigameBase>();

        if (minigameScript != null)
        {
            minigameScript.SetupDifficulty(difficulty);
            minigameScript.OnMinigameSuccess += HandleSuccess;
        }
        else
        {
            Debug.LogError("Prefab içinde MinigameBase'den türeyen bir script yok!");
        }
    }

    void HandleSuccess()
    {
        Debug.Log("Minigame baþarýyla tamamlandý! Kasa/Kapý açýlýyor...");

        // 1. Oyuncunun hareketini geri aç

        // 2. Inspector'dan atadýðýn olaylarý tetikle (Örn: Kapý animasyonunu oynat)
        OnUnlock?.Invoke();

        // 3. Bu tetikleyiciyi kapat ki tekrar ayný kasayý açmaya çalýþmasýn
        this.enabled = false;
    }

    // Oyuncunun menzile girip girmediðini kontrol etme
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) isPlayerNear = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) isPlayerNear = false;
    }
}