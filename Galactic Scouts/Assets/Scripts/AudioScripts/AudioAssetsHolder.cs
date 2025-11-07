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
    public SoundAudioClip[] GalacticSoundClips;

    [Serializable]
    public class TrackAudioClip
    {
        public AudioManager.ThemeTrack track;
        public AudioClip beginning;
        public AudioClip loop;
        public AudioClip end;
    }

    [Serializable]
    public class SoundAudioClip
    {
        public AudioManager.Sound sound;
        public AudioClip audio;
        public float volume = 1.0f;
    }
}
