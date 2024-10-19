using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIPanel : MonoBehaviour
{
    [SerializeField] private CharacterStatusListElement playerCardListElement;
    [SerializeField] private CharacterStatusListElement enemyCardListElement;
    [SerializeField] private Transform gameOverUI;
    [SerializeField] private Button gameOverButton;

    private List<ControllerRecord> playerStatus = new(1);

    private HashSet<ControllerRecord> enemiesStatusSet = new();
    private List<ControllerRecord> enemiesStatus = new();

    private PlayerController player;
    
    private void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        playerStatus.Add(player.ControllerRecord);
    }

    private void Start()
    {
        gameOverUI.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        player.onHealthChange += OnPlayerHealthChange;
        player.onAttack += OnPlayerAttack;
        player.onDead += OnDead;
        
        gameOverButton.onClick.AddListener(GoToHome);
    }

    private void OnDisable()
    {
        player.onHealthChange -= OnPlayerHealthChange;
        player.onAttack -= OnPlayerAttack;
        player.onDead -= OnDead;
        
        gameOverButton.onClick.RemoveListener(GoToHome);
    }

    private void OnPlayerHealthChange(HealthRecord _)
    {
        playerStatus[0] = player.ControllerRecord;
        playerCardListElement.SetData(playerStatus);
    }

    private void OnDead()
    {
        gameOverUI.gameObject.SetActive(true);
        AudioManager.Instance.StopBgm();
        AudioManager.Instance.PlaySoundFXClip(AudioManager.Instance.youDeerSound, 0.7f);
    }

    private void GoToHome()
    {
        Game.Gsm.GoToState(GameStateId.Home);
    }
    
    private void OnPlayerAttack(Controller controller)
    {
        var targetHealth = controller.ControllerRecord;
        
        if (controller.Health.current <= 0)
        {
            if (enemiesStatusSet.Contains(targetHealth))
            {
                enemiesStatusSet.Remove(targetHealth);
                enemiesStatus.Remove(targetHealth);
            }
        } else if (!enemiesStatusSet.Contains(targetHealth))
        {
            enemiesStatusSet.Add(targetHealth);
            enemiesStatus.Add(targetHealth);
        } 
        
        enemyCardListElement.SetData(enemiesStatus);
    }
}
