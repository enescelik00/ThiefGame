using UnityEngine;
using UnityEngine.UI; // Butonlarýn Image bileþenine eriþmek için ekledik
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class LevelManagerScript : MonoBehaviour
{
    [Header("Menü Panelleri")]
    public GameObject difficultyPanel; // Arkasý karartmalý yeni zorluk paneli

    [Header("Bölüm Zeminleri (Image)")]
    public Image imgOrta;
    public Image imgZor;

    [Header("Açýk/Kapalý Zemin Sprite'larý")]
    public Sprite spriteOrtaLocked;
    public Sprite spriteOrtaUnlocked;
    public Sprite spriteZorLocked;
    public Sprite spriteZorUnlocked;

    [Header("Kilit Ýkonlarý (Objeler)")]
    public GameObject lockIconOrta;
    public GameObject lockIconZor;

    [Header("Bölüm Metinleri (TMP)")]
    public TextMeshProUGUI txtOrta;
    public TextMeshProUGUI txtZor;

    [Header("Uyarý Paneli Ayarlarý (Mevcut)")]
    public GameObject warningPanel;
    public TextMeshProUGUI warningText;

    private void Start()
    {
        // Oyun baþladýðýnda zorluk panelini kapalý tut
        if (difficultyPanel != null)
            difficultyPanel.SetActive(false);
    }

    // --- 1. EVE TIKLANDIÐINDA ÇAÐRILACAK FONKSÝYON ---
    public void OpenDifficultyPanel()
    {
        difficultyPanel.SetActive(true);
        UpdateLocks(); // Kilit durumlarýný kontrol et ve UI'ý güncelle
        StartCoroutine(PopAnimation(difficultyPanel.transform)); // Tatlý bir açýlýþ efekti
    }

    // --- 2. SÝYAH BOÞLUÐA TIKLAYINCA ÇAÐRILACAK FONKSÝYON ---
    public void CloseDifficultyPanel()
    {
        difficultyPanel.SetActive(false);
    }

    // --- 3. KÝLÝT VE GÖRSEL GÜNCELLEME MANTIÐI ---
    private void UpdateLocks()
    {
        // Oyuncunun ilerlemesini çekiyoruz. Kayýt yoksa varsayýlan olarak 1 (Sadece Kolay) döner.
        // Ýleride bölüm bitirme scriptine þunu yazacaksýn: PlayerPrefs.SetInt("LevelProgress", 2);
        int progress = PlayerPrefs.GetInt("LevelProgress", 1);

        // Kolay her zaman açýk, ona dokunmuyoruz.

        // ORTA BÖLÜM KONTROLÜ
        if (progress >= 2)
        {
            imgOrta.sprite = spriteOrtaUnlocked; // Parlak sarý zemin
            lockIconOrta.SetActive(false); // Kilidi gizle
            txtOrta.color = Color.white; // Yazýyý bembeyaz yap
        }
        else
        {
            imgOrta.sprite = spriteOrtaLocked; // Soluk sarý zemin
            lockIconOrta.SetActive(true); // Kilidi göster
            txtOrta.color = new Color(0.8f, 0.8f, 0.8f, 1f); // Yazýyý hafif gri/silik yap
        }

        // ZOR BÖLÜM KONTROLÜ
        if (progress >= 3)
        {
            imgZor.sprite = spriteZorUnlocked; // Parlak kýrmýzý zemin
            lockIconZor.SetActive(false); // Kilidi gizle
            txtZor.color = Color.white;
        }
        else
        {
            imgZor.sprite = spriteZorLocked; // Soluk kýrmýzý zemin
            lockIconZor.SetActive(true); // Kilidi göster
            txtZor.color = new Color(0.8f, 0.8f, 0.8f, 1f);
        }
    }

    // --- 4. BUTONLARA TIKLANDIÐINDA ÇALIÞACAK FONKSÝYONLAR ---

    public void OnKolayClicked()
    {
        LoadGameScene("level1");
    }

    public void OnOrtaClicked()
    {
        if (PlayerPrefs.GetInt("LevelProgress", 1) >= 2)
            LoadGameScene("level2");
        else
            ShowLockedWarning(); // Eðer bölüm kilitliyse senin yazdýðýn uyarýyý ver!
    }

    public void OnZorClicked()
    {
        if (PlayerPrefs.GetInt("LevelProgress", 1) >= 3)
            LoadGameScene("level3");
        else
            ShowLockedWarning();
    }


    // --- ANA HARÝTA BÝNALARI (BANKA VE MÜZE) ---

    public void OnBankaClicked()
    {
        // Ýleride buraya da PlayerPrefs ile "Banka açýldý mý?" kontrolü ekleyeceðiz.
        // Þimdilik tamamen kilitli olduðu için direkt uyarý verdiriyoruz.
        ShowLockedWarning();
    }

    public void OnMuzeClicked()
    {
        // Þimdilik tamamen kilitli
        ShowLockedWarning();
    }

    // --- YARDIMCI FONKSÝYONLAR (Mevcut Mantýk) ---

    private void LoadGameScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void ShowLockedWarning()
    {
        StopAllCoroutines();
        StartCoroutine(WarningRoutine());
    }

    private IEnumerator WarningRoutine()
    {
        warningText.text = "BU BÖLÜM ÞU AN KÝLÝTLÝ!";
        warningPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        warningPanel.SetActive(false);
    }

    // --- EKSTRA: ZORLUK PANELÝ AÇILIÞ ANÝMASYONU ---
    private IEnumerator PopAnimation(Transform panelTransform)
    {
        panelTransform.localScale = Vector3.zero; // Panel önce sýfýr boyutunda baþlar
        float time = 0;
        float duration = 0.2f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float scale = Mathf.Lerp(0, 1, time / duration);
            panelTransform.localScale = new Vector3(scale, scale, scale); // Büyüt
            yield return null;
        }
        panelTransform.localScale = Vector3.one; // Ýþlem bitince tam boyuta sabitle
    }
}