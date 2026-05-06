using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatCar : Pet
{
    public override void FaceLeft()
    {
        _sprite.flipX = true;
    }

    public override void FaceRight()
    {
        _sprite.flipX = false;
    }

    // Update is called once per frame
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        StartCoroutine(Freeze(1f));
    }

}
