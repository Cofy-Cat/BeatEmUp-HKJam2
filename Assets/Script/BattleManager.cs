using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private AudioClip bgm;
    
    void Start()
    {
        AudioManager.Instance.PlayBgm(bgm, 1f);
    }
}
