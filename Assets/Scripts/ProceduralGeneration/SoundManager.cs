using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(AudioSource))]
public class SoundManager : MonoBehaviour 
{
    public AudioClip[] sfx;
    private static Dictionary<string, AudioClip> _sfx = new Dictionary<string, AudioClip>();

    public AudioClip[] musics;
    private static Dictionary<string, AudioClip> _musics = new Dictionary<string, AudioClip>();

    private static AudioSource _audioSource;

    public static bool mute = false;

    void Awake()
    {
        foreach (var v in sfx)
        {
            _sfx[v.name] = v;
        }

        foreach (var v in musics)
        {
            _musics[v.name] = v;
        }

        _audioSource = this.GetComponent<AudioSource>();
    }

    public static void PlaySound(string sfxName)
    {
        if (mute)
            return;
        _audioSource.PlayOneShot(_sfx[sfxName]);
    }

    public static void PlayMusic(string musicName)
    {
        if (mute)
            return;
        _audioSource.clip = _musics[musicName];
        _audioSource.Play();
    }
}
