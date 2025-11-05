using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempAudioPlayer : MonoBehaviour
{
    void Start()
    {
        AudioManager.PlayTrack(AudioManager.ThemeTrack.StageOne);
    }
    
}
