using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkinCard : MonoBehaviour
{
    public Image skinIcon;
    public TextMeshProUGUI skinNameText;
    public TextMeshProUGUI priceText;
    public Button actionButton;
    public TextMeshProUGUI buttonText;

    [HideInInspector] public int skinIndex; // Bu kartýn hangi skine ait olduðunu tutacak
}