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
        }
    }
}