using UnityEngine;
using UnityEngine.SceneManagement;

public class YakalanmaEkraniKontrol : MonoBehaviour
{
    public GameObject yakalanmaCanvasObjesi;

    public void BolumuYenidenBaslat()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AnaMenuyeDon()
    {
        Time.timeScale = 1f;

        // YENÝ EKLENEN KISIM: Level müziðini isminden bul ve yok et!
        // Not: Eðer prefab'in adý "MuzikYoneticisi" deðilse, buradaki ismi kendi verdiðin isimle deðiþtir.
        GameObject levelMuzigi = GameObject.Find("MuzikYoneticisi");
        if (levelMuzigi != null)
        {
            Destroy(levelMuzigi);
        }

        // Ana menü sahnesini yükle
        SceneManager.LoadScene("MENU");
    }
}