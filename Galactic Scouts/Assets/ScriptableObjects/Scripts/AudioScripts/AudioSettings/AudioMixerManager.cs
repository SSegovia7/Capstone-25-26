using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerManager : MonoBehaviour

{
    [SerializeField] private AudioMixer m_Mixer;
    public void SetVolumeMaster(float level) 
    {
        m_Mixer.SetFloat("MasterVolume", Mathf.Log10(level) * 20f);
    }

    public void SetVolumeSoundFX(float level) 
    {
        m_Mixer.SetFloat("SoundFXVolume", Mathf.Log10(level) * 20f);
    }

    public void SetVolumeMusic(float level) 
    {
        m_Mixer.SetFloat("MusicVolume", Mathf.Log10(level) * 20f);
    }
}
