using System;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private string nextLevel;

    private void OnTriggerEnter2D(Collider2D other)
    {
        GoToNextLevel(other.GetComponentInParent<PlayerController>());
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        GoToNextLevel(other.GetComponentInParent<PlayerController>());
    }

    private void GoToNextLevel(PlayerController player)
    {
        if (player != null && BattleManager.Instance.EnemyAllDead())
        {
            Game.Gsm.GoToState(GameStateId.Battle, new BattleState.Param()
            {
                playerHealth = player.Health,
                sceneName = nextLevel
            });
        }
    }
}
