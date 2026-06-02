using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryGameManager : MonoBehaviour
{
    public static MemoryGameManager Instance;

    [Header("Ayarlar")]
    public Transform cardContainer;     // Grid Layout olan nesne
    public GameObject cardPrefab;       // Card_Prefab nesnesi
    public Sprite[] cardIcons;          // 8 adet simge görselin

    [Header("Durum")]
    public bool canClick = true;
    private List<CardController> allCards = new List<CardController>();
    private CardController firstSelected;
    private CardController secondSelected;

    void Awake() => Instance = this;

    void Start()
    {
        InitializeGame();
    }

    void InitializeGame()
    {
        // 8 simgeden 16'lýk bir liste oluþtur (her birinden ikiþer tane)
        List<int> ids = new List<int>();
        for (int i = 0; i < cardIcons.Length; i++)
        {
            ids.Add(i);
            ids.Add(i);
        }

        // Listeyi karýþtýr (Shuffle)
        for (int i = 0; i < ids.Count; i++)
        {
            int temp = ids[i];
            int randomIndex = Random.Range(i, ids.Count);
            ids[i] = ids[randomIndex];
            ids[randomIndex] = temp;
        }

        // Kartlarý oluþtur
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
        canClick = false; // Giriþleri kilitle

        if (firstSelected.cardID == secondSelected.cardID)
        {
            // Eþleþme Bildirimi
            firstSelected.MatchFound();
            secondSelected.MatchFound();
            Debug.Log("Eþleþme Tamam!");
        }
        else
        {
            // Yanlýþ seçim, 0.7 saniye bekle ve geri kapat
            yield return new WaitForSeconds(0.7f);
            firstSelected.CloseCard();
            secondSelected.CloseCard();
        }

        firstSelected = null;
        secondSelected = null;
        canClick = true; // Giriþleri aç
    }
}