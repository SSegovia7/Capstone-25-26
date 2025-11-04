using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempAudioPlayer : MonoBehaviour
{
    void Start()
    {
        AudioManager.PlayMusic(AudioManager.ThemeMusic.StageOne);
    }
    
}
