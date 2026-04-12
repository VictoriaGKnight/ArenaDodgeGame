using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    public AudioClip backgroundMusic;
    public AudioClip shootSound;
    public AudioClip damageSound;

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
        if (clip != null && sfxSource != null)
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
}