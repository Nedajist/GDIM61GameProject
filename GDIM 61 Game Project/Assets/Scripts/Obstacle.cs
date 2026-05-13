using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : Entity
{
    [SerializeField] public float knockbackForce;
    [SerializeField] public float damage;
    [SerializeField] private float _speed = 8;


    private float _maxHealthPoints;

    private void Start()
    {
        _maxHealthPoints = healthPoints;
    }


    public override void ReceiveDamage(float damage)
    {
        //damage sfx
        float previousHealthPoints = healthPoints;
        healthPoints -= damage;
        if (healthPoints <= 0)
        {
            StartCoroutine(FadeAway(0.5f));
        }


        if (damage > 0)
        {
            originalColor = new Color(originalColor.r, originalColor.g, originalColor.b, healthPoints / _maxHealthPoints);
            _sprite.color = originalColor;
            StartCoroutine(FlashColor(0.1f, 0.1f, Color.red));
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.GetComponent<Pet>() == null && collision.transform.GetComponent<Projectile>() == null)
        {             // bounces if not hitting pet/projectile

            Vector2 lineFromCollider = (Vector2)transform.position - collision.contacts[0].point;
            lineFromCollider = lineFromCollider.normalized;
            _rb.velocity = (lineFromCollider * _speed);
        }
    }

}
