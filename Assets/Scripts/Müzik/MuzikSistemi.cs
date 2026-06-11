using UnityEngine;

public class MuzikSistemi : MonoBehaviour
{
    private static MuzikSistemi instance;

    void Awake()
    {
        // Eðer sahnede zaten bir müzik yöneticisi yoksa, beni "Ölümsüz" yap
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Sahneler arasý geçiþte silinmemi engeller
        }
        else
        {
            // Eðer level yeniden baþladýðýnda ben zaten varsam, yeni kopyamý yok et (Müzik üst üste binmesin)
            Destroy(gameObject);
        }
    }
}