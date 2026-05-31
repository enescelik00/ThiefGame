using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

// YENÝ: Unity'de her waypoint için hem konum hem süre girmemizi saðlayan özel liste yapýsý

public class GuardAI : MonoBehaviour
{
    [Header("Hareket Ayarlarý")]
    public float yurumeHizi = 2.5f;
    public float kovalamaHizi = 7.5f;

    [Header("Devriye Yolu (Özel Süreli)")]
    public List<DurakNoktasi> waypoints; // Listemizi yeni yapýmýza göre güncelledik

    [Header("Görüþ ve Yakalama Ayarlarý")]
    public float gorusMesafesi = 7f;
    public float gorusAcisi = 70f;
    public float yakalamaMesafesi = 1.5f;
    public Transform gozNoktasi;
    public GameObject yakalanmaEkrani;

    private NavMeshAgent agent;
    private Animator anim;
    private int mevcutHedefIndex = 0;
    private bool bekliyorMu = false;

    private bool avModuAktif = false;
    private Transform oyuncu;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        oyuncu = GameObject.FindGameObjectWithTag("Player").transform;
        agent.speed = yurumeHizi;

        if (waypoints.Count > 0) HedefeGit();
    }

    void Update()
    {
        if (avModuAktif)
        {
            Kovala();
        }
        else
        {
            DevriyeGez();
            GorusKontroluYap();
        }
    }

    void DevriyeGez()
    {
        if (anim != null)
        {
            anim.SetBool("isWalking", agent.velocity.magnitude > 0.1f);
        }

        if (!agent.pathPending && agent.remainingDistance < 0.5f && !bekliyorMu)
        {
            StartCoroutine(BekleVeYeniHedefeGit());
        }
    }

    void Kovala()
    {
        agent.speed = kovalamaHizi;
        agent.SetDestination(oyuncu.position);

        if (anim != null)
        {
            anim.SetBool("isWalking", true);
        }

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
            // KRÝTÝK DÜZELTME 1: Lazer ayaklara deðil, hýrsýzýn göðsüne/kafasýna gitsin (+1 birim yukarý)
            Vector3 hedefNokta = oyuncu.position + Vector3.up * 1.0f;

            // KRÝTÝK DÜZELTME 2: Lazer NPC'nin kendi vücuduna çarpmasýn diye baþlangýç noktasýný biraz öne (transform.forward) alýyoruz
            Vector3 baslangicNoktasi = gozNoktasi.position + transform.forward * 0.5f;

            // Yönü yeni göðüs noktasýna göre ayarlýyoruz
            Vector3 yon = (hedefNokta - baslangicNoktasi).normalized;
            float aci = Vector3.Angle(transform.forward, yon);

            if (aci < gorusAcisi / 2f)
            {
                // SAHNEDE GÖREBÝLMEK ÝÇÝN: Bu kod Scene ekranýnda kýrmýzý bir lazer çizgisi çizer!
                Debug.DrawRay(baslangicNoktasi, yon * gorusMesafesi, Color.red);

                RaycastHit hit;
                if (Physics.Raycast(baslangicNoktasi, yon, out hit, gorusMesafesi))
                {
                    // Lazer kime çarptýysa adýný konsola yazdýr (Sorunu bulmamýzý çok kolaylaþtýrýr)
                    // Debug.Log("Lazer þuna çarptý: " + hit.collider.gameObject.name);

                    if (hit.collider.CompareTag("Player"))
                    {
                        avModuAktif = true; // Yakaladý!
                    }
                }
            }
        }
    }

    void HedefeGit()
    {
        if (waypoints.Count == 0 || waypoints[mevcutHedefIndex].nokta == null) return;

        // Karakterin gideceði noktayý yeni listeden çekiyoruz
        agent.SetDestination(waypoints[mevcutHedefIndex].nokta.position);
    }

    System.Collections.IEnumerator BekleVeYeniHedefeGit()
    {
        bekliyorMu = true;

        // YENÝ: Bekleme süresini genel ayardan deðil, mevcut duraðýn özel ayarýndan çekiyoruz
        float ozelBeklemeSuresi = waypoints[mevcutHedefIndex].beklemeSuresi;

        // Sýradaki hedefe geç (index'i artýr)
        mevcutHedefIndex = (mevcutHedefIndex + 1) % waypoints.Count;

        // O duraða özel süre kadar bekle
        yield return new WaitForSeconds(ozelBeklemeSuresi);

        bekliyorMu = false;
        HedefeGit();
    }

    void Yakala()
    {
        if (yakalanmaEkrani != null) yakalanmaEkrani.SetActive(true);
        Time.timeScale = 0f;
    }
}