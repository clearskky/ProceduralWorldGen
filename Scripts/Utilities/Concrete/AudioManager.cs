using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public AudioSource sfxAudioSource, musicAudioSource;
    public AudioMixer sfxAudioMixer, musicAudioMixer;

    [Header("Player Clips")]
    public AudioClip miningMachineLoopClip, mineCompleteClip, mineFailClip;

    [Header("Vendor Clips")]
    public AudioClip sellInventoryClip, buyUpgradeClip, buyConsumableClip, refuelClip;

    [Header("UI Clips")]
    public AudioClip mainMenuMusicClip, gameplayMusicClip, invalidActionClip, defeatClip, buttonHitClip;

    // Singleton Implementation
    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        musicAudioMixer.SetFloat("musicVol", PlayerPrefs.GetFloat("musicVol"));
        sfxAudioMixer.SetFloat("sfxVol", PlayerPrefs.GetFloat("sfxVol"));

        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            musicAudioSource.clip = mainMenuMusicClip;
            musicAudioSource.Play();
            musicAudioSource.loop = true;
        }
        else
        {
            musicAudioSource.clip = gameplayMusicClip;
            musicAudioSource.Play();
            musicAudioSource.loop = true;

            sfxAudioSource.clip = miningMachineLoopClip;
            sfxAudioSource.Play();
            sfxAudioSource.loop = true;
        }
    }


    public void PlayInvalidActionClip()
    {
        sfxAudioSource.PlayOneShot(invalidActionClip);
    }

    public void PlayDefeatClip()
    {
        sfxAudioSource.PlayOneShot(defeatClip);
    }

    public void PlayButtonPressClip()
    {
        sfxAudioSource.PlayOneShot(buttonHitClip);
    }

    public void PlayMineFailClip()
    {
        sfxAudioSource.PlayOneShot(mineFailClip);
    }

    public void PlayMineCompleteClip()
    {
        sfxAudioSource.PlayOneShot(mineCompleteClip);
    }

}