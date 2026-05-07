using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio; // Mixer için þart
using TMPro; // Dropdown için þart
using System.Collections;
using System.Collections.Generic;

public class SettingsManager : MonoBehaviour
{
    [Header("Ses ve Mixer")]
    public AudioMixer mainMixer;
    public Slider musicSlider;
    public Slider soundSlider;

    [Header("Titreþim")]
    public Toggle vibrationToggle;
    public RectTransform toggleHandle;
    public Image toggleBackground;
    public Color onColor, offColor;
    public float onPositionX = -4f, offPositionX = -44f;

    [Header("Dil Seçimi")]
    public TMP_Dropdown languageDropdown;

    void Start()
    {
        LoadSettings();
    }

    // --- SES FONKSÝYONLARI ---
    public void SetMusicVolume(float value)
    {
        // Logaritmik dönüþüm: 0.0001 ile 1 arasýný -80dB ile 0dB arasýna çeker
        float dbValue = Mathf.Log10(Mathf.Max(0.0001f, value)) * 20;
        mainMixer.SetFloat("MusicVol", dbValue);
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void SetSoundVolume(float value)
    {
        float dbValue = Mathf.Log10(Mathf.Max(0.0001f, value)) * 20;
        mainMixer.SetFloat("SoundVol", dbValue);
        PlayerPrefs.SetFloat("SoundVolume", value);
    }

    // --- DÝL FONKSÝYONU ---
    public void SetLanguage(int index)
    {
        PlayerPrefs.SetInt("LanguageIndex", index);
        Debug.Log("Seçilen Dil ID: " + index);
        // Burada ileride Localization paketini tetikleyeceðiz
    }

    // --- TÝTREÞÝM (Önceki Turn'den) ---
    public void SetVibration(bool isOn)
    {
        PlayerPrefs.SetInt("Vibration", isOn ? 1 : 0);
        StopAllCoroutines();
        StartCoroutine(SlideToggle(isOn));
    }

    private IEnumerator SlideToggle(bool isOn)
    {
        float targetX = isOn ? onPositionX : offPositionX;
        Vector2 startPos = toggleHandle.anchoredPosition;
        Color startColor = toggleBackground.color;
        Color targetColor = isOn ? onColor : offColor;

        float time = 0;
        while (time < 0.15f)
        {
            time += Time.deltaTime;
            float t = time / 0.15f;
            toggleHandle.anchoredPosition = Vector2.Lerp(startPos, new Vector2(targetX, startPos.y), t);
            toggleBackground.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }
    }

    private void LoadSettings()
    {
        // Sesleri Yükle
        float mVol = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        musicSlider.value = mVol;
        SetMusicVolume(mVol);

        float sVol = PlayerPrefs.GetFloat("SoundVolume", 0.75f);
        soundSlider.value = sVol;
        SetSoundVolume(sVol);

        // Titreþimi Yükle
        bool isVib = PlayerPrefs.GetInt("Vibration", 1) == 1;
        vibrationToggle.isOn = isVib;
        toggleHandle.anchoredPosition = new Vector2(isVib ? onPositionX : offPositionX, toggleHandle.anchoredPosition.y);
        toggleBackground.color = isVib ? onColor : offColor;

        // Dili Yükle
        languageDropdown.value = PlayerPrefs.GetInt("LanguageIndex", 0);
    }
}