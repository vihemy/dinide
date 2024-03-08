using System.Collections;
using UnityEngine;
using System;

public class AudioManager : Singleton<AudioManager>
{
    public Sound[] sounds;

    new void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.panStereo = s.pan;
        }
    }

    private void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }

    private void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }

    private void PlayOneShot(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.PlayOneShot(s.clip);
    }

    private void SetVolume(string name, float volume)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.volume = volume;
    }

    private float GetVolume(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        return s.source.volume;
    }
    private void FadeOutAndStop(string name, float fadeTime)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        StartCoroutine(FadeOutAndStopCoroutine(s.source, fadeTime));
    }

    private void FadeInAndPlay(string name, float fadeTime)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        StartCoroutine(FadeInAndStartCoroutine(s.source, fadeTime));
    }

    private IEnumerator FadeOutAndStopCoroutine(AudioSource audioSource, float fadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    private IEnumerator FadeInAndStartCoroutine(AudioSource audioSource, float fadeTime)
    {
        float initialVolume = audioSource.volume;
        audioSource.volume = 0;
        audioSource.Play();

        while (audioSource.volume < initialVolume)
        {
            audioSource.volume += initialVolume * Time.deltaTime / fadeTime;
            yield return null;
        }

        audioSource.volume = initialVolume;
    }

    private IEnumerator CrossFadeCoroutine(AudioSource sourceA, AudioSource sourceB, float fadeTime)
    {
        float startVolumeA = sourceA.volume;
        float startVolumeB = sourceB.volume;

        while (sourceA.volume > 0)
        {
            sourceA.volume -= startVolumeA * Time.deltaTime / fadeTime;
            sourceB.volume += startVolumeB * Time.deltaTime / fadeTime;
            yield return null;
        }

        sourceA.Stop();
        sourceA.volume = startVolumeA;
        sourceB.volume = startVolumeB;
    }
}


[System.Serializable]
public class Sound

{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 1;

    [Range(-10f, 10f)]
    public float pitch = 1;

    public bool loop;

    [HideInInspector]
    public AudioSource source;

    [Range(-1f, 1f)]
    public float pan = 0;
}
