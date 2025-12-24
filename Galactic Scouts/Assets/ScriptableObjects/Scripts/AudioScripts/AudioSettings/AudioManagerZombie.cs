using UnityEngine;

public class AudioManagerZombie : MonoBehaviour
{
    [SerializeField] private AudioClip Zombie_Steps1;
    private AudioSource audioSource;


    public void Play_SFX_Steps1()
    {
        float volume = 0.5f; // Custom volume for clip1 (Overridden by Audio Mixer)
        PlayClip(Zombie_Steps1, volume);
    }


    private void PlayClip(AudioClip clip, float volume)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip, volume);
        }
    }
}
