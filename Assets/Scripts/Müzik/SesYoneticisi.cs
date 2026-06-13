using UnityEngine;

public class SesYoneticisi : MonoBehaviour
{
    public static SesYoneticisi instance;

    [Header("Ses Kaynaðý")]
    public AudioSource sfxKaynak;

    [Header("Ses Dosyalarý (Klipler)")]
    public AudioClip kapiSesi;
    public AudioClip sandikSesi;
    public AudioClip yakalanmaSesi;
    public AudioClip kazanmaSesi;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // SENÝN AYARLAR MENÜSÜNDEKÝ KAYDI OKUYOR!
        // SettingsManager "SoundVolume" ismiyle kaydettiði için burayý ona uyarladýk.
        float kayitliSfxSesi = PlayerPrefs.GetFloat("SoundVolume", 0.75f);
        if (sfxKaynak != null)
        {
            sfxKaynak.volume = kayitliSfxSesi;
        }
    }

    // --- Ses Çalma Fonksiyonlarý ---
    public void KapiSesiniCal()
    {
        if (kapiSesi != null) sfxKaynak.PlayOneShot(kapiSesi);
    }

    public void SandikSesiniCal()
    {
        if (sandikSesi != null) sfxKaynak.PlayOneShot(sandikSesi);
    }

    public void YakalanmaSesiniCal()
    {
        if (yakalanmaSesi != null) sfxKaynak.PlayOneShot(yakalanmaSesi);
    }

    public void KazanmaSesiniCal()
    {
        if (kazanmaSesi != null) sfxKaynak.PlayOneShot(kazanmaSesi);
    }
}