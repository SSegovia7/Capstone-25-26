using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance { get; private set; }
    private AudioSource _audioSource;
    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (!gameObject.GetComponent<AudioSource>()) gameObject.AddComponent<AudioSource>();
        
        _audioSource = gameObject.GetComponent<AudioSource>();
    }

    void Start()
    {
        StartTrack();
    }


    public void StartTrack()
    {
        StartCoroutine(AudioManager.PlayTrack(AudioManager.ThemeTrack.StageOne, _audioSource));
    }

    public void EndTrack()
    {
        //TO BE IMPLEMENTED
    }
}
