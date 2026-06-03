using UnityEngine;

public class KasaTetikleyici : MonoBehaviour
{
    [Header("Arayüz")]
    public GameObject kazanmaEkrani;

    // HATA DÜZELTÝLDÝ: Boþluk silindi
    private bool oyunBittiMi = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !oyunBittiMi)
        {
            oyunBittiMi = true;

            // 1. Görev sistemini güncelle 
            if (GorevSistemi.instance != null)
            {
                GorevSistemi.instance.KasaAcildi();
            }

            // 2. Kazanma ekranýný görünür yap
            if (kazanmaEkrani != null)
            {
                kazanmaEkrani.SetActive(true);
            }

            // 3. Oyunu dondur ve fareyi görünür yap
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}