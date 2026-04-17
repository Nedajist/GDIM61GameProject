using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class BigBunny : Pet
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void FaceLeft()
    {
        sprite.flipX = true;
    }

    public override void FaceRight()
    {
        sprite.flipX = false;
    }

    public override void AllyDied()
    {
        //Debug.Log("Ally of big bunny died! Big bunny attacks!");
        WhoAndWhere();
        if (enemy_list[0] != null && frozen_turns == 0)
        {
            enemy_list[0].GetComponent<Pet>().ReceiveDamage(attack, gameObject.GetComponent<Pet>());
        }

    }

}
