using System;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using UnityEngine;

public class Ray : MonoBehaviour
{
    [SerializeField] private float damage = 1f;
    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponentInParent<PlayerController>();
        if (player == null)
            return;

        player.Hurt(damage);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        var player = other.GetComponentInParent<PlayerController>();
        if (player == null)
            return;

        player.Hurt(damage);
    }
}
