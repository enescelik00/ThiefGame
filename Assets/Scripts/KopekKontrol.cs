using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class KopekKontrol : MonoBehaviour
{
    [Header("Hareket Ayarlarý")]
    public float yurumeHizi = 2.5f;
    public float beklemeSuresi = 3f;

    [Header("Devriye Yolu")]
    public List<Transform> waypoints;

    [Header("Görüþ Ayarlarý")]
    public float gorusMesafesi = 7f;
    public float gorusAcisi = 70f; // Iþýðýn açýsýyla benzer yapabilirsin
    public Transform gozNoktasi; // Iþýðýn olduðu yeri buraya sürükle

    [Header("Yakalanma Ayarlarý")]
    public GameObject yakalanmaEkrani;

    private NavMeshAgent agent;
    private Animator anim;
    private int mevcutHedefIndex = 0;
    private bool bekliyorMu = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        agent.speed = yurumeHizi;

        if (waypoints.Count > 0) HedefeGit();
    }

    void Update()
    {
        // 1. Animasyon (Blend Tree: Vert parametresi)
        if (anim != null)
        {
            anim.SetFloat("Vert", agent.velocity.magnitude > 0.1f ? 1f : 0f);
        }

        // 2. Devriye Kontrolü
        if (!agent.pathPending && agent.remainingDistance < 0.5f && !bekliyorMu)
        {
            StartCoroutine(BekleVeYeniHedefeGit());
        }

        // 3. Görüþ Alaný Taramasý (Sürekli Kontrol)
        GorusKontroluYap();
    }

    void GorusKontroluYap()
    {
        // "Player" tagine sahip hýrsýzý bul
        GameObject oyuncu = GameObject.FindGameObjectWithTag("Player");
        if (oyuncu == null) return;

        // Köpekle oyuncu arasýndaki mesafeyi ölç
        float mesafe = Vector3.Distance(transform.position, oyuncu.transform.position);

        if (mesafe < gorusMesafesi)
        {
            // Köpekten oyuncuya doðru olan yönü bul
            Vector3 yon = (oyuncu.transform.position - transform.position).normalized;

            // Köpeðin baktýðý yön ile oyuncunun olduðu yön arasýndaki açýyý ölç
            float aci = Vector3.Angle(transform.forward, yon);

            if (aci < gorusAcisi / 2f)
            {
                // Görüþ alanýnda, peki arada engel (duvar vb.) var mý?
                RaycastHit hit;
                if (Physics.Raycast(gozNoktasi.position, yon, out hit, gorusMesafesi))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        Yakala();
                    }
                }
            }
        }
    }

    void Yakala()
    {
        if (yakalanmaEkrani != null) yakalanmaEkrani.SetActive(true);
        Time.timeScale = 0f; // Oyunu dondur
    }

    void HedefeGit()
    {
        if (waypoints.Count == 0) return;
        agent.SetDestination(waypoints[mevcutHedefIndex].position);
    }

    System.Collections.IEnumerator BekleVeYeniHedefeGit()
    {
        bekliyorMu = true;
        mevcutHedefIndex = (mevcutHedefIndex + 1) % waypoints.Count;
        yield return new WaitForSeconds(beklemeSuresi);
        bekliyorMu = false;
        HedefeGit();
    }
}