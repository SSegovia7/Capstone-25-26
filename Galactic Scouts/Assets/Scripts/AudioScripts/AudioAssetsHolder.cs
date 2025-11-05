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


    public TrackAudioClip[] TrackClips;
    public GalaticScoutSoundAudioClip[] GalacticSoundClips;

    [Serializable]
    public class TrackAudioClip
    {
        public AudioManager.ThemeTrack track;
        public AudioClip audio;
    }

    [Serializable]
    public class GalaticScoutSoundAudioClip
    {
        public AudioManager.GalaticScoutSound sound;
        public AudioClip audio;
    }
}
