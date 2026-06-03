using UnityEngine;
using TMPro;

public class AnaMenuBanka : MonoBehaviour
{
    public TextMeshProUGUI toplamParaText;

    void Start()
    {
        // Cihaza kayýtlý parayý çek (Eðer daha önce hiç oynanmadýysa 0 getirir)
        int bakiye = PlayerPrefs.GetInt("ToplamParam", 0);

        // Ekrana yansýt
        if (toplamParaText != null)
        {
            toplamParaText.text = "Bakiye: $" + bakiye;
        }
    }
}