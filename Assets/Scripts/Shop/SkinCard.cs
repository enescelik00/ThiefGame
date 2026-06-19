using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkinCard : MonoBehaviour
{
    public int skinIndex;
    public TextMeshProUGUI skinNameText;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI buttonText;
    public Button actionButton;

    [Header("UI Görselleri")]
    public Image skinIcon;
    public Image arkaPlanImage; // Kartýn arka plan objesi
    public Image butonImage;    // Butonun görsel objesi
    public Image glowImage;     // Kapatýp açacaðýmýz ýþýklý çerçeve objesi
}