using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [Header("Veritabaný ve UI Baðlantýlarý")]
    public SkinItem[] skinDatabase;
    public GameObject skinCardPrefab;
    public Transform shopContent;
    public TextMeshProUGUI toplamParaText; // Sað üstteki para yazýmýz (YENÝ EKLENDÝ)

    [Header("Stüdyo Baðlantýsý")]
    public GameObject studyoManken; // Mankenin rengini/materyalini deðiþtirmek için

    private void Start()
    {
        // Varsayýlan skin (ID: 0) her zaman satýn alýnmýþ (açýk) olmalý
        PlayerPrefs.SetInt("SkinUnlocked_0", 1);

        // Eðer daha önce hiç skin kuþanýlmadýysa, varsayýlan skini kuþan
        if (!PlayerPrefs.HasKey("EquippedSkinID"))
            PlayerPrefs.SetInt("EquippedSkinID", 0);

        ParaYazisiniGuncelle(); // Baþlangýçta paramýzý ekrana yaz
        MagazayiOlustur();
    }

    private void ParaYazisiniGuncelle()
    {
        if (toplamParaText != null)
        {
            // Ekip arkadaþýnýn kaydettiði "ToplamParam" anahtarýný okuyoruz
            int guncelPara = PlayerPrefs.GetInt("ToplamParam", 0);
            toplamParaText.text = "$ " + guncelPara.ToString("N0"); // 15000 yerine 15.000 yazar
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

            if (currentSkin.skinIcon != null)
                kartScript.skinIcon.sprite = currentSkin.skinIcon;

            ButonDurumunuGuncelle(kartScript, currentSkin);

            // 1. Satýn Al/Kuþan Butonuna Týklama (Zaten vardý)
            kartScript.actionButton.onClick.AddListener(() => ButonaTiklandi(kartScript, currentSkin));

            // 2. KARTIN KENDÝSÝNE Týklama (ÖNÝZLEME ÝÇÝN - YENÝ EKLENDÝ)
            Button kartAnaButon = yeniKartObjesi.GetComponent<Button>();
            if (kartAnaButon != null)
            {
                kartAnaButon.onClick.AddListener(() => SkiniOnizle(currentSkin));
            }
        }
    }

    // YENÝ EKLENEN ÖNÝZLEME FONKSÝYONU
    private void SkiniOnizle(SkinItem skin)
    {
        Debug.Log("Vitrin Güncellendi: " + skin.skinName);

        if (studyoManken != null)
        {
            // Önce skin ID'sine göre hangi rengi vereceðimizi seçelim
            Color yeniRenk = Color.white;
            if (skin.skinID == 0) yeniRenk = Color.white;
            else if (skin.skinID == 1) yeniRenk = Color.magenta;
            else if (skin.skinID == 2) yeniRenk = Color.black;
            else yeniRenk = Color.yellow;

            // BÜYÜ BURADA: Mankenin içindeki TÜM parçalarý (Mesh veya SkinnedMesh fark etmez) bul
            Renderer[] tumParcalar = studyoManken.GetComponentsInChildren<Renderer>();

            // Hepsini tek tek boya
            foreach (Renderer parca in tumParcalar)
            {
                parca.material.color = yeniRenk;
            }
        }
    }

    private void ButonDurumunuGuncelle(SkinCard kart, SkinItem skin)
    {
        int kusanilanSkin = PlayerPrefs.GetInt("EquippedSkinID");
        bool satinAlindiMi = PlayerPrefs.GetInt("SkinUnlocked_" + skin.skinID, 0) == 1;

        if (kusanilanSkin == skin.skinID)
        {
            kart.buttonText.text = "KUÞANILDI";
            kart.actionButton.interactable = false;
            kart.priceText.text = "";
        }
        else if (satinAlindiMi)
        {
            kart.buttonText.text = "KUÞAN";
            kart.actionButton.interactable = true;
            kart.priceText.text = "";
        }
        else
        {
            kart.buttonText.text = "SATIN AL";
            kart.actionButton.interactable = true;
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
        }
        else
        {
            // Ekip arkadaþýnýn sistemiyle entegre bakiye kontrolü
            int paramiz = PlayerPrefs.GetInt("ToplamParam", 0);
            if (paramiz >= skin.price)
            {
                // Parayý düþ ve yeni bakiyeyi kaydet
                PlayerPrefs.SetInt("ToplamParam", paramiz - skin.price);
                PlayerPrefs.SetInt("SkinUnlocked_" + skin.skinID, 1);
                PlayerPrefs.SetInt("EquippedSkinID", skin.skinID);
                Debug.Log(skin.skinName + " satýn alýndý!");

                // Sað üstteki paramýzý anýnda güncelle
                ParaYazisiniGuncelle();
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
    }
}