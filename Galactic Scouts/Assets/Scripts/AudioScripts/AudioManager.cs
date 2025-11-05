using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

public static class AudioManager
{
    public enum ThemeTrack
    {
        StageOne
    }

    public enum GalaticScoutSound
    {
        Shooting,
        TakeDamage
    }

    public static void PlayTrack(ThemeTrack track)
    {
        GameObject trackGO = new GameObject("Track Theme");
        AudioSource audioSource = trackGO.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.clip = GetTrackAudioClip(track);
        audioSource.Play();
    }

    public static void PlaySound(GalaticScoutSound sound)
    {
        GameObject soundGO = new GameObject("GS_Sound");
        AudioSource audioSource = soundGO.AddComponent<AudioSource>(); ;
        audioSource.PlayOneShot(GetSoundAudioClip(sound));
        float timeUntilDestroy = 5.0f;
        UnityEngine.Object.Destroy(soundGO, timeUntilDestroy);
    }

    private static AudioClip GetTrackAudioClip(ThemeTrack track)
    {
        foreach (AudioAssetsHolder.TrackAudioClip audioClip in AudioAssetsHolder.i.TrackClips)
        {
            if (audioClip.track == track)
            {
                return audioClip.audio;
            }
        }
        Debug.LogWarning("No Audio Clip");
        return null;
    }

    private static AudioClip GetSoundAudioClip(GalaticScoutSound sound)
    {
        foreach (AudioAssetsHolder.GalaticScoutSoundAudioClip audioClip in AudioAssetsHolder.i.GalacticSoundClips)
        {
            if (audioClip.sound == sound)
            {
                return audioClip.audio;
            }
        }
        Debug.LogWarning("No Audio Clip");
        return null;
    }

}
