using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class YakalanmaEfekti : MonoBehaviour
{
    [Header("Arayüz Objeleri")]
    public Image siyahArkaPlan;
    public GameObject onPlanaCikacakResim; // YENÝ: Resmimizi buraya tanýmlýyoruz

    [Header("Ayarlar")]
    public float kararmaHizi = 1.5f;

    void OnEnable()
    {
        // 1. SÝHÝRLÝ KOMUT: Unity'ye bu resmi zorla en öne (hiyerarþinin en altýna) çizmesini emrediyoruz!
        if (onPlanaCikacakResim != null)
        {
            onPlanaCikacakResim.SetActive(true);
            onPlanaCikacakResim.transform.SetAsLastSibling();
        }

        // 2. Kararma iþlemini hazýrlayýp baþlatýyoruz
        if (siyahArkaPlan != null)
        {
            Color c = siyahArkaPlan.color;
            c.a = 0f; // Siyah ekran görünmez baþlasýn
            siyahArkaPlan.color = c;

            StartCoroutine(KarartmaRutini());
        }
    }

    IEnumerator KarartmaRutini()
    {
        float saydamlik = 0f;

        while (saydamlik < 1f)
        {
            saydamlik += Time.unscaledDeltaTime * kararmaHizi;

            Color c = siyahArkaPlan.color;
            c.a = Mathf.Clamp01(saydamlik);
            siyahArkaPlan.color = c;

            yield return null;
        }
    }
}