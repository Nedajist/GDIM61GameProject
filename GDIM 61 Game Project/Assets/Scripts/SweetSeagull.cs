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
}
