using UnityEngine;
using UnityEngine.AI;

public class DogAI : MonoBehaviour
{
    [Header("Devriye Ayarlarý (NavMesh)")]
    public Transform[] duraklar;
    private int mevcutNoktaIndex = 0;
    private NavMeshAgent agent;

    [Header("Görüþ Ayarlarý")]
    public Transform hirsiz;
    public float gorusMesafesi = 6f;
    [Range(0, 360)] public float gorusAcisi = 120f;
    public LayerMask engelKatmani;
    public Light gorusIsigi;

    private bool oyunBitti = false;
    private Animator anim;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        if (duraklar.Length > 0)
        {
            agent.SetDestination(duraklar[0].position);
        }
    }

    void Update()
    {
        if (oyunBitti) return;

        DevriyeGez();
        HirsiziAra();
    }

    void DevriyeGez()
    {
        if (duraklar.Length == 0) return;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            mevcutNoktaIndex = (mevcutNoktaIndex + 1) % duraklar.Length;
            agent.SetDestination(duraklar[mevcutNoktaIndex].position);
        }
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
        anim.speed = 0f; // Animasyonu dondur

        if (gorusIsigi != null) gorusIsigi.color = Color.red;
        Debug.LogError("KÖPEÐE YAKALANDIN!");
    }
}