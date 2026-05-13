using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NorthernNarwhal : Pet
{
    private bool isCharging = false;
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
        Spearhead(isCharging);
    }
    void Spearhead(bool charge)
    {
        if (charge == false)
        {
            speed *= 10f;
            isCharging = true;
        }
        else
        {
            speed /= 10f;
            isCharging = false;
        }
    }
}
