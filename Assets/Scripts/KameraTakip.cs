using UnityEngine;

public class KameraTakip : MonoBehaviour
{
    [Header("Takip Ayarlarý")]
    public Transform hedef; // Hýrsýz'ý buraya sürükleyeceðiz
    public float takipHizi = 5f; // Kameranýn yumuþaklýk hýzý

    private Vector3 baslangicMesafesi;

    void Start()
    {
        // Oyun baþladýðýnda kameranýn hýrsýza olan o anki uzaklýðýný kaydet
        if (hedef != null)
        {
            baslangicMesafesi = transform.position - hedef.position;
        }
    }

    void LateUpdate() // Kamera takipleri titreme yapmamasý için LateUpdate'e yazýlýr
    {
        if (hedef != null)
        {
            // Gitmesi gereken yeni yeri bul
            Vector3 gitmesiGerekenYer = hedef.position + baslangicMesafesi;

            // Oraya anýnda ýþýnlanma, yumuþakça (Lerp) süzülerek git
            transform.position = Vector3.Lerp(transform.position, gitmesiGerekenYer, takipHizi * Time.deltaTime);
        }
    }
}