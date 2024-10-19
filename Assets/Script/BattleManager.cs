using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoInstance<BattleManager>
{
    [SerializeField] private AudioClip bgm;
    [SerializeField] private List<Controller> enemies;
    [SerializeField] private Portal nextLevelPortal;

    protected override void Awake()
    {
        base.Awake();
        nextLevelPortal = FindFirstObjectByType<Portal>();
    }

    void Start()
    {
        nextLevelPortal.gameObject.SetActive(false);
        AudioManager.Instance.PlayBgm(bgm, 0.4f);
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
        if (EnemyAllDead())
        {
            nextLevelPortal.gameObject.SetActive(true);
        }
    }

    public bool EnemyAllDead()
    {
        return enemies.All(e => e.isDead);
    }
}
