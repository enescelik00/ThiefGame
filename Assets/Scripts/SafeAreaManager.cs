using UnityEngine;

public class SafeAreaManager : MonoBehaviour
{
    private RectTransform panel;

    void Awake()
    {
        panel = GetComponent<RectTransform>();
        GuvenliAlaniUygula();
    }

    void GuvenliAlaniUygula()
    {
        // Telefonun fiziksel olarak kullanýlabilir "Güvenli" alanýný al
        Rect safeArea = Screen.safeArea;

        // Bu alaný Canvas'ýn anlayacaðý oranlara (0 ile 1 arasý) çevir
        Vector2 minAnchor = safeArea.position;
        Vector2 maxAnchor = minAnchor + safeArea.size;

        minAnchor.x /= Screen.width;
        minAnchor.y /= Screen.height;
        maxAnchor.x /= Screen.width;
        maxAnchor.y /= Screen.height;

        // Panelin köþelerini bu güvenli alanýn sýnýrlarýna eþitle
        panel.anchorMin = minAnchor;
        panel.anchorMax = maxAnchor;
    }
}