using UnityEngine;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    [HideInInspector] public int cardID; // Eþleþme kontrolü için kimlik
    public Image cardBackImage;       // Kartýn arkasý (cardBackground)
    public Image iconImage;           // Kartýn önündeki simge

    private bool isRevealed = false;
    private Button btn;

    void Awake()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClicked);
    }

    public void SetupCard(int id, Sprite iconSprite)
    {
        cardID = id;
        iconImage.sprite = iconSprite;
        CloseCard(); // Baþlangýçta kapalý
    }

    void OnClicked()
    {
        // Eðer kart zaten açýksa veya Manager giriþleri kilitlemiþse týklama
        if (isRevealed || !MemoryGameManager.Instance.canClick) return;

        RevealCard();
        MemoryGameManager.Instance.OnCardSelected(this);
    }

    public void RevealCard()
    {
        isRevealed = true;
        iconImage.gameObject.SetActive(true); // Simgeyi göster
        // Buraya ileride görsel efekt/animasyon ekleyeceðiz
    }

    public void CloseCard()
    {
        isRevealed = false;
        iconImage.gameObject.SetActive(false); // Simgeyi gizle
    }

    public void MatchFound()
    {
        btn.interactable = false; // Artýk týklanamaz
        // Buraya "baþarýlý" efekti (örn: yeþil yanma) eklenebilir
    }
}