using UnityEngine;

public class SoundCollectible : MonoBehaviour
{
    [Header("Pickup Settings")]
    public string targetTag = "TargetTag"; // Tag to detect
    public AudioClip pickupSound;
    public float volume = 1f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            // Play sound using AudioManagerExternal audio source
            if (AudioManagerExternal.Instance != null)
                AudioManagerExternal.Instance.PlaySound(pickupSound, volume);
        }
    }
}
