using UnityEngine;

public class SesKontrol : MonoBehaviour
{
    [Header("Arayüz")]
    public GameObject carpiIsareti; // Unity'den o kýrmýzý X yazýsýný buraya atayacaðýz

    private bool sesAcik = true;

    void Start()
    {
        // Oyun ilk baþladýðýnda ses açýksa çarpýyý otomatik gizle
        if (carpiIsareti != null)
        {
            carpiIsareti.SetActive(false);
        }
    }

    public void SesiAcKapat()
    {
        sesAcik = !sesAcik; // Durumu tersine çevir

        if (sesAcik)
        {
            // Sesi AÇ
            AudioListener.volume = 1f;
            if (carpiIsareti != null) carpiIsareti.SetActive(false); // Çarpýyý gizle
        }
        else
        {
            // Sesi KAPAT
            AudioListener.volume = 0f;
            if (carpiIsareti != null) carpiIsareti.SetActive(true); // Çarpýyý göster (Kýrmýzý X çýksýn)
        }
    }
}