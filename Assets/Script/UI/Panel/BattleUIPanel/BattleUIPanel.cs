using System.Collections.Generic;
using UnityEngine;

public class BattleUIPanel : MonoBehaviour
{
    [SerializeField] private CharacterStatusListElement playerCardListElement;
    [SerializeField] private CharacterStatusListElement enemyCardListElement;

    private List<HealthRecord> playerStatus = new(1);

    private HashSet<HealthRecord> enemiesStatusSet = new();
    private List<HealthRecord> enemiesStatus = new();

    private PlayerController player;
    
    private void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        playerStatus.Add(player.Health);
    }

    private void OnEnable()
    {
        player.onHealthChange += OnPlayerHealthChange;
        player.onAttack += OnPlayerAttack;
    }

    private void OnDisable()
    {
        player.onHealthChange -= OnPlayerHealthChange;
        player.onAttack -= OnPlayerAttack;
    }

    private void OnPlayerHealthChange(HealthRecord playerHealth)
    {
        playerStatus[0] = playerHealth;
        playerCardListElement.SetData(playerStatus);
    }
    
    private void OnPlayerAttack(Controller controller)
    {
        var targetHealth = controller.Health;
        
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
