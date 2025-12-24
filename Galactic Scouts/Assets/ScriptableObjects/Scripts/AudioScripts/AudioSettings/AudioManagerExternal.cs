using UnityEngine;
using UnityEngine.Audio; // Needed for AudioMixerGroup

public class AudioManagerExternal : MonoBehaviour
{
    public static AudioManagerExternal Instance;
    private AudioSource audioSource;

    [Header("Audio Mixer")]
    public AudioMixerGroup outputMixerGroup; // assign Audio Mixer in Inspector

    void Awake()
    {
        if (Instance == null) // It will survive scene changes and won’t be disabled with the UI
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = gameObject.AddComponent<AudioSource>();
            
            if (outputMixerGroup != null) // Assign mixer group if specified
                audioSource.outputAudioMixerGroup = outputMixerGroup;
        }
        else
        {
            Destroy(gameObject); // Only will exist 1 manager, any copy when swapping scenes will be destroyed
        }
    }

    public void PlaySound(AudioClip clip, float volume = 1f)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip, volume);
    }
}
