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

    public enum Sound
    {
        GS_Shooting,
        GS_Dash,
        GS_BoxCollect,
        Enemy_TakeDamage
    }

    private enum MusicTrackStage{beginning, main, end}

    private static ThemeTrack _currentTrack;

    public static IEnumerator PlayTrack(ThemeTrack track, AudioSource audioSource)
    {
        AudioClip intro = GetTrackAudioClip(track, MusicTrackStage.beginning);
        AudioClip main = GetTrackAudioClip(track, MusicTrackStage.main);

        audioSource.clip = intro;
        audioSource.Play();

        yield return new WaitForSeconds(intro.length - 0.2f); //-0.2f removes a small audio gap when transistioning between 2 tracks

        audioSource.clip = main;
        audioSource.loop = true;
        audioSource.Play();
    }

    public static void PlaySound(Sound sound, float timeUntilDestroy = 3.0f)
    {
        float volume;
        AudioClip audioClip = GetSoundAudioClip(sound, out volume);

        GameObject soundGO = new GameObject("Sound");
        AudioSource audioSource = soundGO.AddComponent<AudioSource>();
        audioSource.volume = volume;
        audioSource.PlayOneShot(audioClip);

        UnityEngine.Object.Destroy(soundGO, timeUntilDestroy);
    }

    private static AudioClip GetTrackAudioClip(ThemeTrack track, MusicTrackStage currentStage)
    {
        foreach (AudioAssetsHolder.TrackAudioClip audioClip in AudioAssetsHolder.i.TrackClips)
        {
            if (audioClip.track == track)
            {
                switch (currentStage)
                {
                    case MusicTrackStage.beginning:
                        return audioClip.beginning;
                    case MusicTrackStage.main:
                        return audioClip.loop;
                    case MusicTrackStage.end:
                        return audioClip.end;
                }
                return audioClip.beginning;
            }
        }
        Debug.LogWarning("No Audio Clip Detected");
        return null;
    }

    private static AudioClip GetSoundAudioClip(Sound sound, out float volume)
    {
        float minVolume = 0.0f;
        float maxVolume = 1.0f;
        volume = 1.0f;

        foreach (AudioAssetsHolder.SoundAudioClip audioClip in AudioAssetsHolder.i.GalacticSoundClips)
        {
            if (audioClip.sound == sound)
            {
                volume = Math.Clamp(audioClip.volume, minVolume, maxVolume);
                return audioClip.audio;
            }
        }
        Debug.LogWarning("No Audio Clip");
        return null;
    }

}
