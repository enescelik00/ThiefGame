using UnityEngine;
using TMPro;

public class GanimetHesaplayici : MonoBehaviour
{
    [Header("Ganimet Baðlantýlarý")]
    public Transform oyuncuEnvanteri;
    public TextMeshProUGUI ganimetListesiText;
    public TextMeshProUGUI kazancText; // Sadece bu bölümden kazanýlaný gösterecek

    [Header("Süre Baðlantýlarý")]
    public TextMeshProUGUI oyunIciSaatText;    // Ekranýn sað üstündeki sayan saat
    public TextMeshProUGUI bitirmeSuresiText;  // Kazanma ekranýnda yazacak süre

    void OnEnable()
    {
        HesaplaVeKaydet();
        SureyiEkranaYaz();
    }

    void HesaplaVeKaydet()
    {
        int kazanilanPara = 0;
        string liste = "<b>Toplanan Ganimetler:</b>\n\n";

        foreach (Transform esya in oyuncuEnvanteri)
        {
            EsyaOzellikleri ozellik = esya.GetComponent<EsyaOzellikleri>();
            if (ozellik != null)
            {
                kazanilanPara += ozellik.fiyat;
                liste += "- " + ozellik.esyaAdi + " ($" + ozellik.fiyat + ")\n";
            }
        }

        if (kazanilanPara == 0) liste += "<i>Hiç deðerli eþya toplanmadý...</i>\n";

        // Parayý kaydet ama ekrana GENEL BAKÝYEYÝ YAZMA
        int eskiBakiye = PlayerPrefs.GetInt("ToplamParam", 0);
        PlayerPrefs.SetInt("ToplamParam", eskiBakiye + kazanilanPara);
        PlayerPrefs.Save();

        // Ekrana sadece bu bölümün vurgununu yaz
        if (ganimetListesiText != null) ganimetListesiText.text = liste;
        if (kazancText != null) kazancText.text = "<b>Kazanýlan Vurgun:</b> $" + kazanilanPara;
    }

    void SureyiEkranaYaz()
    {
        // Sað üstteki saatin o anki yazýsýný alýp, Kazanma ekranýna aktarýyoruz
        if (oyunIciSaatText != null && bitirmeSuresiText != null)
        {
            bitirmeSuresiText.text = "<b>Kaçýþ Süresi:</b> " + oyunIciSaatText.text;
        }
    }
}