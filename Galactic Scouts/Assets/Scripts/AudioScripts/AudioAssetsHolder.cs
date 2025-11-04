using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioAssetsHolder : MonoBehaviour
{

    private static AudioAssetsHolder _i;

    //accesses the Resources folder and gets this component from the AudioAsset GameObject
    public static AudioAssetsHolder i
    {
        get
        {
            if (_i == null) _i = Instantiate(Resources.Load<AudioAssetsHolder>("AudioAssets"));
            return _i;
        }
    }


    public MusicAudioClip[] MusicClips;

    [Serializable]
    public class MusicAudioClip
    {
        public AudioManager.ThemeMusic music;
        public AudioClip audio;
    }
}
