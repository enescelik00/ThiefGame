using UnityEngine;

public class SandikSistemi : MonoBehaviour
{
    [Header("Arayüz Ayarlarý (Her sandýkta ayný kalacak)")]
    public GameObject sandikEkrani; // Canvas'taki SandikEkrani paneli
    public Transform esyaYuvasi;    // Eþyanýn çýkacaðý tam orta nokta

    [Header("Bu Sandýða Özel Ýçerik")]
    public GameObject icindekiEsyaPrefab; // Bu sandýkta ne var? (Anahtar, Altýn vb.)

    private bool iciDoluMu = true; // Sandýk daha önce soyuldu mu?
    private GameObject ekrandakiEsya; // Ekranda beliren eþyayý takip etmek için

    void Start()
    {
        if (sandikEkrani != null)
        {
            sandikEkrani.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            sandikEkrani.SetActive(true); // Ekraný aç
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Eðer sandýk doluysa ve içine bir eþya koyduysak (boþ deðilse)
            if (iciDoluMu && icindekiEsyaPrefab != null)
            {
                // YENÝ EKLENEN KISIM: Ses tam burada, eþya ortaya çýkarken (sandýk açýlýrken) çalacak!
                if (SesYoneticisi.instance != null)
                {
                    SesYoneticisi.instance.SandikSesiniCal();
                }

                // Eþyanýn kopyasýný oluþtur ve ekranýn ortasýndaki yuvaya yerleþtir
                ekrandakiEsya = Instantiate(icindekiEsyaPrefab, esyaYuvasi);

                // Eþyanýn pozisyonunu zorla sýfýrla (Tam ortaya oturt)
                ekrandakiEsya.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                iciDoluMu = false; // Sandýðýn içi artýk boþaldý
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            sandikEkrani.SetActive(false); // Ekraný kapat

            // Eðer oyuncu eþyaya TIKLAMADAN (almadan) uzaklaþtýysa:
            // Eþya hala ekrandaysa onu sil ve sandýðý tekrar "dolu" hale getir.
            if (ekrandakiEsya != null && ekrandakiEsya.transform.parent == esyaYuvasi)
            {
                Destroy(ekrandakiEsya);
                iciDoluMu = true;
            }
        }
    }
}