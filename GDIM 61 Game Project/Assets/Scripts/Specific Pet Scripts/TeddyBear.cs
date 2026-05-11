using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeddyBear : Pet
{
    public bool isCub = false;
    public bool isCubAlive = true;
    private int maxPossibleHealing = 50;
    private float currentHealing = 0;
    public override void FaceLeft()
    {
        _sprite.flipX = true;
    }

    public override void FaceRight()
    {
        _sprite.flipX = false;
    }

    
    protected override string ReturnAbilityText()
    {
        _abilityText = "Teddy Bear - Nourishment: Heals friendly pets by colliding into them";
        return _abilityText;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        Pet petCollider = collision.transform.GetComponent<Pet>();

        if (GameController.instance.currentGameState == GameState.Combat)
        {
            if (petCollider != null && petCollider.petSide == petSide && currentHealing < maxPossibleHealing)
            {
                petCollider.ReceiveHealing(3);
                currentHealing += 3;
                petCollider.StartCoroutine(petCollider.FlashColor(0.1f, 0.1f, Color.green));
            }
        }
    }
    public override void Die()
    {
        base.Die();
        if (isCub == true)
        {
            isCubAlive = false;
        }
    }




}
