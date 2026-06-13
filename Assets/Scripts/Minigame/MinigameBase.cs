using System;
using UnityEngine;

public abstract class MinigameBase : MonoBehaviour
{
    // Minigame baþarýyla bittiðinde tetiklenecek event
    public Action OnMinigameSuccess;

    // Zorluk ayarýný alacaðýmýz fonksiyon. 
    // Bunu kendi minigame scriptlerinde override edip içini dolduracaksýn.
    public virtual void SetupDifficulty(MinigameDifficulty difficulty)
    {
        Debug.Log("Minigame zorluðu ayarlandý: " + difficulty);
    }

    // Minigame kazanýldýðýnda senin çaðýracaðýn fonksiyon
    protected void WinMinigame()
    {
        // Dinleyen bir script varsa ona haber ver (Kasa, kapý vs.)
        OnMinigameSuccess?.Invoke();

        // Minigame ekranýný kapat/yok et
        Destroy(gameObject);
    }
}