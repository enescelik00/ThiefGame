using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [Header("Veritabaný ve UI Baðlantýlarý")]
    public SkinItem[] skinDatabase;
    public GameObject skinCardPrefab;
    public Transform shopContent;
    public TextMeshProUGUI toplamParaText;

    [Header("Stüdyo Baðlantýsý")]
    public Transform vitrinPlatformu; // Karakterlerin doðacaðý platform (Magaza_Studio)
    private GameObject mevcutManken;    // O an ekranda dönen karakteri hafýzada tutmak için

    // YENÝ: Sahnede o an var olan kartlarý takip etmek için liste
    private System.Collections.Generic.List<SkinCard> olusturulanKartlar = new System.Collections.Generic.List<SkinCard>();

    private void Start()
    {
        // Varsayýlan skin (ID: 0) her zaman satýn alýnmýþ (açýk) olmalý
        PlayerPrefs.SetInt("SkinUnlocked_0", 1);

        // Eðer daha önce hiç skin kuþanýlmadýysa, varsayýlan skini kuþan
        if (!PlayerPrefs.HasKey("EquippedSkinID"))
            PlayerPrefs.SetInt("EquippedSkinID", 0);

        ParaYazisiniGuncelle();
        MagazayiOlustur();

        // Baþlangýçta kuþanýlmýþ olan skini vitrinde otomatik göster
        BaslangicSkininiGoster();
    }

    private void BaslangicSkininiGoster()
    {
        int kusanilanSkinID = PlayerPrefs.GetInt("EquippedSkinID", 0);
        foreach (SkinItem skin in skinDatabase)
        {
            if (skin.skinID == kusanilanSkinID)
            {
                SkiniOnizle(skin);
                break;
            }
        }
    }

    private void ParaYazisiniGuncelle()
    {
        if (toplamParaText != null)
        {
            int guncelPara = PlayerPrefs.GetInt("ToplamParam", 0);
            toplamParaText.text = "$ " + guncelPara.ToString("N0");
        }
    }

    private void MagazayiOlustur()
    {
        for (int i = 0; i < skinDatabase.Length; i++)
        {
            SkinItem currentSkin = skinDatabase[i];

            GameObject yeniKartObjesi = Instantiate(skinCardPrefab, shopContent);
            SkinCard kartScript = yeniKartObjesi.GetComponent<SkinCard>();

            kartScript.skinIndex = currentSkin.skinID;
            kartScript.skinNameText.text = currentSkin.skinName;

            // --- YENÝ EKLENEN: GÖRSELLERÝ DATADAN ÇEKÝP KARTA GÝYDÝRME ---
            if (currentSkin.skinIcon != null && kartScript.skinIcon != null)
                kartScript.skinIcon.sprite = currentSkin.skinIcon;

            if (currentSkin.arkaPlanResmi != null && kartScript.arkaPlanImage != null)
                kartScript.arkaPlanImage.sprite = currentSkin.arkaPlanResmi;

            if (currentSkin.butonResmi != null && kartScript.butonImage != null)
                kartScript.butonImage.sprite = currentSkin.butonResmi;

            // Glow Rengini Ayarla ve Baþlangýçta Kapalý Tut
            if (kartScript.glowImage != null)
            {
                kartScript.glowImage.color = currentSkin.glowRengi;
                kartScript.glowImage.gameObject.SetActive(false);
            }
            // -------------------------------------------------------------

            ButonDurumunuGuncelle(kartScript, currentSkin);

            // 1. Satýn Al/Kuþan Butonuna Týklama
            kartScript.actionButton.onClick.AddListener(() => ButonaTiklandi(kartScript, currentSkin));

            // 2. Kartýn Kendisine Týklama (Önizleme)
            Button kartAnaButon = yeniKartObjesi.GetComponent<Button>();
            if (kartAnaButon != null)
            {
                kartAnaButon.onClick.AddListener(() => SkiniOnizle(currentSkin));
            }

            // Oluþturulan kartý listemize ekleyelim ki glow'unu kontrol edebilelim
            olusturulanKartlar.Add(kartScript);
        }
    }

    private void SkiniOnizle(SkinItem skin)
    {
        if (skin == null)
        {
            Debug.LogError("Hata: Týklanan SkinItem dosyasý boþ (null)!");
            return;
        }

        Debug.Log("Vitrin Güncellenmeye Çalýþýlýyor: " + skin.skinName);

        if (vitrinPlatformu == null)
        {
            Debug.LogError("Hata: ShopManager içindeki 'Vitrin Platformu' alaný boþ! ");
            return;
        }

        if (skin.skinPrefab == null)
        {
            Debug.LogError($"Hata: [{skin.skinName}] isimli ScriptableObject dosyasýnýn içindeki 'Skin Prefab' alaný BOÞ!");
            return;
        }

        // 1. Eski mankeni sil
        if (mevcutManken != null)
        {
            Destroy(mevcutManken);
        }

        // 2. Yeni skini doðurt
        mevcutManken = Instantiate(skin.skinPrefab, vitrinPlatformu);

        // 3. Pozisyon sýfýrlama
        mevcutManken.transform.localPosition = Vector3.zero;
        mevcutManken.transform.localRotation = Quaternion.Euler(0, 180, 0);

        // KRÝTÝK: Model boyutu ayarý
        mevcutManken.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        Debug.Log($"Baþarýlý: [{skin.skinName}] stüdyoda baþarýyla oluþturuldu!");

        foreach (SkinCard kart in olusturulanKartlar)
        {
            if (kart.skinIndex == skin.skinID)
            {
                // Seçilen kartý %5 büyüt ve öne çýkar
                kart.transform.localScale = new Vector3(1.05f, 1.05f, 1.05f);

                // Ýsteðe baðlý: Eðer kartýn arkasýndaki rengi de koddan hafifçe deðiþtirmek istersen:
                // kart.arkaPlanImage.color = skin.glowRengi; 
            }
            else
            {
                // Seçilmeyen diðer tüm kartlarý normal boyutuna (1) geri döndür
                kart.transform.localScale = Vector3.one;

                // kart.arkaPlanImage.color = Color.white; // Normal rengine çek
            }
        }

        // --- YENÝ EKLENEN: GLOW AÇ/KAPAT MANTIÐI ---
        // Sadece önizlenen kartýn ýþýðýný aç, diðerlerininkini kapat
        foreach (SkinCard kart in olusturulanKartlar)
        {
            if (kart.glowImage != null)
            {
                // Kartýn ID'si seçili ID ile aynýysa TRUE (yanar), deðilse FALSE (söner)
                kart.glowImage.gameObject.SetActive(kart.skinIndex == skin.skinID);
            }
        }
        // -------------------------------------------
    }

    private void ButonDurumunuGuncelle(SkinCard kart, SkinItem skin)
    {
        int kusanilanSkin = PlayerPrefs.GetInt("EquippedSkinID");
        bool satinAlindiMi = PlayerPrefs.GetInt("SkinUnlocked_" + skin.skinID, 0) == 1;

        if (kusanilanSkin == skin.skinID)
        {
            kart.buttonText.text = "KUÞANILDI";
            kart.actionButton.interactable = false;
            kart.priceText.text = "SATIN ALINDI"; // Deðiþtirildi: Boþ býrakmak yerine Satýn Alýndý yazýyor
        }
        else if (satinAlindiMi)
        {
            kart.buttonText.text = "KUÞAN";
            kart.actionButton.interactable = true;
            kart.priceText.text = "SATIN ALINDI"; // Deðiþtirildi: Boþ býrakmak yerine Satýn Alýndý yazýyor
        }
        else
        {
            kart.buttonText.text = "SATIN AL";
            kart.actionButton.interactable = true;

            // Fiyat 0 ise "ÜCRETSÝZ", deðilse normal fiyatý basar
            kart.priceText.text = skin.price == 0 ? "ÜCRETSÝZ" : "$ " + skin.price.ToString("N0");
        }
    }

    private void ButonaTiklandi(SkinCard kart, SkinItem skin)
    {
        bool satinAlindiMi = PlayerPrefs.GetInt("SkinUnlocked_" + skin.skinID, 0) == 1;

        if (satinAlindiMi)
        {
            PlayerPrefs.SetInt("EquippedSkinID", skin.skinID);
            Debug.Log(skin.skinName + " kuþanýltý!");
            SkiniOnizle(skin); // Kuþanýldýðýnda vitrindeki görüntüyü de güncelle
        }
        else
        {
            int paramiz = PlayerPrefs.GetInt("ToplamParam", 0);
            if (paramiz >= skin.price)
            {
                PlayerPrefs.SetInt("ToplamParam", paramiz - skin.price);
                PlayerPrefs.SetInt("SkinUnlocked_" + skin.skinID, 1);
                PlayerPrefs.SetInt("EquippedSkinID", skin.skinID);
                Debug.Log(skin.skinName + " satýn alýndý!");

                ParaYazisiniGuncelle();
                SkiniOnizle(skin); // Satýn alýndýðýnda vitrindeki görüntüyü de güncelle
            }
            else
            {
                Debug.Log("Para yetersiz!");
                return;
            }
        }

        EskiKartlariTemizle();
        MagazayiOlustur();
    }

    private void EskiKartlariTemizle()
    {
        foreach (Transform child in shopContent)
        {
            Destroy(child.gameObject);
        }
        olusturulanKartlar.Clear(); // YENÝ: Listeyi sýfýrla ki eski silinmiþ kartlar hafýzada kalmasýn
    }
}