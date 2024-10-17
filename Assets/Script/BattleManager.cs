using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private SpriteAnimation vfxPrefab;
    
    void Start()
    {
        var vfxPool = Game.Pool.GetOrCreatPool("Vfx", () => new PrefabPool<SpriteAnimation>(vfxPrefab, true));
    }
}
