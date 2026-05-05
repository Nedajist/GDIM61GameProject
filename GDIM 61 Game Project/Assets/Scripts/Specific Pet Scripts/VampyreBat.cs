using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampyreBat : Pet
{
    public override void FaceLeft()
    {
        _sprite.flipX = false;
    }

    public override void FaceRight()
    {
        _sprite.flipX = true;
    }
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        Vector2 lineToCollider = collision.contacts[0].point - (Vector2) transform.position;
        Pet collidingPet = collision.transform.GetComponent<Pet>();
        float approaching = Vector2.Dot(lineToCollider, collision.relativeVelocity); 

        if (collidingPet != null && collidingPet.petSide == petSide && approaching >= 0)
        {
            ReceiveHealing(attack);
            collidingPet.ReceiveDamage(attack, this);
            AlertAlliesOfAttack();
        }
    }

    protected override string ReturnAbilityText()
    {
        _abilityText = "Vampyre Bat - Omnomnom: Attacks allies and enemies. Heals for all damage dealt.";
        return _abilityText;
    }
}
