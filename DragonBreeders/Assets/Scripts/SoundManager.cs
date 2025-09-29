using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource bgmSource;
    public AudioSource sfxSource;

    public AudioClip uiClickSource;
    public Canvas parentCanvas;

    public AudioClip StartBGM;
    public AudioClip mainBGM1;
    public AudioClip mainBGM2;
    public AudioClip BattleBGM;

    public AudioClip bathAudioClip;
    public AudioClip dragonScreamAudioClip;

    public AudioClip eatAudioClip;
    public AudioClip playAudioClip;
    public AudioClip errorAudioClip;
    public AudioClip successAudioClip;
    public AudioClip restAudioClip;
    public AudioClip uiClickItem;
    public AudioClip uiClickTraining;
    public AudioClip uiClickBack;

    public AudioClip battleWinAudioClip;
    public AudioClip battleLoseAudioClip;
    public AudioClip dragonAttackAudioClip;
    public AudioClip dragonSkillAudioClip;
    public AudioClip mosnterAttackAudioClip;
    public AudioClip mosnterSkillAudioClip;

    public Slider volumeSliderStart;
    public float masterVolume = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
    }

    private void Start()
    {
        Button[] buttons = parentCanvas.GetComponentsInChildren<Button>();

        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlaySFX(uiClickSource);
            });
        }

        if (volumeSliderStart != null)
        {
            volumeSliderStart.minValue = 0f;
            volumeSliderStart.maxValue = 1f;
            volumeSliderStart.value = masterVolume;
            volumeSliderStart.onValueChanged.AddListener(SetMasterVolume);
        }

        ApplyVolume();

        PlayBGM(StartBGM);

    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        ApplyVolume();

        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.Save();
    }

    private void ApplyVolume()
    {
        AudioListener.volume = masterVolume;
    }

    public void PlayBGM(AudioClip clip)
    {
        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void SetBGMVolume(float volume)
    {
        bgmSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }

    public void RandomMainBGMPlay()
    {
        int random = Random.Range(1,3);

        switch (random)
        {
            case 1:
                PlayBGM(mainBGM1);
                break;
            case 2:
                PlayBGM(mainBGM2);
                break;
        }
    }

    public void PlayErrorSound()
    {
        PlaySFX(errorAudioClip);
    }
}

