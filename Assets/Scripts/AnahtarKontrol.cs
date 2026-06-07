using UnityEngine;

public class AnahtarKontrol : MonoBehaviour
{
    [Header("Baðlantýlar")]
    public Transform oyuncuEnvanteri;
    public GameObject arkadasininMinigameObjesi; // Tikini kapattýðýmýz obje

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (AnahtarVarMi())
            {
                // Anahtar varsa arkadaþýnýn minigame sistemini AKTÝF ET
                if (arkadasininMinigameObjesi != null)
                {
                    arkadasininMinigameObjesi.SetActive(true);
                }
            }
            else
            {
                Debug.Log("Kapý kilitli! Anahtarý bulman lazým.");
            }
        }
    }

    bool AnahtarVarMi()
    {
        foreach (Transform esya in oyuncuEnvanteri)
        {
            if (esya.name.Contains("Anahtar") || esya.CompareTag("Anahtar"))
            {
                // Ýstersen anahtarý kullandýktan sonra silmek için aþaðýdaki satýrý açabilirsin:
                // Destroy(esya.gameObject);
                return true;
            }
        }
        return false;
    }
}