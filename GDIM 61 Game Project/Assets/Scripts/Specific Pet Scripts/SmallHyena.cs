using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallHyena : Pet
{

    public override void FaceLeft()
    {
        _sprite.flipX = true;
    }

    public override void FaceRight()
    {
        _sprite.flipX = false;
    }

    

}
