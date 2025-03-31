using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!PlayerPrefs.HasKey("voiceVolume"))
        {
            PlayerPrefs.SetFloat("voiceVolume", 2);
            Load();
        }
        else
        {
            Load();
        }
    }

    // Update is called once per frame
    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        Save();
    }

    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("voiceVolume");
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("voiceVolume", volumeSlider.value);
    }
}
