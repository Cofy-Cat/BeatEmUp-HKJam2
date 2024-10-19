using System;
using System.Collections.Generic;
using cfEngine.Logging;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private AudioClip bgm;
    [SerializeField] private List<Controller> enemies;

    void Start()
    {
        AudioManager.Instance.PlayBgm(bgm, 1f);
    }

    private void OnEnable()
    {
        foreach (var enemy in enemies)
        {
            enemy.onDead += EnemyOnDead;
        }
    }

    private void OnDisable()
    {
        foreach (var enemy in enemies)
        {
            enemy.onDead -= EnemyOnDead;
        }
    }
    
    private void EnemyOnDead()
    {
        Log.LogInfo("Enemy all dead");
    }
}
