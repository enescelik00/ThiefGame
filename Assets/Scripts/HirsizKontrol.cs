using UnityEngine;

public class HirsizKontrol : MonoBehaviour
{
    [Header("Hareket Ayarlar�")]
    public float yurumeHizi = 5f;
    public float donusHizi = 15f;

    [Header("Aray�z (UI) Ba�lant�lar�")]
    public GameObject yakalanmaEkrani; // Canva resminin oldu�u obje buraya s�r�klenecek

    private Rigidbody rb;
    private Animator anim;
    private Vector3 hareketYonu;
    private bool yakalandiMi = false;
    public bool hackYapiyorMu = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        // Oyun ba��nda yakalanma ekran�n�n kapal� oldu�undan emin ol
        if (yakalanmaEkrani != null)
        {
            yakalanmaEkrani.SetActive(false);
        }
    }

    
        void Update()
        {
            if (yakalandiMi || hackYapiyorMu) return;

            float yatay = Input.GetAxisRaw("Horizontal"); // A ve D
            float dikey = Input.GetAxisRaw("Vertical");   // W ve S

            // 1. Kameranın baktığı İLERİ ve SAĞ yönlerini al
            Vector3 kameraIleri = Camera.main.transform.forward;
            Vector3 kameraSag = Camera.main.transform.right;

            // 2. Y eksenini sıfırla ki karakter yere bakarak havaya uçmaya çalışmasın
            kameraIleri.y = 0f;
            kameraSag.y = 0f;
            kameraIleri.Normalize();
            kameraSag.Normalize();

            // 3. W,A,S,D tuşlarını kameranın yönleriyle çarpıp birleştir
            hareketYonu = (kameraIleri * dikey + kameraSag * yatay).normalized;

            // Animasyon geçişleri
            if (hareketYonu.magnitude > 0.1f)
            {
                anim.SetBool("isWalking", true);
            }
            else
            {
                anim.SetBool("isWalking", false);
            }
        }
    

    void FixedUpdate()
    {
        if (yakalandiMi || hackYapiyorMu) // YENİ HALİ
        {
            // Kaymayı durdur ama Y eksenindeki yerçekimini (düşmeyi) koru
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            anim.SetBool("isWalking", false); // Yürüme animasyonunu zorla kes
            return;
        }

        // KRİTİK DEĞİŞİKLİK: Y eksenindeki yerçekimi hızını (rb.velocity.y) KORU!
        Vector3 yeniHiz = hareketYonu * yurumeHizi;
        yeniHiz.y = rb.linearVelocity.y;

        rb.linearVelocity = yeniHiz;

        // Karakterin yüzünü yürüdüğü yöne yumuşakça döndür
        if (hareketYonu != Vector3.zero)
        {
            Quaternion hedefRotasyon = Quaternion.LookRotation(hareketYonu);
            rb.rotation = Quaternion.Slerp(rb.rotation, hedefRotasyon, donusHizi * Time.fixedDeltaTime);
        }
    }

    // NPC'ler seni g�rd���nde bu fonksiyonu tetikleyecek
    public void YakalandiEkraniniGoster()
    {
        yakalandiMi = true;
        rb.linearVelocity = Vector3.zero; // Kaymay� durdur
        anim.SetBool("isWalking", false);
        // �leride ko�ma ekleyince �unu da a�ars�n: anim.SetBool("isRunning", false);

        // Canva g�rselini ekrana getir
        if (yakalanmaEkrani != null)
        {
            yakalanmaEkrani.SetActive(true);
        }
    }
}