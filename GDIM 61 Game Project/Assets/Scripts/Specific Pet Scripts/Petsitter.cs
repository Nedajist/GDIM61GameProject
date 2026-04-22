using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Petsitter : Pet
{



    public override void FaceLeft()
    {
        sprite.flipX = true;
    }

    public override void FaceRight()
    {
        sprite.flipX = false;
    }

    public override void AllyPetEnterPostAttack()
    {
        WhoAndWhere();
        
        if (current_position == 0 && team_list.Count > 1) // if teddy bear is at the front
        {
            team_list[1].GetComponent<Pet>().healthPoints += 1;
        }
        
        else if (current_position == team_list.Count - 1) // if teddy bear is at the back
        {
            team_list[current_position - 1].GetComponent<Pet>().healthPoints += 1;
        }

        else if (team_list.Count > 2) // if teddy bear is somewhere in between
        {
            if (team_list[current_position + 1].GetComponent<Pet>().healthPoints > team_list[current_position - 1].GetComponent<Pet>().healthPoints)
            {
                team_list[current_position + 1].GetComponent<Pet>().healthPoints += 1; // heals pet in behind
            }
            else
            {
                team_list[current_position - 1].GetComponent<Pet>().healthPoints += 1; // heals pet in front
            }


        }
    }
    protected override string ReturnAbilityText()
    {
        abilityText = "Teddy Bear - Nourishment: At the end of your turn, the adjacent allied pet with the least HP gains 1 HP";
        return abilityText;
    }


}
