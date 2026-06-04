using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 1. MonoBehaviour yerine MinigameBase'den miras alýyoruz
public class MemoryGameManager : MinigameBase
{
    public static MemoryGameManager Instance;

    [Header("Zorluk Ayarlarý")]
    [Range(2, 8)]
    [Tooltip("Oyunda kaç ÇÝFT kart olacak? (Örn: 4 = Kolay, 8 = Zor)")]
    public int pairsToPlay = 8; // SetupDifficulty ile bu sayý ezilecek

    [Header("UI Ayarlarý")]
    public GameObject mainPanel;
    public Transform cardContainer;
    public GameObject cardPrefab;
    public Sprite[] cardIcons;
    public GridLayoutGroup gridLayout;

    [Header("Ses Ayarlarý")]
    public AudioSource audioSource;
    public AudioClip matchSuccessSound;

    [Header("Oyun Durumu")]
    public bool canClick = true;
    private List<CardController> allCards = new List<CardController>();
    private CardController firstSelected;
    private CardController secondSelected;

    private int matchesFound = 0;

    void Awake() => Instance = this;

    // 2. Start metodunu kaldýrdýk çünkü baþlatma iþini SetupDifficulty'e devrediyoruz.

    public override void SetupDifficulty(MinigameDifficulty difficulty)
    {
        // Temel sýnýftaki ayarý çaðýr
        base.SetupDifficulty(difficulty);

        // Kasadan gelen zorluða göre çift kart sayýsýný belirle
        if (difficulty == MinigameDifficulty.Easy)
            pairsToPlay = 4; // 4 çift (8 kart)
        else if (difficulty == MinigameDifficulty.Medium)
            pairsToPlay = 6; // 6 çift (12 kart)
        else if (difficulty == MinigameDifficulty.Hard)
            pairsToPlay = 8; // 8 çift (16 kart)

        // Zorluk belirlendikten sonra kartlarý daðýt
        InitializeGame();
    }

    void InitializeGame()
    {
        matchesFound = 0;
        allCards.Clear();

        // Güvenlik önlemi: Ýstenen çift sayýsý, elimizdeki ikon sayýsýný aþmasýn
        if (pairsToPlay > cardIcons.Length) pairsToPlay = cardIcons.Length;

        // Grid'i otomatik ayarla
        if (gridLayout != null)
        {
            gridLayout.constraintCount = pairsToPlay;
        }

        // 1. Önce elimizdeki tüm resimlerin ID'lerini bir havuza atalým (Örn: 0'dan 7'ye kadar)
        List<int> availableIcons = new List<int>();
        for (int i = 0; i < cardIcons.Length; i++)
        {
            availableIcons.Add(i);
        }

        // 2. Bu havuzdan, zorluða göre istenen sayý kadar RASTGELE resim seçelim
        List<int> selectedIconIds = new List<int>();
        for (int i = 0; i < pairsToPlay; i++)
        {
            int randomIndex = Random.Range(0, availableIcons.Count);
            selectedIconIds.Add(availableIcons[randomIndex]);

            // Ayný resmi iki farklý çiftmiþ gibi seçmemek için seçileni havuzdan çýkar
            availableIcons.RemoveAt(randomIndex);
        }

        // 3. Seçilen bu rastgele resimlerin her birinden oyuna 2'þer adet kart (çift) ekle
        List<int> ids = new List<int>();
        foreach (int iconId in selectedIconIds)
        {
            ids.Add(iconId);
            ids.Add(iconId);
        }
        // Karýþtýr
        for (int i = 0; i < ids.Count; i++)
        {
            int temp = ids[i];
            int randomIndex = Random.Range(i, ids.Count);
            ids[i] = ids[randomIndex];
            ids[randomIndex] = temp;
        }

        // Container içindeki eski kartlarý temizle
        foreach (Transform child in cardContainer)
        {
            Destroy(child.gameObject);
        }

        // Kartlarý yarat
        foreach (int id in ids)
        {
            GameObject newCard = Instantiate(cardPrefab, cardContainer);
            CardController controller = newCard.GetComponent<CardController>();
            controller.SetupCard(id, cardIcons[id]);
            allCards.Add(controller);
        }
    }

    public void OnCardSelected(CardController card)
    {
        if (firstSelected == null)
        {
            firstSelected = card;
        }
        else
        {
            secondSelected = card;
            StartCoroutine(CheckMatch());
        }
    }

    IEnumerator CheckMatch()
    {
        canClick = false;

        yield return new WaitForSeconds(0.3f);

        if (firstSelected.cardID == secondSelected.cardID)
        {
            // EÞLEÞME BAÞARILI
            if (audioSource != null && matchSuccessSound != null)
                audioSource.PlayOneShot(matchSuccessSound);

            firstSelected.MatchFound();
            secondSelected.MatchFound();

            matchesFound++;
            Debug.Log($"Þifre Kýrýldý: {matchesFound} / {pairsToPlay}");

            // OYUN BÝTÝÞ KONTROLÜ
            if (matchesFound >= pairsToPlay)
            {
                firstSelected = null;
                secondSelected = null;
                StartCoroutine(FinishMinigameRoutine());
                yield break;
            }
        }
        else
        {
            // EÞLEÞME BAÞARISIZ
            yield return new WaitForSeconds(0.6f);
            firstSelected.CloseCard();
            secondSelected.CloseCard();
        }

        firstSelected = null;
        secondSelected = null;
        canClick = true;
    }

    IEnumerator FinishMinigameRoutine()
    {
        // Sesin bitmesi ve oyuncunun son kartý görmesi için bekleme
        yield return new WaitForSeconds(1f);

        Debug.Log("Hafýza Minigame Tamamlandý! Base class'a bildiriliyor...");

        // 3. MinigameBase içindeki kazanma fonksiyonunu çaðýrýyoruz.
        // Bu fonksiyon hem kapýya haber verecek hem de prefabý yok edecek.
        WinMinigame();
    }
}