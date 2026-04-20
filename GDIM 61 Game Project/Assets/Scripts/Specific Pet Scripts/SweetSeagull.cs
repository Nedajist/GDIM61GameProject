using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweetSeagull : Pet
{




    public override void FaceLeft()
    {
        sprite.flipX = false;
    }

    public override void FaceRight()
    {
        sprite.flipX = true;
    }

    public override void ReceiveDamage(int damage, Pet aggressor)
    {
        healthPoints -= 1;
        if (healthPoints <= 0)
        {
            WhoAndWhere();
            for (int i = 0; i < enemy_list.Count; i++)
            {
                enemy_list[i].GetComponent<Pet>().frozen_turns += 2;
            }
            Die();
        }
    }
    protected override string ReturnAbilityText()
    {
        abilityText = "Sweet Seagull - Frosting Spin: When this pet dies, all enemy pets are frozen for 2 turns";
        return abilityText;
    }

}
