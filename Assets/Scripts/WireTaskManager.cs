using UnityEngine;

public class WireTaskManager : MonoBehaviour
{
    [Header("Görev Ayarlarý")]
    public int totalWires = 4; // Toplam baðlanmasý gereken kablo sayýsý
    private int connectedWires = 0; // Þu ana kadar baðlananlar

    // Bir kablo doðru baðlandýðýnda bu fonksiyon tetiklenecek
    public void WireConnected()
    {
        connectedWires++;
        Debug.Log(connectedWires + "/" + totalWires + " kablo baðlandý.");

        // Eðer hepsi baðlandýysa görevi bitir
        if (connectedWires >= totalWires)
        {
            TaskComplete();
        }
    }

    private void TaskComplete()
    {
        Debug.Log("<color=green>GÖREV BAÞARILI! Panel Kapanýyor...</color>");

        // Görev bitince UI panelini kapat.
        // (Eðer Panel_WireTask kapanýrsa, üzerindeki MiniGameController 
        // otomatik olarak zamaný Time.timeScale = 1 yapýp arka planý akýtmaya devam edecek!)
        gameObject.SetActive(false);
    }
}