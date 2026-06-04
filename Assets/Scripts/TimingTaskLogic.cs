using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// 1. MonoBehaviour yerine MinigameBase'den miras alýyoruz
public class TimingTaskLogic : MinigameBase, IPointerClickHandler
{
    [Header("UI Referanslarý")]
    public RectTransform pointerHolder;
    public RectTransform targetGroup;
    public Image greenZoneImage;
    public Image redZoneImage;

    [Header("Oyun / Boyut Ayarlarý")]
    public float rotationSpeed = 200f;
    public float minGreenAngle = 25f;
    public float maxGreenAngle = 55f;

    [Tooltip("Kýrmýzý alan yeþilin kaç katý olacak? (Örn: 2 ise kýrmýzý alan yeþilin tam 2 katý olur)")]
    public float redZoneMultiplier = 2f;

    public int requiredSuccesses = 3;

    private float currentPointerAngle = 0f;
    private float currentTargetAngle = 0f;
    private float currentGreenWidth = 0f;
    private float currentOffset = 0f;

    private bool isTaskComplete = false;
    private int currentSuccesses = 0;

    void OnEnable()
    {
        isTaskComplete = false;
        currentSuccesses = 0;
        // NOT: SetupNewTarget() fonksiyonunu buradan kaldýrdýk, 
        // çünkü artýk tetikleyiciden zorluk ayarý gelince SetupDifficulty içinden çaðrýlacak.
    }

    // 2. Dýþarýdan tetikleyici (kasa/kapý) tarafýndan çaðrýlan zorluk ayarý fonksiyonu
    public override void SetupDifficulty(MinigameDifficulty difficulty)
    {
        base.SetupDifficulty(difficulty);

        // Kasadan gelen zorluða göre oyun parametrelerini dinamik olarak deðiþtiriyoruz
        if (difficulty == MinigameDifficulty.Easy)
        {
            rotationSpeed = 150f;       // Daha yavaþ dönen ibre
            requiredSuccesses = 2;      // 2 baþarýlý kombo yeterli
            minGreenAngle = 40f;        // Yeþil alan daha geniþ
            maxGreenAngle = 60f;
        }
        else if (difficulty == MinigameDifficulty.Medium)
        {
            rotationSpeed = 220f;       // Orta hýz
            requiredSuccesses = 3;      // 3 baþarýlý kombo
            minGreenAngle = 30f;        // Orta geniþlikte yeþil alan
            maxGreenAngle = 50f;
        }
        else if (difficulty == MinigameDifficulty.Hard)
        {
            rotationSpeed = 300f;       // Çok hýzlý dönen ibre
            requiredSuccesses = 4;      // Üst üste 4 kombo þart
            minGreenAngle = 18f;        // Ýncecik, vurmasý zor yeþil alan
            maxGreenAngle = 30f;
        }

        // Parametreler ayarlandýktan sonra ilk hedefi oluþtur
        SetupNewTarget();
    }

    void SetupNewTarget()
    {
        // 1. Yeþil alanýn rastgele boyutunu belirle
        currentGreenWidth = Random.Range(minGreenAngle, maxGreenAngle);

        // 2. Kýrmýzý alanýn boyutunu yeþile ORANTILI olarak hesapla
        float currentRedWidth = currentGreenWidth * redZoneMultiplier;

        currentTargetAngle = Random.Range(0f, 360f);
        targetGroup.localRotation = Quaternion.Euler(0f, 0f, currentTargetAngle);

        greenZoneImage.fillAmount = currentGreenWidth / 360f;
        redZoneImage.fillAmount = currentRedWidth / 360f;

        // 3. Yeþili Kýrmýzýnýn Ortasýna Kaydýr (Orantýlý sisteme göre)
        currentOffset = (currentRedWidth - currentGreenWidth) / 2f;
        greenZoneImage.rectTransform.localRotation = Quaternion.Euler(0f, 0f, -currentOffset);
    }

    void Update()
    {
        if (isTaskComplete) return;

        currentPointerAngle -= rotationSpeed * Time.deltaTime;

        if (currentPointerAngle <= -360f) currentPointerAngle += 360f;

        pointerHolder.localRotation = Quaternion.Euler(0f, 0f, currentPointerAngle);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isTaskComplete) return;

        float normalizedPointer = (currentPointerAngle % 360f + 360f) % 360f;
        float greenStartAngle = (currentTargetAngle - currentOffset + 360f) % 360f;
        float greenEndAngle = (greenStartAngle - currentGreenWidth + 360f) % 360f;

        bool isHit = false;

        // %100 Sadece Yeþil Alan Kontrolü
        if (greenStartAngle > greenEndAngle)
        {
            if (normalizedPointer <= greenStartAngle && normalizedPointer >= greenEndAngle)
                isHit = true;
        }
        else
        {
            if (normalizedPointer <= greenStartAngle || normalizedPointer >= greenEndAngle)
                isHit = true;
        }

        if (isHit)
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
            Debug.Log("<color=red>ISKALADIN! Kombo sýfýrlandý.</color>");
            SetupNewTarget();
        }
    }

    void TaskComplete()
    {
        isTaskComplete = true;
        Debug.Log("<color=green>HACK TAMAMLANDI!</color>");
        Invoke(nameof(DelayedClosePanel), 0.5f);
    }

    void DelayedClosePanel()
    {
        // 3. Ekraný kapatma iþini base class'a devrediyoruz.
        // Böylece hýrsýzýn donmasý çözülecek, fare kilitlenecek ve kapý/kasa tetiklenecek.
        WinMinigame();
    }
}