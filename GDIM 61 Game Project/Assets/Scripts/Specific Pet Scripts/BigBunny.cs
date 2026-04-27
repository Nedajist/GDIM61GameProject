using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class BigBunny : Pet
{
    // Start is called before the first frame update


    public override void FaceLeft()
    {
        _sprite.flipX = true;
    }

    public override void FaceRight()
    {
        _sprite.flipX = false;
    }

    public override void AllyDied()
    {


    }

    protected override string ReturnAbilityText()
    {
        _abilityText = "Big Bunny - Jump kick: when a team member dies, bunny immediately attacks the closest enemy";
        return _abilityText;
    }
}
