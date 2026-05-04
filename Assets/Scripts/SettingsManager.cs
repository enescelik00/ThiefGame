using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SettingsManager : MonoBehaviour
{
    [Header("Ses Ayarlarý")]
    public Slider musicSlider;
    public Slider soundSlider;

    [Header("Titreþim Ayarlarý")]
    public Toggle vibrationToggle;
    public RectTransform toggleHandle;
    public float onPositionX = -4f;
    public float offPositionX = -44f;

    [Header("Titreþim Renkleri")]
    public Image toggleBackground; // Renk deðiþtirecek olan arka planýmýz (Yeþil Hap)
    public Color onColor;  // Açýkkenki renk (Yeþil)
    public Color offColor; // Kapalýykenki renk (Gri)

    void Start()
    {
        LoadSettings();
    }

    public void SetMusicVolume(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void SetSoundVolume(float value)
    {
        PlayerPrefs.SetFloat("SoundVolume", value);
    }

    public void SetVibration(bool isOn)
    {
        PlayerPrefs.SetInt("Vibration", isOn ? 1 : 0);
        StopAllCoroutines();
        StartCoroutine(SlideToggle(isOn));
    }

    // Hem pozisyonu hem rengi ayný anda yumuþakça deðiþtiren animasyon
    private IEnumerator SlideToggle(bool isOn)
    {
        float targetX = isOn ? onPositionX : offPositionX;
        Vector2 startPos = toggleHandle.anchoredPosition;
        Vector2 targetPos = new Vector2(targetX, startPos.y);

        Color startColor = toggleBackground.color;
        Color targetColor = isOn ? onColor : offColor;

        float time = 0;
        float duration = 0.15f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration; // 0 ile 1 arasýnda zaman çarpaný

            // Pozisyonu kaydýr
            toggleHandle.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);

            // Rengi soldur/canlandýr
            toggleBackground.color = Color.Lerp(startColor, targetColor, t);

            yield return null;
        }

        // Animasyon bitince tam deðerleri oturt (hata payýný önlemek için)
        toggleHandle.anchoredPosition = targetPos;
        toggleBackground.color = targetColor;
    }

    private void LoadSettings()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        soundSlider.value = PlayerPrefs.GetFloat("SoundVolume", 1f);

        bool isVibrationOn = PlayerPrefs.GetInt("Vibration", 1) == 1;
        vibrationToggle.isOn = isVibrationOn;

        // Oyun ilk açýldýðýnda renk ve pozisyon anýnda ayarlansýn (animasyonsuz)
        toggleHandle.anchoredPosition = new Vector2(isVibrationOn ? onPositionX : offPositionX, toggleHandle.anchoredPosition.y);
        toggleBackground.color = isVibrationOn ? onColor : offColor;
    }
}