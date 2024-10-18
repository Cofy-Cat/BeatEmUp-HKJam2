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
    public static AudioManager instance;

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

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }

        DontDestroyOnLoad(gameObject);

        // Hooks up the 'OnSceneLoaded' method to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void PlaySoundFXClip(AudioClip clip, float volume)
    {
        SFXSource.PlayOneShot(clip);
        SFXSource.volume = volume;
    }

    // Called whenever a scene is loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode)
    {
        musicSource.Stop();

        // Play the corresponding music clip based on the scene
        switch (scene.buildIndex)
        {
            case 0:
                musicSource.clip = mainBGM;
                break;
            case 1:
                musicSource.clip = level1BGM;
                break;
            case 2:
                musicSource.clip = level2BGM;
                break;
            case 3:
                musicSource.clip = level3BGM;
                break;
        }

        musicSource.Play();
    }
}