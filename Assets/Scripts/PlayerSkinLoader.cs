using UnityEngine;

public class PlayerSkinLoader : MonoBehaviour
{
    [Header("Maðaza Veritabaný")]
    public SkinItem[] skinDatabase; // Maðazadaki ScriptableObject listesini buraya da baðlýyoruz

    [Header("Görselin Doðacaðý Yer")]
    public Transform gorselYuva; // Karakterin altýndaki boþ child obje

    void Start()
    {
        // 1. Maðazada kuþanýlan skin ID'sini oku
        int secilenSkinID = PlayerPrefs.GetInt("EquippedSkinID", 0);

        // 2. O skini karakterin içine yükle
        SkinleriYukle(secilenSkinID);
    }

    void SkinleriYukle(int id)
    {
        // Varsa içerideki eski görseli temizle
        foreach (Transform child in gorselYuva)
        {
            Destroy(child.gameObject);
        }

        // Veritabanýndan doðru prefabý bul ve doðurt
        foreach (SkinItem skin in skinDatabase)
        {
            if (skin.skinID == id && skin.skinPrefab != null)
            {
                GameObject yeniGorsel = Instantiate(skin.skinPrefab, gorselYuva);

                // Pozisyon ve rotasyonu sýfýrla ki ana collider'ýn tam ortasýna otursun
                yeniGorsel.transform.localPosition = Vector3.zero;

                // Meshy modelleri yüzü arkaya dönük doðuyorsa burayý yine (0, 180, 0) yapabilirsin
                yeniGorsel.transform.localRotation = Quaternion.identity;

                break;
            }
        }
    }
}