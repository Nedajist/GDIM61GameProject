using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Sphynx : Pet
{
    // Start is called before the first frame update
    private Vector3 pos; 
    private int remainingLives = 9;
    protected override void Start()
    {
        base.Start();
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ReceiveDamage(float damage)
    {
        healthPoints -= damage;

        if (damage > 0)
        {
            StartCoroutine(FlashColor(0.1f, 0.1f, Color.red));
            transform.GetComponent<HealthBar>().StartCoroutine(transform.GetComponent<HealthBar>().TempSizeChange(0.15f, 0.15f, 0.5f));
        }

        if (healthPoints <= 0 && remainingLives > 0)
        {
            transform.position = pos;
            healthPoints = maxHealthPoints;
            remainingLives -= 1;
        }
        else if (healthPoints <= 0 && remainingLives <= 0)
        {
            Die();
        }
    }
    /*
    protected override void DamageCheck(Pet other)
    {
        if (other.petSide != petSide && other.CanBeAttackedCheck())
        {
            other.ResetIFrames();
            other.ReceiveDamage(attack);
            GameController.instance.CullLists(teamList);
            AlertAlliesOfAttack();
        }
    }
    */
}
