using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [Header("Mixer Reference")]
    public AudioMixer audioMixer;

    [Header("UI Sliders")]
    public Slider masterSlider;
    public Slider soundFXSlider;
    public Slider musicSlider;

    private void Start()
    {
        // Load saved values or fallback to mixer values
        InitializeSlider(masterSlider, "MasterVolume", "MasterVol");
        InitializeSlider(soundFXSlider, "SoundFXVolume", "SoundFXVol");
        InitializeSlider(musicSlider, "MusicVolume", "MusicVol");
    }

    private void InitializeSlider(Slider slider, string mixerParam, string prefsKey)
    {
        float savedValue = PlayerPrefs.GetFloat(prefsKey, -1f); // will return -1 if the key doesn’t exist in PlayerPrefs yet

        if (savedValue >= 0) // Load saved value. If we get back -1, we know no value has been saved yet and jump to else if
        {
            slider.value = savedValue;
            SetVolume(mixerParam, savedValue);
        }
        else if (audioMixer.GetFloat(mixerParam, out float currentValue)) // Sync to mixer
        {
            slider.value = Mathf.Pow(10, currentValue / 20f);
        }
    }

    public void SetMasterVolume(float sliderValue)
    {
        SetVolume("MasterVolume", sliderValue);
        PlayerPrefs.SetFloat("MasterVol", sliderValue);
    }

    public void SetSoundFXVolume(float sliderValue)
    {
        SetVolume("SoundFXVolume", sliderValue);
        PlayerPrefs.SetFloat("SoundFXVol", sliderValue);
    }

    public void SetMusicVolume(float sliderValue)
    {
        SetVolume("MusicVolume", sliderValue);
        PlayerPrefs.SetFloat("MusicVol", sliderValue);
    }

    private void SetVolume(string mixerParam, float sliderValue)
    {
        // Adjust values to log10(0)
        float dB = Mathf.Log10(Mathf.Max(sliderValue, 0.0001f)) * 20f;
        audioMixer.SetFloat(mixerParam, dB);
    }

    private void OnDisable()
    {
        PlayerPrefs.Save(); // Persist settings
    }
}
