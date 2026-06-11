using UnityEngine;
using UnityEngine.SceneManagement; // Sahneler için þart

public class KazanmaEkraniKontrol : MonoBehaviour
{
    public void SiradakiLeveleGec()
    {
        Time.timeScale = 1f; // Zamaný sýfýrla/normale döndür

        // Aktif sahnenin (Level'ýn) numarasýný/indeksini al ve 1 ekleyerek sýradakini yükle
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