using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class GuardAI : MonoBehaviour
{
    // Durak ve bekleme süresini bir arada tutan küçük bir yapý
    [System.Serializable]
    public struct WaypointData
    {
        public Transform nokta;
        public float beklemeSuresi;
    }

    [Header("Devriye Ayarlarý")]
    public WaypointData[] duraklar; // Artýk sadece Transform deðil, süre de tutuyor
    private int mevcutNoktaIndex;
    private NavMeshAgent agent;
    private bool beklemedeMi = false;

    [Header("Görüþ ve Diðer Ayarlar")]
    public Transform hirsiz;
    public float gorusMesafesi = 8f;
    [Range(0, 360)] public float gorusAcisi = 60f;
    public LayerMask engelKatmani;
    public Light gorusIsigi;

    private Animator anim;
    private bool oyunBitti = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        if (duraklar.Length > 0)
        {
            agent.SetDestination(duraklar[0].nokta.position);
        }
    }

    void Update()
    {
        if (oyunBitti) return;

        DevriyeGez();
        HirsiziAra();
        AnimasyonuYonet();
    }

    void DevriyeGez()
    {
        if (beklemedeMi || duraklar.Length == 0) return;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            StartCoroutine(BekleVeDevamEt());
        }
    }

    IEnumerator BekleVeDevamEt()
    {
        beklemedeMi = true;

        // O anki duraðýn bekleme süresini al
        float suAnkiBekleme = duraklar[mevcutNoktaIndex].beklemeSuresi;

        // Eðer süre 0'dan büyükse bekle, deðilse direkt geç
        if (suAnkiBekleme > 0)
        {
            yield return new WaitForSeconds(suAnkiBekleme);
        }

        mevcutNoktaIndex = (mevcutNoktaIndex + 1) % duraklar.Length;
        agent.SetDestination(duraklar[mevcutNoktaIndex].nokta.position);

        beklemedeMi = false;
    }

    // AnimasyonuYonet ve HirsiziAra fonksiyonlarý ayný kalacak...
    void AnimasyonuYonet()
    {
        if (agent.velocity.magnitude > 0.1f) anim.SetBool("isWalking", true);
        else anim.SetBool("isWalking", false);
    }

    void HirsiziAra()
    {
        if (hirsiz == null) return;
        Vector3 hirsizaGidenYon = (hirsiz.position - transform.position).normalized;
        float hirsizaOlanMesafe = Vector3.Distance(transform.position, hirsiz.position);

        if (hirsizaOlanMesafe < gorusMesafesi)
        {
            if (Vector3.Angle(transform.forward, hirsizaGidenYon) < gorusAcisi / 2)
            {
                if (!Physics.Raycast(transform.position, hirsizaGidenYon, hirsizaOlanMesafe, engelKatmani))
                {
                    Yakalandin();
                }
            }
        }
    }

    void Yakalandin()
    {
        oyunBitti = true;
        agent.isStopped = true;
        anim.SetBool("isWalking", false);
        if (gorusIsigi != null) gorusIsigi.color = Color.red;
        Debug.LogError("YAKALANDIN!");
        hirsiz.GetComponent<HirsizKontrol>().YakalandiEkraniniGoster();
    }
}