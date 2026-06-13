using UnityEngine;
using System.Collections;

public class FadeKontrol : MonoBehaviour
{
    public CanvasGroup loseCanvasGroup;
    public float fadeSuresi = 1.5f; // Saniyesini buradan ayarlayabilirsin

    // Obje SetActive(true) olduðu AN burasý otomatik çalýþýr!
    void OnEnable()
    {
        if (loseCanvasGroup != null)
        {
            loseCanvasGroup.alpha = 0f; // Önce þeffaf yap
            StartCoroutine(DoFadeIn()); // Animasyonu baþlat
        }
        SesYoneticisi.instance.YakalanmaSesiniCal();
    }

    private IEnumerator DoFadeIn()
    {
        float gecenSure = 0f;

        while (gecenSure < fadeSuresi)
        {
            gecenSure += Time.unscaledDeltaTime; // Oyun dursa bile zamaný say
            loseCanvasGroup.alpha = gecenSure / fadeSuresi;
            yield return null;
        }

        loseCanvasGroup.alpha = 1f; // En son tam netleþtir
    }
}