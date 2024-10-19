using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomeUIPanel : MonoBehaviour
{
    [SerializeField] private Button startButton;
        
    [SerializeField] private string nextScene;

    private void Start()
    {
        AudioManager.Instance.PlaySoundFXClip(AudioManager.Instance.mainBGM, 0.6f);
    }

    private void OnEnable()
    {
        startButton.onClick.AddListener(OnStartClicked);
    }

    private void OnDisable()
    {
        startButton.onClick.RemoveListener(OnStartClicked);
    }

    private void OnStartClicked()
    {
        StartCoroutine(StartSequenceRoutine());
    }

    private IEnumerator StartSequenceRoutine()
    {
        yield return new WaitForSeconds(0f);

        Game.Gsm.GoToState(GameStateId.Battle, new BattleState.Param()
        {
            sceneName = nextScene,
            playerHealth = null
        });
    }
}
