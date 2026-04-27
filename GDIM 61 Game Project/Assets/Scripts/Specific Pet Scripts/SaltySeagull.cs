using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaltySeagull : Pet
{

    public override void FaceLeft()
    {
        _sprite.flipX = true;
    }

    public override void FaceRight()
    {
        _sprite.flipX = false;
    }


    protected override string ReturnAbilityText()
    {
        _abilityText = "Wind dance - + 1 ATK after an ally attacks";
        return _abilityText;
    }

}
