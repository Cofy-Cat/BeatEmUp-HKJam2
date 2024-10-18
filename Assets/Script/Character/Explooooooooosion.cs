using System;
using System.Collections.Generic;
using UnityEngine;

public class Explooooooooosion: MonoBehaviour
{
    [SerializeField] private string explosionSpriteVfxName;
    [SerializeField] private AudioClip explosionSound;

    [SerializeField] private Vector2 force = new Vector2(3, 6);
    [SerializeField] private float distance = 2f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float stunDuration = 2f;
    [SerializeField] private Collider2D detectCollider;
    [SerializeField] private Collider2D explosionCollider;

    private void Start()
    {
        detectCollider.enabled = true;
        explosionCollider.enabled = false;
    }

    public void Explosiooon()
    {
        detectCollider.enabled = false;
        explosionCollider.enabled = true;

        if (explosionSound != null)
        {
            AudioManager.instance.PlaySoundFXClip(explosionSound, 1f);
        }

        if (!string.IsNullOrEmpty(explosionSpriteVfxName) && Game.Pool.TryGetPool("Vfx", out var pool) && pool is PrefabPool<SpriteAnimation> animPool)
        {
            var anim = animPool.Get();
            anim.gameObject.SetActive(true);
            anim.transform.position = transform.position;
            anim.transform.localScale = explosionCollider.bounds.size;
            anim.Play(explosionSpriteVfxName, onAnimationEnd:() =>
            {
                NextFrame.Instance.Execute(() => Destroy(this.gameObject));
                animPool.Release(anim);
            });
        }
        
        List<Collider2D> overlaps = new();
        explosionCollider.Overlap(overlaps);
        for (var i = 0; i < overlaps.Count; i++)
        {
            var controller = overlaps[i].GetComponentInParent<Controller>();
            if (controller != null)
            {
                controller.Hurt(damage);
                var offsetX = Mathf.Sign(controller.transform.position.x - transform.position.x);
                controller.Command.ExecuteCommand(
                    new KnockBackCommand(
                        new Vector2(offsetX * force.x, force.y),
                        distance,
                        10f,
                        stunDuration
                    ));
            }
        }
    }
}