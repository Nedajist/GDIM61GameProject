using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet : MonoBehaviour
{
    [SerializeField] public int healthPoints;
    [SerializeField] public int attack;
    [SerializeField] public int cost;
    [SerializeField] protected SpriteRenderer sprite;
    [SerializeField] public bool ally;
    [SerializeField] public int frozen_turns = 0;


    protected int current_position = -1;
    protected List<GameObject> team_list;
    protected Vector3[] team_position_list;

    public virtual void ReceiveDamage(int damage)
    {
        healthPoints -= damage;
        if (healthPoints <= 0)
        {
            Die();
        }
    }

    public virtual void WhoAndWhere()
    {

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

        for (int i = 0; i < team_list.Count; i++)
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
    }


    public virtual void DealDamage()
    {

    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    public virtual void FaceLeft()
    {

    }

    public virtual void FaceRight()
    {

    }

    public virtual void EnterPreAttack() // right before the pet attacks
    {

    }
    public virtual void EnterAttack() // right as the pet attacks
    {

    }
    public virtual void EnterPostAttack() // right after the pet attacks
    {

    }
    private void OnMouseOver()
    {
        
    }

    public virtual void AllyDied() 
    {
        
    }

    public virtual void AllyPetEnterPreAttack()
    {


    }

    public virtual void AllyPetEnterAttack()
    {


    }

    public virtual void AllyPetEnterPostAttack()
    {


    }




}
    /*
   // public void OnMouseOver()
//    {
 //       Debug.Log("Mouse is Over);")
  //      if(GameController.instance.currentGameState == GameState.PreCombat)
    //    {
      //      
        }
   // }


}
*/
//use a collider to detect if cursor is hovering over animal sprite, then display tooltip with pet stats and abilities. Use OnMouseOver() method to detect hovering and OnMouseExit() to hide tooltip.
