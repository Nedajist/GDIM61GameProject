using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Petsitter : Pet
{

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
        _abilityText = "Teddy Bear - Nourishment: At the end of your turn, the adjacent allied pet with the least HP gains 1 HP";
        return _abilityText;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        Pet petCollider = collision.transform.GetComponent<Pet>();

        if (GameController.instance.currentGameState == GameState.Combat)
        {
            if (petCollider != null && petCollider.petSide == petSide)
            {
                petCollider.healthPoints += 1;
                petCollider.StartCoroutine(petCollider.FlashColor(0.1f, 0.1f, Color.green));
            }
        }
    }




}
