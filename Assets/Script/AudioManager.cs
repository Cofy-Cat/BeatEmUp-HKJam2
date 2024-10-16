using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public enum AudioType
    {
        Background,
        Walk,
        Punch,
        KnockDown,
        GameStart,
        GameOver,
        Explosion,
    }

    [Header("---------- Audio Source ----------")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;
    
    [Header("---------- Audio Clip   ----------")]
    public AudioClip background;
    public AudioClip walk;
    public AudioClip punch;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
        Debug.Log("AudioManager Started");
    }

    // Update is called once per frame
    public void PLaySoundFXClip(AudioClip clip, Transform spawnTrasnform, float volumne)
    {
        SFXSource.PlayOneShot(clip);
    }
}
