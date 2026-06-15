using UnityEngine;

// Bu satýr, Unity'de sað týkladýðýmýzda özel bir menü çýkmasýný saðlar
[CreateAssetMenu(fileName = "Yeni Skin", menuName = "Magaza/Skin Ýtemi")]
public class SkinItem : ScriptableObject
{
    [Header("Skin Bilgileri")]
    public int skinID;           // Her skinin benzersiz bir numarasý olmalý (0, 1, 2...)
    public string skinName;      // Kartta yazacak isim
    public int price;            // Fiyat (0 ise ücretsiz yaparýz)

    [Header("Görseller")]
    public Sprite skinIcon;      // Kartýn solundaki 2D resim
    // public GameObject skinModel; // (Bunu ileride 3D podyuma modeli çaðýrýrken kullanacaðýz, þimdilik dursun)
}