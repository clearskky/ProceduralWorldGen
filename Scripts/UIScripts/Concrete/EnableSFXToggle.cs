using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class EnableSFXToggle : MonoBehaviour
{
    public AudioMixer sfxMixer;

    private Toggle toggle;
    void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(delegate {
            OnToggleClick();
        });

        if (PlayerPrefs.GetFloat("sfxVol") <= -80.0f)
        {
            toggle.isOn = true;
        }
        else
        {
            toggle.isOn = false;
        }
    }

    public void OnToggleClick()
    {
        if (!toggle.isOn)
        {
            sfxMixer.SetFloat("sfxVol", -80.0f);
            PlayerPrefs.SetFloat("sfxVol", -80.0f);
        }
        else
        {
            sfxMixer.SetFloat("sfxVol", 0.0f);
            PlayerPrefs.SetFloat("sfxVol", 0.0f);
        }
        PlayerPrefs.Save();
    }
}
