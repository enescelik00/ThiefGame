using UnityEngine;
using TMPro; // TextMeshPro yazýlarýyla iþlem yapmak için zorunlu

public class SureSistemi : MonoBehaviour
{
    [Header("Zaman Yazýlarý")]
    public TextMeshProUGUI canliSureText;  // Sað üstteki
    public TextMeshProUGUI bitisSuresiText; // Yakalanma ekranýndaki

    private float gecenSure = 0f;

    void Update()
    {
        // Time.deltaTime, oyun donduðunda (Time.timeScale = 0) otomatik durur.
        // Bu yüzden "yakalandý mý?" diye kod yazmýyoruz, kendisi donuyor!
        gecenSure += Time.deltaTime;

        // Süreyi dakika ve saniyeye çevirme matematiði
        int dakika = Mathf.FloorToInt(gecenSure / 60);
        int saniye = Mathf.FloorToInt(gecenSure % 60);

        // Zamaný 00:00 formatýnda metne dönüþtür
        string zamanMetni = string.Format("{0:00}:{1:00}", dakika, saniye);

        // 1. Sað üstteki sayacý sürekli güncelle
        if (canliSureText != null)
        {
            canliSureText.text = zamanMetni;
        }

        // 2. Kapalý olan Yakalanma ekranýndaki yazýyý da gizlice arka planda güncelle
        // Böylece ekran açýldýðý an, o anki donmuþ süreyi gösterecek!
        if (bitisSuresiText != null)
        {
            bitisSuresiText.text = "GECEN SURE: " + zamanMetni;
        }
    }
}