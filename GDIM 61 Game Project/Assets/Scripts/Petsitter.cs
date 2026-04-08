using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Petsitter : Pet
{



    public override void FaceLeft()
    {
        sprite.flipX = false;
    }

    public override void FaceRight()
    {
        sprite.flipX = true;
    }
}
