using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class GuardAI : MonoBehaviour
{
    [Header("Hareket Ayarlarý")]
    public float yurumeHizi = 2.5f;
    public float kovalamaHizi = 7.5f;

    [Header("Devriye Yolu (Özel Süreli)")]
    public List<DurakNoktasi> waypoints;

    [Header("Görüþ ve Yakalama Ayarlarý")]
    public float gorusMesafesi = 7f;
    public float gorusAcisi = 70f;
    public float yakalamaMesafesi = 1.5f;
    // YENÝ EKLENDÝ: NPC'nin arkasýnda bile olsan seni fark edeceði "dibine girme" mesafesi
    public float hissetmeMesafesi = 1.5f;
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

        // --- YENÝ EKLENEN SÝSTEM: 6. HÝS / DÝBÝNE GÝRME ---
        // Eðer oyuncu görüþ açýsýnda olmasa bile dibimize kadar girdiyse anýnda fark et!
        if (mesafe <= hissetmeMesafesi)
        {
            avModuAktif = true;
            return; // Direkt yakaladýðý için aþaðýdaki lazer hesaplamalarýný yapmaya gerek kalmaz.
        }
        // --------------------------------------------------

        if (mesafe < gorusMesafesi)
        {
            Vector3 hedefNokta = oyuncu.position + Vector3.up * 1.0f;
            Vector3 baslangicNoktasi = gozNoktasi.position + transform.forward * 0.5f;
            Vector3 yon = (hedefNokta - baslangicNoktasi).normalized;
            float aci = Vector3.Angle(transform.forward, yon);

            if (aci < gorusAcisi / 2f)
            {
                Debug.DrawRay(baslangicNoktasi, yon * gorusMesafesi, Color.red);

                RaycastHit hit;
                if (Physics.Raycast(baslangicNoktasi, yon, out hit, gorusMesafesi))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        avModuAktif = true;
                    }
                }
            }
        }
    }

    void HedefeGit()
    {
        if (waypoints.Count == 0 || waypoints[mevcutHedefIndex].nokta == null) return;
        agent.SetDestination(waypoints[mevcutHedefIndex].nokta.position);
    }

    System.Collections.IEnumerator BekleVeYeniHedefeGit()
    {
        bekliyorMu = true;
        float ozelBeklemeSuresi = waypoints[mevcutHedefIndex].beklemeSuresi;
        mevcutHedefIndex = (mevcutHedefIndex + 1) % waypoints.Count;
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