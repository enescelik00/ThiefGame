using UnityEngine;

public class KameraYakalanma : MonoBehaviour
{
    [Header("Bitiþ Ekraný")]
    public GameObject yakalanmaEkrani; // Kýrmýzý YAKALANDIN ekraný

    void OnTriggerEnter(Collider other)
    {
        // Eðer ýþýða (sensöre) deðen kiþi hýrsýzsa
        if (other.CompareTag("Player"))
        {
            if (yakalanmaEkrani != null)
            {
                // Yakalanma ekranýný aç
                yakalanmaEkrani.SetActive(true);

                // Oyunu dondur
                Time.timeScale = 0f;
            }
        }
    }
}