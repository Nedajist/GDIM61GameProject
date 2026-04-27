using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.VisualScripting.FullSerializer;
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
    public bool in_shop = false;
    private bool petClicked = false;

    protected int current_position = -1;
    protected List<GameObject> team_list; // RELATIVE TO THIS PET
    protected Vector3[] team_position_list;

    protected List<GameObject> enemy_list; // RELATIVE TO THIS PET
    protected Vector3[] enemy_position_list;
    protected string abilityText = "Temp";
    private float timeRemaining = 0.5f;
    
    void Update()
    {
        /*

        NEW COMBAT
        physics based bey-blades sim
        - player clicks on pet
        - player moves cursor
        - update method sets the transform of the pet to match the cursor each frame

        */
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        bool isClicked = false;

       
        if (hit.collider != null)
        {
            if (hit.transform == transform)
            {
                if (Input.GetMouseButton(0))
                {
                    isClicked = true;
                }
            }
        }
        if (isClicked == true)
        {
            transform.position = mousePos;
        }
        

        
/*
//OLD COMBAT SYSTEM
        if (hit.collider != null)
        {
            if (hit.transform == transform)
            {
                Debug.Log(transform);
                ReturnAbilityText();
                UIController.Instance.ShowStats(healthPoints, attack, abilityText);
                sprite.color = Color.grey;
                //detect left click
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Pet clicked: " + gameObject.name);
                    petClicked = true;
                }

                if (petClicked == true)
                {
                    sprite.color = Color.yellow;

                    if (Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        PurchaseCheck();
                        GameController.instance.ChangeOrder(this, 0);
                        petClicked = false;
                    }
                    if (Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        PurchaseCheck();
                        GameController.instance.ChangeOrder(this, 1);
                        petClicked = false;
                    }
                    if (Input.GetKeyDown(KeyCode.Alpha3))
                    {
                        PurchaseCheck();
                        GameController.instance.ChangeOrder(this, 2);
                        petClicked = false;
                    }
                    if (Input.GetKeyDown(KeyCode.Alpha4))
                    {
                        GameController.instance.ChangeOrder(this, 3);
                        petClicked = false;
                        PurchaseCheck();
                    }
                    if (Input.GetKeyDown(KeyCode.Alpha5))
                    {
                        PurchaseCheck();
                        GameController.instance.ChangeOrder(this, 4);
                        petClicked = false;
                    }
                    if (Input.GetKeyDown(KeyCode.Alpha6))
                    {
                        PurchaseCheck();
                        GameController.instance.ChangeOrder(this, 5);
                        petClicked = false;
                    }
                }
            }

        }
        else
        {
            UIController.Instance.HideStats();

            sprite.color = Color.white;

            petClicked = false;
            //Debug.Log("not hovering");
        }
        */
        
    }

    public virtual void ReceiveDamage(int damage, Pet aggressor)
    {
        //damage sfx
        int previousHealthPoints = healthPoints;
        healthPoints -= damage;
        if (healthPoints <= 0)
        {
            Die();
        }
        

        if (damage > 0)
        {
            sprite.color = Color.red;
            DamageFlashTimer();
        }

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
        }
    }

    private void PurchaseCheck() // updates balance text, adds pet to playerteamlist 
    {
        if (in_shop == true)
        {
            GameController.instance.balance -= cost;
            GameController.instance.UI.balanceText.text = "Balance: " + GameController.instance.balance;
            GameController.instance.playerTeamList.Add(transform.gameObject);
            in_shop = false;
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


    protected virtual string ReturnAbilityText()
    {
        return abilityText;
    }

    private void DamageFlashTimer()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        if (timeRemaining <= 0)
        {
            sprite.color = Color.white;
            timeRemaining = 0.5f;
        }
    }

}

