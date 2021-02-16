using Pixelplacement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [Header("References")]
    [SerializeField]
    private AudioSource vendorMusicTheme = null;
    [SerializeField]
    private AudioSource lootAreaMusicTheme = null;
    [SerializeField]
    private AudioSource spikesSFX = null;
    [SerializeField]
    private AudioSource barbarianDetectPlayerSFX = null;
    [SerializeField]
    private AudioSource barbarianChargeSFX = null;
    [SerializeField]
    private AudioSource barbarianImpactSFX = null;
    [SerializeField]
    private AudioSource slimeDetectPlayerSFX = null;
    [SerializeField]
    private AudioSource slimeChargeSFX = null;
    [SerializeField]
    private AudioSource slimeImpactSFX = null;
    [SerializeField]
    private AudioSource giveLootSFX = null;
    [SerializeField]
    private AudioSource takeLootSFX = null;
    [Header("Variables")]
    [SerializeField]
    private float timeToFadeClip = 1f;
    [SerializeField]
    [Range(0f,1f)]
    private float musicVolume = 0.5f;
    [SerializeField]
    [Range(0f, 1f)]
    private float sfxVolume = 0.5f;
    [SerializeField]
    [Range(0f, 1f)]
    private float globalVolume = 0.5f;
    //helpers
    private AudioSource musicToFadeOut = null;
    private AudioSource musicToFadeIn = null;
    private float maxMusicVolume = 0f;
    private void OnDestroy()
    {
        _instance = null;   
    }
    private void Awake()
    {
        maxMusicVolume = musicVolume * globalVolume;
        vendorMusicTheme.volume = maxMusicVolume;
        lootAreaMusicTheme.volume = maxMusicVolume;
        AudioListener.volume = sfxVolume * globalVolume;
        musicToFadeIn = vendorMusicTheme;
        musicToFadeIn.gameObject.SetActive(true);
        StartCoroutine(FadeInMusic());
    }

    public void FadeInVendorMusic()
    {
        musicToFadeIn = vendorMusicTheme;
        musicToFadeOut = lootAreaMusicTheme;
        musicToFadeIn.gameObject.SetActive(true);
        FadeMusic();
    }
    public void FadeInLootAreaMusic()
    {
        musicToFadeIn = lootAreaMusicTheme;
        musicToFadeOut = vendorMusicTheme;
        musicToFadeIn.gameObject.SetActive(true);
        FadeMusic();
    }
    private void FadeMusic()
    {
        StartCoroutine(FadeInMusic());
        StartCoroutine(FadeOutMusic());
    }
    public void PlaySpikeSFX()
    {
        PlayOneShot(spikesSFX);
    }
    public void PlayBarbDetectSFX()
    {
        PlayOneShot(barbarianDetectPlayerSFX);
    }
    public void PlayBarbChargeSFX()
    {
        PlayOneShot(barbarianChargeSFX);
    }
    public void PlayBarbImpactSFX()
    {
        PlayOneShot(barbarianImpactSFX);
    }
    public void PlaySlimeImpactSFX()
    {
        PlayOneShot(slimeImpactSFX);
    }
    public void PlaySlimeDetectionSFX()
    {
        PlayOneShot(slimeDetectPlayerSFX);
    }
    public void PlaySlimeChargeSFX()
    {
        PlayOneShot(slimeChargeSFX);
    }
    public void PlayGiveLootSFX()
    {
        PlayOneShot(giveLootSFX);
    }
    public void PlayTakeLootSFX()
    {
        PlayOneShot(takeLootSFX);
    }

    public void ChangeMusicVolume(float value)
    {
        musicVolume = value;
        vendorMusicTheme.volume = musicVolume * globalVolume;
        lootAreaMusicTheme.volume = musicVolume * globalVolume;
    }
    public void ChangeSFXVolume(float value)
    {
        sfxVolume = value;
        AudioListener.volume = globalVolume;
    }

    public void ChangeGlobalVolume(float value)
    {
        globalVolume = value;
        ChangeMusicVolume(musicVolume);
        ChangeSFXVolume(sfxVolume);
    }

    private void PlayOneShot(AudioSource audioSource)
    {
        audioSource.PlayOneShot(audioSource.clip);
    }
    IEnumerator FadeInMusic(float value = 0f)
    {
        if(value/timeToFadeClip < 1f)
        {
            value += Time.deltaTime;
            musicToFadeIn.volume = Mathf.Lerp(0f, maxMusicVolume,value/timeToFadeClip);
            yield return null;
            StartCoroutine(FadeInMusic(value));
        }
    }
    IEnumerator FadeOutMusic(float value = 0f)
    {
        if (value / timeToFadeClip < 1f)
        {
            value += Time.deltaTime;
            musicToFadeOut.volume = Mathf.Lerp(maxMusicVolume, 0f, value / timeToFadeClip);
            yield return null;
            StartCoroutine(FadeOutMusic(value));
        }
        else
        {
            musicToFadeOut.gameObject.SetActive(false);
        }
    }
}
