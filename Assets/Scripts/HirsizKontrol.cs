using UnityEngine;

public class HirsizKontrol : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float yurumeHizi = 5f;
    public float donusHizi = 15f;

    [Header("Arayüz (UI) Bağlantıları")]
    public GameObject yakalanmaEkrani;
    public VirtualJoystick virtualJoystick; // Joystick referansımız

    private Rigidbody rb;
    private Animator anim;
    private Vector3 hareketYonu;
    private bool yakalandiMi = false;
    public bool hackYapiyorMu = false;

    // Performans: Kamerayı her frame aratmak yerine Start'ta hafızaya alıyoruz
    private Camera anaKamera;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anaKamera = Camera.main;

        // NOT: anim = GetComponent<Animator>(); satırı SİLİNDİ. 
        // Çünkü Animator artık ana objede değil, sonradan doğacak olan skin prefabının içinde.

        // Sahnede aktif olan VirtualJoystick scriptini otomatik bulur ve atanır!
        if (virtualJoystick == null)
        {
            virtualJoystick = FindFirstObjectByType<VirtualJoystick>();
        }

        if (yakalanmaEkrani != null)
            yakalanmaEkrani.SetActive(false);
    }

    void Update()
    {
        // --- 1. YENİ EKLENEN ANİMATÖR GÜVENLİK KONTROLÜ ---
        // Eğer animator henüz bulunamadıysa (skin prefabı yükleniyorsa) alt objelerde aramaya devam et.
        if (anim == null)
        {
            anim = GetComponentInChildren<Animator>();

            // Eğer hala bulamadıysa (doğma aşamasındaysa) aşağıdaki hareket kodlarına geçme, hata vermemesi için bekle.
            if (anim == null) return;
        }

        // --- 2. KURŞUN GEÇİRMEZ KONTROL: Eğer joystick referansı kopmuşsa hemen yenisini bul!
        if (virtualJoystick == null)
        {
            virtualJoystick = FindFirstObjectByType<VirtualJoystick>();
        }

        // --- 3. Karakter yakalandıysa veya minigame oynuyorsa hareket kodlarını atla
        if (yakalandiMi || hackYapiyorMu) return;

        // --- Yürüme Mantığı ---
        float yatay = Input.GetAxisRaw("Horizontal");
        float dikey = Input.GetAxisRaw("Vertical");

        if (virtualJoystick != null && virtualJoystick.InputVector.magnitude > 0.05f)
        {
            yatay = virtualJoystick.InputVector.x;
            dikey = virtualJoystick.InputVector.y;
        }

        Vector3 kameraIleri = anaKamera.transform.forward;
        Vector3 kameraSag = anaKamera.transform.right;

        kameraIleri.y = 0f;
        kameraSag.y = 0f;
        kameraIleri.Normalize();
        kameraSag.Normalize();

        hareketYonu = (kameraIleri * dikey + kameraSag * yatay).normalized;

        bool yuruyorMu = Mathf.Abs(yatay) > 0.05f || Mathf.Abs(dikey) > 0.05f;

        // Artık anim değişkeninin dolu olduğundan %100 eminiz, hata vermeden çalışacaktır.
        anim.SetBool("isWalking", yuruyorMu);
    }

    void FixedUpdate()
    {
        if (yakalandiMi || hackYapiyorMu)
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);

            // Eğer animatör bulunduysa yürüme animasyonunu durdur
            if (anim != null) anim.SetBool("isWalking", false);
            return;
        }

        // Hızı uygula (Yerçekimini bozmadan)
        Vector3 yeniHiz = hareketYonu * yurumeHizi;
        yeniHiz.y = rb.linearVelocity.y;
        rb.linearVelocity = yeniHiz;

        // Karakteri hareket yönüne yumuşakça döndür
        if (hareketYonu != Vector3.zero)
        {
            Quaternion hedefRotasyon = Quaternion.LookRotation(hareketYonu);
            rb.rotation = Quaternion.Slerp(rb.rotation, hedefRotasyon, donusHizi * Time.fixedDeltaTime);
        }
    }

    public void YakalandiEkraniniGoster()
    {
        yakalandiMi = true;
        rb.linearVelocity = Vector3.zero;

        // Eğer animatör bulunduysa yürüme animasyonunu durdur
        if (anim != null) anim.SetBool("isWalking", false);

        if (yakalanmaEkrani != null)
            yakalanmaEkrani.SetActive(true);
    }
}