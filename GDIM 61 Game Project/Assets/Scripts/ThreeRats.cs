using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ThreeRats : Pet
{
    [SerializeField] GameObject _one_rat;

    private GameObject _instantiated_rat;
    bool alive = true; // to prevent infinite recursion via big bunny ability chaining

    public override void ReceiveDamage(int damage, Pet aggressor)
    {
        if (alive == false)
        {
            return;
        }
        //Debug.Log("rat received damage!");
        healthPoints -= 1;
        if (healthPoints <= 0)
        {
            //Debug.Log("dying rat");
            Die();
        }
    }

    public override void FaceLeft()
    {
        sprite.flipX = false;
    }

    public override void FaceRight()
    {
        sprite.flipX = true;
    }

    public override void Die()
    {
        alive = false;
        //Debug.Log("Die called");
        Destroy(gameObject);
        WhoAndWhere();

        if (ally == true)
        {
            team_list = GameController.instance.playerTeamList;
            team_position_list = GameController.instance.playerPositionList;
        }
        else
        {
            team_list = GameController.instance.enemyTeamList;
            team_position_list = GameController.instance.enemyPositionList;

        }

        for (int i = 0; i < team_list.Count; i ++)
        {
            if (team_list[i].GetInstanceID() == gameObject.GetInstanceID())
            {
                current_position = i;
            }
            else
            {
                team_list[i].GetComponent<Pet>().AllyDied();
            }

        }

        SummonRat(current_position); // first rat summon

        if (team_list.Count < 6)
        {
            SummonRat(-1); // second rat summon
        }

    }

    void SummonRat(int team_index)
    {
        Debug.Log("Rat summoned!");
        if (team_index < 0) //summons rat at end of list
        {
            _instantiated_rat = Instantiate(_one_rat, team_position_list[team_list.Count], Quaternion.identity);
            _instantiated_rat.GetComponent<Pet>().ally = ally;
            team_list.Add(_instantiated_rat);
        }
        else // summons rat at dead many rats position
        {
            _instantiated_rat = Instantiate(_one_rat, team_position_list[team_index], Quaternion.identity);
            _instantiated_rat.GetComponent<Pet>().ally = ally;
            team_list[team_index] = _instantiated_rat;
        }


        if (ally)
        {
            _instantiated_rat.GetComponent<Pet>().FaceRight();
        }
        else
        {
            _instantiated_rat.GetComponent<Pet>().FaceLeft();
        }


    }



}
