using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class KopekKontrol : MonoBehaviour
{
    [Header("Hareket Ayarlarý")]
    public float yurumeHizi = 2.5f;
    public float kovalamaHizi = 6.5f; // Ýnsanlardan biraz daha hýzlý koþabilir!

    [Header("Devriye Yolu (Özel Süreli)")]
    // GuardAI'da oluþturduðumuz DurakNoktasi yapýsýný burada direkt kullanýyoruz
    public List<DurakNoktasi> waypoints;

    [Header("Görüþ ve Yakalama Ayarlarý")]
    public float gorusMesafesi = 7f;
    public float gorusAcisi = 70f;
    public float yakalamaMesafesi = 1.5f; // Hýrsýza ne kadar yaklaþýnca oyun bitsin
    public Transform gozNoktasi;
    public GameObject yakalanmaEkrani;

    private NavMeshAgent agent;
    private Animator anim;
    private int mevcutHedefIndex = 0;
    private bool bekliyorMu = false;

    // YENÝ: Av Modu Kontrolü
    private bool avModuAktif = false;
    private Transform oyuncu;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        // Hýrsýzý bul ve hafýzaya al
        oyuncu = GameObject.FindGameObjectWithTag("Player").transform;

        agent.speed = yurumeHizi;

        if (waypoints.Count > 0) HedefeGit();
    }

    void Update()
    {
        // 1. Animasyon (Blend Tree: Vert parametresi)
        if (anim != null)
        {
            // Ýster yürüsün ister koþsun, hareket ediyorsa animasyonu oynat
            anim.SetFloat("Vert", agent.velocity.magnitude > 0.1f ? 1f : 0f);
        }

        // 2. Durum Kontrolü (Kovalýyor mu yoksa Devriyede mi?)
        if (avModuAktif)
        {
            Kovala();
        }
        else
        {
            DevriyeGez();
            GorusKontroluYap(); // Sadece devriyedeyken etrafý tarasýn
        }
    }

    void DevriyeGez()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f && !bekliyorMu)
        {
            StartCoroutine(BekleVeYeniHedefeGit());
        }
    }

    void Kovala()
    {
        // Hýzý artýr ve hýrsýza kilitlen
        agent.speed = kovalamaHizi;
        agent.SetDestination(oyuncu.position);

        // Yakalama mesafesine girdi mi?
        float mesafe = Vector3.Distance(transform.position, oyuncu.position);
        if (mesafe <= yakalamaMesafesi)
        {
            Yakala();
        }
    }

    void GorusKontroluYap()
    {
        if (oyuncu == null) return;

        float mesafe = Vector3.Distance(transform.position, oyuncu.position);

        if (mesafe < gorusMesafesi)
        {
            // YENÝ: Akýllý Niþan Alma Sistemi (Ayaklara deðil göðse lazer atar)
            Vector3 hedefNokta = oyuncu.position + Vector3.up * 1.0f;
            Vector3 baslangicNoktasi = gozNoktasi.position + transform.forward * 0.5f;

            Vector3 yon = (hedefNokta - baslangicNoktasi).normalized;
            float aci = Vector3.Angle(transform.forward, yon);

            if (aci < gorusAcisi / 2f)
            {
                // Görsel Hata Ayýklama Lazer Çizgisi
                Debug.DrawRay(baslangicNoktasi, yon * gorusMesafesi, Color.red);

                RaycastHit hit;
                if (Physics.Raycast(baslangicNoktasi, yon, out hit, gorusMesafesi))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        avModuAktif = true; // Seni gördü, av moduna geçti!
                    }
                }
            }
        }
    }

    void Yakala()
    {
        if (yakalanmaEkrani != null) yakalanmaEkrani.SetActive(true);
        Time.timeScale = 0f;
    }

    void HedefeGit()
    {
        if (waypoints.Count == 0 || waypoints[mevcutHedefIndex].nokta == null) return;
        agent.SetDestination(waypoints[mevcutHedefIndex].nokta.position);
    }

    System.Collections.IEnumerator BekleVeYeniHedefeGit()
    {
        bekliyorMu = true;

        // YENÝ: Noktaya özel bekleme süresini çekiyoruz
        float ozelBeklemeSuresi = waypoints[mevcutHedefIndex].beklemeSuresi;

        mevcutHedefIndex = (mevcutHedefIndex + 1) % waypoints.Count;
        yield return new WaitForSeconds(ozelBeklemeSuresi);

        bekliyorMu = false;
        HedefeGit();
    }
}