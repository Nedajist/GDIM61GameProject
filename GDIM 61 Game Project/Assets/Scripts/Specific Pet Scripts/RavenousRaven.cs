using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RavenousRaven : Pet
{
    [SerializeField] private float ravenousMultiplier = 1.1f;
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
        if (GameController.instance.currentGameState != GameState.Combat) // no speed enhancements or timer resets if not in combat
        {
            return;
        }

        Pet collidingPet = collision.transform.GetComponent<Pet>();
        Rectangle collidingRectangle = collision.transform.GetComponent<Rectangle>();
        _movementTimer = _secondsBetweenMovement; // resets auto move timer 
        speedMultiplier += speedBoostPerCollision;

        if (collidingRectangle != null) // colliding with drawn rectangle confirmed
        {
            collidingRectangle.ReceiveDamage(attack, transform.GetComponent<Pet>());
        }

        if (collidingPet != null) // colliding with pet confirmed
        {
            Vector2 lineToCollider = collision.contacts[0].point - (Vector2)transform.position;
            lineToCollider = lineToCollider.normalized;

            float approaching = Vector2.Dot(lineToCollider, collision.relativeVelocity); // linetocollider is moving from THIS object to the OTHER object. If the dot product between linetocollider and the OTHER object's relative velocity is negative, the OTHER object is not moving towards THIS object (i think) 

            if (collidingPet.petSide != petSide && approaching < 0)
            {
                collidingPet.ReceiveDamage(attack, transform.GetComponent<Pet>());
                GameController.instance.CullLists(teamList);
                RapidAcceleration();
                AlertAlliesOfAttack();
            }

        }
    }
    void RapidAcceleration()
    {
        StartCoroutine(FlashColor(0.1f, 0.1f, Color.magenta));
        speed *= ravenousMultiplier;
        attack *= ravenousMultiplier;
    }

    protected override string ReturnAbilityText()
    {
        _abilityText = "Ravenous Raven - Retaliate: If the user is attacked, its attributes are boosted significantly";
        return _abilityText;
    }
}
