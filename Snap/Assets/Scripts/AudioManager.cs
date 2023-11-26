using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // Singleton instance

    public AudioSource audioSource; // Reference to the AudioSource component
    public AudioSource backgroundMusicSource; // Reference to a separate AudioSource component for background music

    // Volume property to control the sound volume
    private static float _soundVolume = 1.0f;
    public static float soundVolume
    {
        get { return _soundVolume; }
        set
        {
            _soundVolume = Mathf.Clamp01(value);
            Instance.audioSource.volume = _soundVolume;
            Instance.backgroundMusicSource.volume = _soundVolume;
        }
    }

    void Awake()
    {
        // Set up the singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Get the AudioSource components
        audioSource = GetComponent<AudioSource>();
        backgroundMusicSource = gameObject.AddComponent<AudioSource>();
        backgroundMusicSource.loop = true;
    }

    // Play a sound with the specified AudioClip
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip, soundVolume);
    }

    // Play background music with the specified AudioClip
    public void PlayBackgroundMusic(AudioClip clip)
    {
        backgroundMusicSource.clip = clip;
        backgroundMusicSource.Play();
    }

    // Stop playing background music
    public void StopBackgroundMusic()
    {
        backgroundMusicSource.Stop();
    }
}