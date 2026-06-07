using UnityEngine;

public class KacisKontrol : MonoBehaviour
{
    [Header("Bitiþ Ayarlarý")]
    public GameObject kazanmaEkrani; // Senin hazýrladýðýn KAZANMA canvas objesi

    void OnTriggerEnter(Collider other)
    {
        // Eðer ýþýða giren kiþi hýrsýzsa
        if (other.CompareTag("Player"))
        {
            if (kazanmaEkrani != null)
            {
                // Kazanma ekranýný aktif et (O ekran açýlýnca ganimetleri hesaplayan kod otomatik çalýþacak)
                kazanmaEkrani.SetActive(true);

                // Oyunu arka planda dondur (Zamaný durdur)
                Time.timeScale = 0f;
            }
        }
    }
}