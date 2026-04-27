using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Petsitter : Pet
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
        _abilityText = "Teddy Bear - Nourishment: At the end of your turn, the adjacent allied pet with the least HP gains 1 HP";
        return _abilityText;
    }


}
