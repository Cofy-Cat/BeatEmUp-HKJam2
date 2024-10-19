using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoInstance<AudioManager>
{
    public enum AudioType
    {
        Explosion,
        Fall,
        HitFinisher,
        HitLarge1,
        HitLarge2,
        HitLarge3,
        HitSmall1,
        HitSmall2,
        KickSwing,
        Level1BGM,
        Level2BGM,
        Level3BGM,
        MainBGM,
        PunchSwing,
        VictoryMale,
        Walk1,
        Walk2,
        WoodBatFinisher,
        WoodBatSwing
    }

    // Singleton to keep instance alive through all scenes
    [Header("---------- Audio Source ----------")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    [Header("---------- Audio Clip   ----------")]
    public AudioClip explosion;
    public AudioClip fall;
    public AudioClip hitFinisher;
    public AudioClip hitLarge1;
    public AudioClip hitLarge2;
    public AudioClip hitLarge3;
    public AudioClip hitSmall1;
    public AudioClip hitSmall2;
    public AudioClip kickSwing;
    public AudioClip level1BGM;
    public AudioClip level2BGM;
    public AudioClip level3BGM;
    public AudioClip mainBGM;
    public AudioClip punchSwing;
    public AudioClip victoryMale;
    public AudioClip walk1;
    public AudioClip walk2;
    public AudioClip woodBatFinisher;
    public AudioClip woodBatSwing;

    public void PlaySoundFXClip(AudioClip clip, float volume)
    {
        SFXSource.PlayOneShot(clip);
        SFXSource.volume = volume;
    }

    public void PlayBgm(AudioClip clip, float volume)
    {
        musicSource.PlayOneShot(clip);
        musicSource.volume = volume;
    }
}