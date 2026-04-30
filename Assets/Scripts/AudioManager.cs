using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource sfxSource;
    public AudioSource musicSource;

    [Header("Music")]
    public AudioClip backgroundMusic;

    [Header("UI")]
    public Image muteButtonImage;
    public Color normalColor = Color.white;
    public Color mutedColor = new Color(0.6f, 0.6f, 0.6f);

    private bool isMuted = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        PlayMusic(backgroundMusic);
        UpdateMuteVisual();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null && !isMuted)
        {
            sfxSource.pitch = Random.Range(0.9f, 1.1f);
            sfxSource.PlayOneShot(clip);
            sfxSource.pitch = 1f;
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip != null && musicSource != null)
        {
            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;

        AudioListener.volume = isMuted ? 0f : 1f;

        UpdateMuteVisual();
    }

    void UpdateMuteVisual()
    {
        if (muteButtonImage != null)
        {
            muteButtonImage.color = isMuted ? mutedColor : normalColor;
        }
    }
}