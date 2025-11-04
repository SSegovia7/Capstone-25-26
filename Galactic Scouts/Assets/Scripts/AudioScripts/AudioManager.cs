using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class AudioManager
{
    public enum ThemeMusic
    {
        StageOne
    }

    public static void PlayMusic(ThemeMusic music)
    {
        GameObject musicGO = new GameObject("Music Theme");
        AudioSource audioSource = musicGO.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.clip = GetMusicAudioClip(music);
        audioSource.Play();
    }

    private static AudioClip GetMusicAudioClip(ThemeMusic music)
    {
        foreach (AudioAssetsHolder.MusicAudioClip audioClip in AudioAssetsHolder.i.MusicClips)
        {
            if (audioClip.music == music)
            {
                return audioClip.audio;
            }
        }
        Debug.LogWarning("No Audio Clip");
        return null;
    }

}
