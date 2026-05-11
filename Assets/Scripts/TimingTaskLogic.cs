using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TimingTaskLogic : MonoBehaviour, IPointerClickHandler
{
    [Header("UI Referanslarý")]
    public RectTransform pointerHolder; // Üçgeni taþýyan boþ kutu (Holder)
    public Image targetZoneImage; // Kýrmýzý alan (Filled Image)

    [Header("Oyun Ayarlarý")]
    public float rotationSpeed = 150f;
    public float targetZoneWidth = 45f;
    public int requiredSuccesses = 3;

    private float currentPointerAngle = 0f;
    private float targetStartAngle;
    private bool isTaskComplete = false;
    private int currentSuccesses = 0;

    void OnEnable()
    {
        isTaskComplete = false;
        currentSuccesses = 0;
        SetupNewTarget();
    }

    void SetupNewTarget()
    {
        // Hedefi rastgele bir açýya koy (0-360)
        targetStartAngle = Random.Range(0f, 360f);

        // Görseli o açýya döndür
        targetZoneImage.rectTransform.localRotation = Quaternion.Euler(0f, 0f, targetStartAngle);

        // Geniþliðini ayarla
        targetZoneImage.fillAmount = targetZoneWidth / 360f;
    }

    void Update()
    {
        if (isTaskComplete) return;

        // Üçgeni saat yönünde döndür
        currentPointerAngle -= rotationSpeed * Time.unscaledDeltaTime;

        // Açýyý 0-360 arasýnda normalize et
        currentPointerAngle = (currentPointerAngle % 360f + 360f) % 360f;

        // Holder'ý döndür
        pointerHolder.localRotation = Quaternion.Euler(0f, 0f, currentPointerAngle);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isTaskComplete) return;

        // --- YENÝLENMÝÞ MATEMATÝKSEL KONTROL ---
        // Unity'nin 'Filled' Image (Top Origin) mantýðý saat yönünde iþlediði için
        // Hedef alan aslýnda [targetStartAngle - targetZoneWidth] ile [targetStartAngle] arasýndadýr.

        if (IsAngleInRange(currentPointerAngle, targetStartAngle, targetZoneWidth))
        {
            currentSuccesses++;
            Debug.Log("<color=cyan>BAÞARILI! Kombo: " + currentSuccesses + "</color>");

            if (currentSuccesses >= requiredSuccesses)
            {
                TaskComplete();
            }
            else
            {
                SetupNewTarget();
            }
        }
        else
        {
            currentSuccesses = 0;
            Debug.Log("<color=red>ISKALADIN!</color> Kombo sýfýrlandý.");
            SetupNewTarget();
        }
    }

    // Bu fonksiyon artýk 'Filled' mantýðýna (saat yönüne) göre çalýþýyor
    bool IsAngleInRange(float pointerAngle, float startAngle, float width)
    {
        // Hedefin bitiþ açýsýný (saat yönünde geriye doðru) hesapla
        float endAngle = (startAngle - width + 360f) % 360f;
        float s = (startAngle + 360f) % 360f;
        float p = (pointerAngle + 360f) % 360f;

        // Wraparound (Sýfýr noktasýný aþma) durumu kontrolü
        if (endAngle > s)
        {
            return (p <= s || p >= endAngle);
        }
        else
        {
            return (p <= s && p >= endAngle);
        }
    }

    void TaskComplete()
    {
        isTaskComplete = true;
        Debug.Log("<color=green>HACK TAMAMLANDI!</color>");
        Invoke("DelayedClosePanel", 0.5f);
    }

    void DelayedClosePanel()
    {
        gameObject.SetActive(false);
    }
}