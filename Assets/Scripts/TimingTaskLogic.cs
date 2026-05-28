using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TimingTaskLogic : MonoBehaviour, IPointerClickHandler
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
    public float redZoneMultiplier = 2f; // YENÝ: Sabit taþma yerine orantýlý kat sayý

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
        gameObject.SetActive(false);
    }
}