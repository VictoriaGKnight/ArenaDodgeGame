using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsUI : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    void Start()
    {
        if (AudioManager.Instance != null)
        {
            if (musicSlider != null)
            {
                musicSlider.SetValueWithoutNotify(AudioManager.Instance.GetMusicVolume());
            }

            if (sfxSlider != null)
            {
                sfxSlider.SetValueWithoutNotify(AudioManager.Instance.GetSfxVolume());
            }
        }
    }

    public void OnMusicVolumeChanged(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetMusicVolume(value);
        }
    }

    public void OnSfxVolumeChanged(float value)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.SetSfxVolume(value);
        }
    }
}
