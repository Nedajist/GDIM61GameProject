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

}
