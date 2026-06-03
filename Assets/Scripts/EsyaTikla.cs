using UnityEngine;

public class EsyaTikla : MonoBehaviour
{
    private Transform oyuncuEnvanteri;

    void Start()
    {
        // Oyun baþladýðýnda sað alttaki envanter panelini otomatik bul
        oyuncuEnvanteri = GameObject.Find("OyuncuEnvanteri").transform;
    }

    // Butona týklandýðýnda çalýþacak fonksiyon
    public void CantaYaEkle()
    {
        if (oyuncuEnvanteri != null)
        {
            // Eþyayý sandýktan çýkarýp, sað alttaki envanter panelinin içine atar
            transform.SetParent(oyuncuEnvanteri);

            // Ekstra: Týkladýktan sonra boyutunun bozulmamasý için
            transform.localScale = Vector3.one;

            // ---- AKILLI GÖREV KONTROLÜ ----
            // Eðer çantaya eklenen bu eþyanýn etiketi (Tag) "Anahtar" ise görevi güncelle!
            if (gameObject.CompareTag("Anahtar"))
            {
                GorevSistemi.instance.AnahtarBulundu();
            }
        }
    }
}