using UnityEngine;
using UnityEngine.Audio;

public class GameAudioBootstrap : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    private void Awake()
    {
        AudioManager.Initialize(audioMixer);
    }
}
