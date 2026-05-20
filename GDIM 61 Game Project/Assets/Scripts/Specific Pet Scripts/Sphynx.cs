using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.PlayerLoop;

public class Sphynx : Pet
{
    // Start is called before the first frame update
    [SerializeField] private TextMeshPro livesText;
    [SerializeField] private float fadeawayDuration = 0.5f;
    [SerializeField] private Collider2D _collider;
    private Vector2 minBounds = new Vector2(-5f, -5f);
    private Vector2 maxBounds = new Vector2(5f, 5f);
    private int remainingLives = 9;
    private bool phase2;
    private bool isDying;
    protected override void Start()
    {
        base.Start();
        phase2 = false;
        isDying = false;
        livesText.text = remainingLives.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        FadeAway();
    }

    public override void ReceiveDamage(float damage)
    {
        if (isDying == false)
        {
            healthPoints -= damage;
        }

        if (damage > 0)
        {
            StartCoroutine(FlashColor(0.1f, 0.1f, Color.red));
            transform.GetComponent<HealthBar>().StartCoroutine(transform.GetComponent<HealthBar>().TempSizeChange(0.15f, 0.15f, 0.5f));
        }

        if (healthPoints <= 0 && remainingLives > 0)
        {
            StartCoroutine(OnDeath(fadeawayDuration));
            livesText.text = remainingLives.ToString();

            if (remainingLives <= 5)
            {
                phase2 = true;
                Debug.Log(phase2);
            }
        }
        else if (remainingLives <= 0)
        {
            Die();
        }
    }
    
    protected override void DamageCheck(Pet other)
    {
        if (!phase2)
        {
            base.DamageCheck(other);
        }
        else
        {
            if (other.petSide != petSide && other.CanBeAttackedCheck())
            {
                other.ResetIFrames();
                if (attack > other.healthPoints)
                {
                    
                    //other.petSide == Side.ai;
                }
                other.ReceiveDamage(attack);
                GameController.instance.CullLists(teamList);
                AlertAlliesOfAttack();
            }
        }
    }

    IEnumerator OnDeath(float duration)
    {
        isDying = true;
        healthPoints = maxHealthPoints;
        _collider.enabled = false;
        float random = UnityEngine.Random.Range(minBounds.x, maxBounds.x);

        yield return new WaitForSeconds(duration);

        isDying = false;
        SetVelocityInRandomDirection();
        transform.position = new Vector2(random, random);
        remainingLives -= 1;
        _collider.enabled = true;
    }

    private void FadeAway()
    {
        Color currentColor = _sprite.color;
        if (isDying == true)
        {
            _rb.bodyType = RigidbodyType2D.Static;
            currentColor.a -= 0.1f;
            _sprite.color = currentColor;
        }
        else if (isDying == false)
        {
            _rb.bodyType = RigidbodyType2D.Dynamic;
            currentColor.a = 1f;
            _sprite.color = currentColor;
        }
    }  
}
