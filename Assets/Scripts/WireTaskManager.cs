using UnityEngine;

// MonoBehaviour yerine MinigameBase'den miras alýyoruz
public class WireTaskManager : MinigameBase
{
    [Header("Görev Ayarlarý")]
    public int totalWires = 4;

    private int connectedWires = 0;

    // Eðer zorluk seviyesine göre kablo sayýsýný deðiþtirmek istersen
    // SetupDifficulty fonksiyonunu burada ezip (override) kullanabilirsin.
    public override void SetupDifficulty(MinigameDifficulty difficulty)
    {
        base.SetupDifficulty(difficulty);

        // Örnek bir mantýk (Ýstersen kullanabilirsin):
        // if (difficulty == MinigameDifficulty.Easy) totalWires = 2;
        // else if (difficulty == MinigameDifficulty.Hard) totalWires = 4;
    }

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

        // Kendi yazdýðýn gameObject.SetActive(false); yerine 
        // artýk MinigameBase içindeki WinMinigame() metodunu çaðýrýyoruz.
        // Bu metod tetikleyiciye (MinigameTrigger) haber verecek ve prefabý temizleyecek.
        WinMinigame();
    }
}