using System;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float damage = 5f;
    [SerializeField] private float lifeTime = 10f;
    [SerializeField] public float direction = -1f;
    [SerializeField] private float attackKnockbackForce = 2.5f;
    [SerializeField] private float yDirection = 0f;
    private Vector2 velocity;
    private Rigidbody2D rb;
    private float startTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Bullet(float direction)
    {
        this.direction = direction;
    }

    void Start()
    {
        startTime = Time.time;
        lifeTime += startTime;
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if (direction < 0) direction = -1;
        else if (direction > 0) direction = 1;

        velocity = new Vector2(direction * speed, yDirection);
        rb.linearVelocity = velocity;
        if (Time.time > lifeTime) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponentInParent<PlayerController>();
        if (player == null)
            return;

        player.Hurt(damage);
        gameObject.SetActive(false);
    }
}
