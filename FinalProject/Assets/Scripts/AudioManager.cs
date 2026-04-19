using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    public AudioClip backgroundMusic;
    public AudioClip shootSound;
    public AudioClip damageSound;

    private const string MasterVolumeKey = "MasterVolume";
    private const string MusicVolumeKey = "MusicVolume";
    private const string SfxVolumeKey = "SfxVolume";

    private float masterVolume = 1f;

    void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        AudioSource[] sources = GetComponents<AudioSource>();

        if (musicSource == null && sources.Length > 0)
        {
            musicSource = sources[0];
        }

        if (sfxSource == null && sources.Length > 1)
        {
            sfxSource = sources[1];
        }

        LoadAudioSettings();
    }

    void Start()
    {
        PlayMusic(backgroundMusic);
    }

    void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.onHealthChanged += PlayDamageSound;
        }
    }

    void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.onHealthChanged -= PlayDamageSound;
        }
    }

    void PlayDamageSound(int newHealth)
    {
        PlaySoundEffect(damageSound);
    }

    public void PlayShootSound()
    {
        PlaySoundEffect(shootSound);
    }

    public void PlaySoundEffect(AudioClip clip)
    {
        if (clip != null && sfxSource != null && AudioListener.volume > 0f && sfxSource.volume > 0f)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip != null && musicSource != null)
        {
            if (musicSource.clip != clip)
            {
                musicSource.clip = clip;
            }

            musicSource.loop = true;

            if (!musicSource.isPlaying)
            {
                musicSource.Play();
            }
        }
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        AudioListener.volume = masterVolume;
        SaveAudioSettings();
    }

    public void SetMusicVolume(float volume)
    {
        if (musicSource != null)
        {
            musicSource.volume = Mathf.Clamp01(volume);
            SaveAudioSettings();
        }
    }

    public void SetSfxVolume(float volume)
    {
        if (sfxSource != null)
        {
            sfxSource.volume = Mathf.Clamp01(volume);
            SaveAudioSettings();
        }
    }

    public float GetMasterVolume()
    {
        return masterVolume;
    }

    public float GetMusicVolume()
    {
        return musicSource != null ? musicSource.volume : 0.3f;
    }

    public float GetSfxVolume()
    {
        return sfxSource != null ? sfxSource.volume : 0.8f;
    }

    public void ResetAudioSettings()
    {
        masterVolume = 1f;

        AudioListener.volume = 1f;

        if (musicSource != null)
        {
            musicSource.volume = 0.3f;
        }

        if (sfxSource != null)
        {
            sfxSource.volume = 0.8f;
        }

        SaveAudioSettings();
    }

    private void SaveAudioSettings()
    {
        PlayerPrefs.SetFloat(MasterVolumeKey, masterVolume);

        if (musicSource != null)
        {
            PlayerPrefs.SetFloat(MusicVolumeKey, musicSource.volume);
        }

        if (sfxSource != null)
        {
            PlayerPrefs.SetFloat(SfxVolumeKey, sfxSource.volume);
        }

        PlayerPrefs.Save();
    }

    private void LoadAudioSettings()
    {
        masterVolume = PlayerPrefs.GetFloat(MasterVolumeKey, 1f);
        float savedMusicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 0.3f);
        float savedSfxVolume = PlayerPrefs.GetFloat(SfxVolumeKey, 0.8f);

        AudioListener.volume = Mathf.Clamp01(masterVolume);

        if (musicSource != null)
        {
            musicSource.volume = Mathf.Clamp01(savedMusicVolume);
        }

        if (sfxSource != null)
        {
            sfxSource.volume = Mathf.Clamp01(savedSfxVolume);
        }
    }
}
