using System;
using UnityEngine;

public class AudioManager : MonoInstance<AudioManager>
{
    public enum AudioType
    {
        MenuBGM,
        Level1BGM,
        Level2BGM,
        Level3BGM,
        Walk,
        Punch,
        Kick,
        FistSwing,
        FootSwing,
        ButtonClick,
        GameStart,
        GameOver,
        KnockDown,
        Explosion,
    }

    [Header("---------- Audio Source ----------")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;
    
    [Header("---------- Audio Clip   ----------")]
    public AudioClip background;
    public AudioClip walk;
    public AudioClip punch;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
        Debug.Log("AudioManager Started");
    }

    // Update is called once per frame
    public void PLaySoundFXClip(AudioClip clip, float volume)
    {
        SFXSource.PlayOneShot(clip);
        SFXSource.volume = volume;
    }
}
