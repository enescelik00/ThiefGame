using UnityEngine;

public class WireTaskManager : MonoBehaviour
{
    [Header("Görev Ayarlarý")]
    public int totalWires = 4;

    private int connectedWires = 0;

    public void WireConnected()
    {
        connectedWires++;
        Debug.Log(connectedWires + "/" + totalWires + " kablo baðlandý.");

        if (connectedWires >= totalWires)
            TaskComplete();
    }

    private void TaskComplete()
    {
        Debug.Log("<color=green>GÖREV BAÞARILI!</color>");
        gameObject.SetActive(false);
    }
}