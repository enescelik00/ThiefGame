using UnityEngine;
using UnityEngine.SceneManagement; // Sahne geçiþleri için þart
using TMPro; // Yazýlar için þart
using System.Collections;

public class LevelManagerScript : MonoBehaviour
{
    [Header("Uyarý Paneli Ayarlarý")]
    public GameObject warningPanel;
    public TextMeshProUGUI warningText;

    // AÇIK BÖLÜM: Sahne Yükleme Fonksiyonu
    public void LoadGameScene(string sceneName)
    {
        // sceneName kýsmýna Unity'deki oyun sahnenin adýný yazacaðýz
        SceneManager.LoadScene(sceneName);
    }

    // KÝLÝTLÝ BÖLÜM: Uyarý Gösterme Fonksiyonu
    public void ShowLockedWarning()
    {
        StopAllCoroutines(); // Eðer ard arda basarsa animasyon bozulmasýn diye sýfýrlarýz
        StartCoroutine(WarningRoutine());
    }

    // Uyarýyý 2 saniye gösterip kapatan zamanlayýcý (Coroutine)
    private IEnumerator WarningRoutine()
    {
        warningText.text = "BU BÖLÜM ÞU AN KÝLÝTLÝ!";
        warningPanel.SetActive(true); // Paneli görünür yap

        yield return new WaitForSeconds(2f); // 2 saniye bekle

        warningPanel.SetActive(false); // Paneli gizle
    }
}