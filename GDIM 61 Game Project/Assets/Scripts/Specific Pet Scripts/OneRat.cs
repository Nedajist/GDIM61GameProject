using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneRat : Pet
{



    public override void FaceLeft()
    {
        _sprite.flipX = false;
    }

    public override void FaceRight()
    {
        _sprite.flipX = true;
    }

 

}
