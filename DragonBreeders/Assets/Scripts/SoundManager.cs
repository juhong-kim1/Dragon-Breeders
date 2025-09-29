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

    public AudioClip battleWinAudioClip;
    public AudioClip battleLoseAudioClip;

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

        PlayBGM(StartBGM);

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

