using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Sphynx : Pet
{
    // Start is called before the first frame update
    [SerializeField] private TextMeshPro livesText;
    private Vector3 pos; 
    private int remainingLives = 9;
    private bool phase2;
    private bool isDying;
    protected override void Start()
    {
        base.Start();
        pos = transform.position;
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
            StartCoroutine(OnDeath(0.5f));
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

        yield return new WaitForSeconds(duration);

        transform.position = pos;
        remainingLives -= 1;
        isDying = false;
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
        }
    }

    
}
