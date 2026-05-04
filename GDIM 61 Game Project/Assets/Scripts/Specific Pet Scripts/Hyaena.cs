using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hyaena : Pet
{
    public override void FaceLeft()
    {
        _sprite.flipX = true;
    }

    public override void FaceRight()
    {
        _sprite.flipX = false;
    }
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        Vector2 lineToCollider = collision.contacts[0].point - (Vector2) transform.position;
        Pet collidingPet = collision.transform.GetComponent<Pet>();
        float approaching = Vector2.Dot(lineToCollider, collision.relativeVelocity); 

        if (collidingPet.petSide != petSide && approaching >= 0)
        {
            maxHealthPoints += 5f;
            healthPoints += 5f;
        }
    }
}
