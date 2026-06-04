using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    [HideInInspector] public int cardID;
    public Image cardBackImage;
    public Image iconImage;

    private bool isRevealed = false;
    private Button btn;
    private Coroutine flipCoroutine;

    void Awake()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClicked);
    }

    public void SetupCard(int id, Sprite iconSprite)
    {
        cardID = id;
        iconImage.sprite = iconSprite;

        isRevealed = false;
        iconImage.gameObject.SetActive(false);
        transform.localScale = Vector3.one;
    }

    void OnClicked()
    {
        if (isRevealed || !MemoryGameManager.Instance.canClick) return;

        RevealCard();
        MemoryGameManager.Instance.OnCardSelected(this);
    }

    public void RevealCard()
    {
        isRevealed = true;
        if (flipCoroutine != null) StopCoroutine(flipCoroutine);
        flipCoroutine = StartCoroutine(FlipAnimation(true));
    }

    public void CloseCard()
    {
        isRevealed = false;
        if (flipCoroutine != null) StopCoroutine(flipCoroutine);
        flipCoroutine = StartCoroutine(FlipAnimation(false));
    }

    public void MatchFound()
    {
        btn.interactable = false;
    }

    private IEnumerator FlipAnimation(bool showFace)
    {
        float duration = 0.15f;
        float elapsedTime = 0f;
        Vector3 currentScale = transform.localScale;

        while (elapsedTime < duration)
        {
            currentScale.x = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            transform.localScale = currentScale;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        currentScale.x = 0f;
        transform.localScale = currentScale;
        iconImage.gameObject.SetActive(showFace);

        elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            currentScale.x = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            transform.localScale = currentScale;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        currentScale.x = 1f;
        transform.localScale = currentScale;
    }
}