using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweetSeagull : Pet
{
    public override void FaceLeft()
    {
        _sprite.flipX = true;
    }

    public override void FaceRight()
    {
        _sprite.flipX = false;
    }


    public override void Die()
    {
        base.Die();
        foreach (GameObject pet in enemyList)
        {
            pet.GetComponent<Pet>().StartCoroutine(pet.GetComponent<Pet>().Freeze(1.5f));
        }
    }


    protected override string ReturnAbilityText()
    {
        _abilityText = "Sweet Seagull - Frosting Spin: When this pet dies, all enemy pets are frozen for 1.5 seconds;";
        return _abilityText;
    }

}
