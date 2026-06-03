using UnityEngine;
using TMPro;

public class GorevSistemi : MonoBehaviour
{
    public static GorevSistemi instance;

    [Header("Arayüz")]
    public TextMeshProUGUI gorevYazisi;

    private bool kameraKapatildi = false;
    private bool anahtarBulundu = false;
    private bool kasaAcildi = false;

    void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        GorevleriEkranaYaz();
    }

    public void KameraKapatildi()
    {
        kameraKapatildi = true;
        GorevleriEkranaYaz();
    }

    public void AnahtarBulundu()
    {
        anahtarBulundu = true;
        GorevleriEkranaYaz();
    }

    public void KasaAcildi()
    {
        kasaAcildi = true;
        GorevleriEkranaYaz();
    }

    void GorevleriEkranaYaz()
    {
        if (gorevYazisi == null) return;

        // İstersen "Görevler :" başlığını da kalın (Bold) yapmak için TextMeshPro'nun <b> etiketini kullanabiliriz
        string metin = "<b>Görevler :</b>\n";

        // Tire (-) yerine İçi Boş Kare (□) veya İçi Dolu Kare (■) kullanıyoruz
        if (!kameraKapatildi)
        {
            metin += "□ Kamera Sistemini Devre Dışı Bırak\n";
        }

        if (!anahtarBulundu)
        {
            metin += "□ Anahtarı Bul\n";
        }
        else if (!kasaAcildi)
        {
            metin += "□ Kasaya Ulaş ve Aç\n";
        }
        else
        {
            metin += "□ Çıkışa Ulaş ve Kaç!\n";
        }

        gorevYazisi.text = metin;
    }
}