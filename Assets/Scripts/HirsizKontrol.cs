using UnityEngine;

public class HirsizKontrol : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float yurumeHizi = 5f;
    public float donusHizi = 15f;

    [Header("Arayüz (UI) Bağlantıları")]
    public GameObject yakalanmaEkrani;
    public VirtualJoystick virtualJoystick; // ← YENİ

    private Rigidbody rb;
    private Animator anim;
    private Vector3 hareketYonu;
    private bool yakalandiMi = false;
    public bool hackYapiyorMu = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        if (yakalanmaEkrani != null)
            yakalanmaEkrani.SetActive(false);
    }

    void Update()
    {
        if (yakalandiMi || hackYapiyorMu) return;

        // Klavye + Joystick birlikte çalışır
        float yatay = Input.GetAxisRaw("Horizontal");
        float dikey = Input.GetAxisRaw("Vertical");

        if (virtualJoystick != null)
        {
            Vector2 joystickInput = virtualJoystick.InputVector;
            if (joystickInput.magnitude > 0.1f)
            {
                yatay = joystickInput.x;
                dikey = joystickInput.y;
            }
        }

        Vector3 kameraIleri = Camera.main.transform.forward;
        Vector3 kameraSag = Camera.main.transform.right;

        kameraIleri.y = 0f;
        kameraSag.y = 0f;
        kameraIleri.Normalize();
        kameraSag.Normalize();

        hareketYonu = (kameraIleri * dikey + kameraSag * yatay).normalized;

        anim.SetBool("isWalking", hareketYonu.magnitude > 0.1f);
    }

    void FixedUpdate()
    {
        if (yakalandiMi || hackYapiyorMu)
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            anim.SetBool("isWalking", false);
            return;
        }

        Vector3 yeniHiz = hareketYonu * yurumeHizi;
        yeniHiz.y = rb.linearVelocity.y;
        rb.linearVelocity = yeniHiz;

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
        anim.SetBool("isWalking", false);

        if (yakalanmaEkrani != null)
            yakalanmaEkrani.SetActive(true);
    }
}