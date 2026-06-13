using UnityEngine;
using UnityEngine.SceneManagement;

public class KazanmaEkraniKontrol : MonoBehaviour
{
    // Kazanma paneli ekranda belirdiði an otomatik olarak bu fonksiyon çalýþýr
    private void OnEnable()
    {
        string aktifSahneIsmi = SceneManager.GetActiveScene().name;
        int mevcutIlerleme = PlayerPrefs.GetInt("LevelProgress", 1);

        // Sorun buradaydý! "level1" yerine "LEVEL1" yaptýk.
        if (aktifSahneIsmi == "LEVEL1" && mevcutIlerleme < 2)
        {
            PlayerPrefs.SetInt("LevelProgress", 2);
            PlayerPrefs.Save();
            Debug.Log("KAYIT BAÞARILI: Orta seviye açýldý!");
        }
        // Ayný þekilde burayý da "LEVEL2" yapýyoruz.
        else if (aktifSahneIsmi == "LEVEL2" && mevcutIlerleme < 3)
        {
            PlayerPrefs.SetInt("LevelProgress", 3);
            PlayerPrefs.Save();
            Debug.Log("KAYIT BAÞARILI: Zor seviye açýldý!");
        }
    }

    public void SiradakiLeveleGec()
    {
        Time.timeScale = 1f; // Zamaný sýfýrla/normale döndür

        int aktifSahneIndeksi = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(aktifSahneIndeksi + 1);
    }

    public void AnaMenuyeDon()
    {
        Time.timeScale = 1f;

        // Level müziðini bul ve yok et (Menü müziðiyle karýþmasýn)
        GameObject levelMuzigi = GameObject.Find("MuzikYoneticisi");
        if (levelMuzigi != null)
        {
            Destroy(levelMuzigi);
        }

        // Ana menü sahnenizin tam ismini buraya yazýn
        SceneManager.LoadScene("MENU");
    }
}