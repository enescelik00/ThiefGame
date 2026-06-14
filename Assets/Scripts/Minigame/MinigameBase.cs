using System;
using UnityEngine;

public abstract class MinigameBase : MonoBehaviour
{
    // Minigame baþarýyla bittiðinde tetiklenecek event
    public Action OnMinigameSuccess;

    // Minigame iptal edildiðinde (çýkýþ yapýldýðýnda) tetiklenecek event
    public Action OnMinigameCancel;

    // Zorluk ayarýný alacaðýmýz fonksiyon. 
    public virtual void SetupDifficulty(MinigameDifficulty difficulty)
    {
        Debug.Log("Minigame zorluðu ayarlandý: " + difficulty);
    }

    // Minigame kazanýldýðýnda çaðýrýlan fonksiyon
    protected void WinMinigame()
    {
        OnMinigameSuccess?.Invoke();
        Destroy(gameObject);
    }

    // KIRMIZI X BUTONUNA VERECEÐÝMÝZ FONKSÝYON BU
    public void CancelMinigame()
    {
        // Trigger scriptine iptal edildiðini duyur (Butonu geri getirmesi için)
        OnMinigameCancel?.Invoke();

        // Minigame ekranýný kapat/yok et
        Destroy(gameObject);
    }
}