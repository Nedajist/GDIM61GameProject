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
    [SerializeField] protected GameObject petTooltipPrefab;
    private bool petClicked = false;

    protected int current_position = -1;
    protected List<GameObject> team_list; // RELATIVE TO THIS PET
    protected Vector3[] team_position_list;

    protected List<GameObject> enemy_list; // RELATIVE TO THIS PET
    protected Vector3[] enemy_position_list;
    
void Update()
{
    Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

    if (hit.collider != null)
    {
        if (hit.transform == transform)
        {
            sprite.color = Color.blue;
            /*UIController.instance.ShowStats(healthPoints, attack);*/
            //detect left click
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Pet clicked: " + gameObject.name);
                petClicked = true;
            }
            if(petClicked == true)
                    {
                        if(Input.GetKeyDown(KeyCode.Alpha1))
                        {
                            GameController.instance.ChangeOrder(this, 0);
                                Debug.Log("Changing order of " + gameObject.name + " to position 1");
                            petClicked = false;
                        }
                        if(Input.GetKeyDown(KeyCode.Alpha2))
                        {
                            GameController.instance.ChangeOrder(this, 1);
                                Debug.Log("Changing order of " + gameObject.name + " to position 2");
                            petClicked = false;
                        }
                        if(Input.GetKeyDown(KeyCode.Alpha3))
                        {
                            GameController.instance.ChangeOrder(this, 2);
                                Debug.Log("Changing order of " + gameObject.name + " to position 3");
                            petClicked = false;
                        }
                        if(Input.GetKeyDown(KeyCode.Alpha4))
                        {
                            GameController.instance.ChangeOrder(this, 3);
                                Debug.Log("Changing order of " + gameObject.name + " to position 4");
                            petClicked = false;
                        }
                        if(Input.GetKeyDown(KeyCode.Alpha5))
                        {
                            GameController.instance.ChangeOrder(this, 4);
                                Debug.Log("Changing order of " + gameObject.name + " to position 5");
                            petClicked = false;
                        }
                        if(Input.GetKeyDown(KeyCode.Alpha6))
                        {
                            GameController.instance.ChangeOrder(this, 5);
                                Debug.Log("Changing order of " + gameObject.name + " to position 6");
                            petClicked = false;
                        }
                    }
        }
        else
        {
             sprite.color = Color.white;
        }
    }
}

    public virtual void ReceiveDamage(int damage, Pet aggressor)
    {
        healthPoints -= damage;
        if (healthPoints <= 0)
        {
            Die();
        }

        Debug.Log(aggressor.name);
    }

    public virtual void WhoAndWhere() // sets team_list, team_position_list, and current_position within both
    {
        if (ally == true)
        {
            team_list = GameController.instance.playerTeamList;
            team_position_list = GameController.instance.playerPositionList;

            enemy_list = GameController.instance.enemyTeamList;
            team_position_list = GameController.instance.enemyPositionList;
        }
        else
        {
            team_list = GameController.instance.enemyTeamList;
            team_position_list = GameController.instance.enemyPositionList;

            enemy_list = GameController.instance.playerTeamList;
            team_position_list = GameController.instance.playerPositionList;
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
        Debug.Log(frozen_turns);
        if (frozen_turns > 0)
        {
            frozen_turns -= 1;
        }
    }
    public virtual void EnterAttack() // right as the pet attacks
    {

    }
    public virtual void EnterPostAttack() // right after the pet attacks
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

//use a collider to detect if cursor is hovering over animal sprite, then display tooltip with pet stats and abilities. Use OnMouseOver() method to detect hovering and OnMouseExit() to hide tooltip.
