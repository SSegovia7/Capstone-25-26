using UnityEngine;

public class AudioManagerPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip SFX_Steps1;
    [SerializeField] private AudioClip clip2;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play_SFX_Steps1()
    {
         float volume = 0.5f; // Custom volume for clip1 (Overridden by Audio Mixer)
        PlayClip(SFX_Steps1, volume);
    }

    public void PlayClip2()
    {
         float volume = 1f; 
        PlayClip(clip2, volume);
    }

    private void PlayClip(AudioClip clip, float volume)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip, volume);
        }
    }
}