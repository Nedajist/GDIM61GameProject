using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweetSeagull : Pet
{




    public override void FaceLeft()
    {
        _sprite.flipX = true;
    }

    public override void FaceRight()
    {
        _sprite.flipX = false;
    }

    public override void ReceiveDamage(int damage, Pet aggressor)
    {
        healthPoints -= 1;

        if (damage > 0)
        {
            StartCoroutine(FlashColor(0.1f, 0.1f, Color.red));
        }

        if (healthPoints <= 0)
        {
            GameController.instance.CullLists(enemyList);
            for (int i = 0; i < enemyList.Count; i++)
            {
                enemyList[i].GetComponent<Pet>().secondsFrozen += 2;
            }
            Die();
        }
    }
    protected override string ReturnAbilityText()
    {
        _abilityText = "Sweet Seagull - Frosting Spin: When this pet dies, all enemy pets are frozen for 2 turns";
        return _abilityText;
    }

}
