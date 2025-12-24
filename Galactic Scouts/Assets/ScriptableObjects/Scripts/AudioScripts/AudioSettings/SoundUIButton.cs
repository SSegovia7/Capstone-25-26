using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SoundUIButton : MonoBehaviour
{
    public AudioClip clickSound;
    public float volume = 1f;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(PlayClickSound);
    }

    void PlayClickSound()
    {
        // Play sound using AudioManagerExternal audio source
        if (AudioManagerExternal.Instance != null)
            AudioManagerExternal.Instance.PlaySound(clickSound, volume);
    }
}
