using UnityEngine;

[CreateAssetMenu(fileName = "Yeni Skin", menuName = "Magaza/Skin Ýtemi")]
public class SkinItem : ScriptableObject
{
    public int skinID;
    public string skinName;
    public int price;

    [Header("Model Ayarý")]
    public GameObject skinPrefab;

    [Header("Özel Kart Tasarýmlarý")]
    public Sprite skinIcon;
    public Sprite arkaPlanResmi; // YENÝ: Kartýn kendi arkasý
    public Sprite butonResmi;    // YENÝ: Karta özel buton
    public Color glowRengi = Color.white; // YENÝ: Glow hangi renk yanacak?
}