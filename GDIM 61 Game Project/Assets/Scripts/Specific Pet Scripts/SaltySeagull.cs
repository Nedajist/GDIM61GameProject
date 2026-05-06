using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaltySeagull : Pet
{
    [SerializeField] float attackBoost = 0.3f;
    [SerializeField] float speedBoost = 0.1f;

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
        _abilityText = "Salty Seagull - Wind dance: +0.3 ATK and +0.1 speed after an ally attacks";
        return _abilityText;
    }

    public override void AllyAttacked()
    {
        StartCoroutine(FlashColor(0.1f, 0.1f, Color.yellow));
        speed += speedBoost;
        attack += attackBoost;

    }

}
