using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeRats : Pet
{


    public override void ReceiveDamage(int damage)
    {
        healthPoints -= 1;
        if (healthPoints <= 0)
        {
            Die();
        }
    }

    public override void FaceLeft()
    {
        sprite.flipX = false;
    }

    public override void FaceRight()
    {
        sprite.flipX = true;
    }
}
