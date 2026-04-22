using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaltySeagull : Pet
{

    public override void FaceLeft()
    {
        sprite.flipX = true;
    }

    public override void FaceRight()
    {
        sprite.flipX = false;
    }

    public override void AllyPetEnterAttack()
    {
        attack += 1;
    }

    protected override string ReturnAbilityText()
    {
        abilityText = "Wind dance - + 1 ATK after an ally attacks";
        return abilityText;
    }

}
