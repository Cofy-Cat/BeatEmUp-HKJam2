using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;
    [SerializeField] private AudioSource soundFXObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Update is called once per frame
    public void PLaySoundFXClip(AudioClip audiocClip, Transform spawnTrasnform, float volumne)
    {
        AudioSource audioSource = Instantiate(soundFXObject, spawnTrasnform.position, Quaternion.identity);
        audioSource.clip = audiocClip;
        audioSource.volume = volumne;
        audioSource.Play();
        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }

}
