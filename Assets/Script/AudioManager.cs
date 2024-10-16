using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;


    [Header("---------- Audio Source ----------")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;
    
    [Header("---------- Audio Clip   ----------")]
    public AudioClip background;
    public AudioClip walk;
    public AudioClip punch1;
    public AudioClip punch2;

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
