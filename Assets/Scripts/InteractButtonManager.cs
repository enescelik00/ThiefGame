using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class InteractButtonManager : MonoBehaviour
{
    public static InteractButtonManager Instance; // Her yerden kolayca eriþebilmek için Singleton

    [Header("UI Referanslarý")]
    public Button interactButton; // Ekranda çýkacak olan buton (Inspector'dan atanacak)

    private void Awake()
    {
        // Singleton Kurulumu
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Oyun baþladýðýnda buton gizli olsun
        if (interactButton != null)
        {
            interactButton.gameObject.SetActive(false);
        }
    }

    // Karakter objeye yaklaþtýðýnda çaðrýlacak fonksiyon
    public void ShowButton(UnityAction onClickAction)
    {
        if (interactButton == null) return;

        // Önceki objeden kalan týklama görevlerini temizle (Çakýþmayý önler)
        interactButton.onClick.RemoveAllListeners();

        // Yeni minigame'i baþlatacak görevi butona ekle
        interactButton.onClick.AddListener(onClickAction);

        // Butonu görünür yap
        interactButton.gameObject.SetActive(true);
    }

    // Karakter objeden uzaklaþtýðýnda çaðrýlacak fonksiyon
    public void HideButton()
    {
        if (interactButton == null) return;

        // Görevleri temizle ve butonu gizle
        interactButton.onClick.RemoveAllListeners();
        interactButton.gameObject.SetActive(false);
    }
}
