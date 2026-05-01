using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : Pet
{
    [SerializeField] private float ravenousMultiplier = 1.5f;
    public override void FaceLeft()
    {
        _sprite.flipX = false;
    }
    public override void FaceRight()
    {
        _sprite.flipX = true;
    }
   /* override void OnCollisionEnter2D(Collision collision)
    {
        base.OnCollisionEnter2D(collision);
        RapidAcceleration();
    }
    */
    void RapidAcceleration()
    {
        speed *= ravenousMultiplier;
        attack *= ravenousMultiplier;
    }
}
