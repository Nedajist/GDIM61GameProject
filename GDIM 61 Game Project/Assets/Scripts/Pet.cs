using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet : MonoBehaviour
{
    [SerializeField] public int healthPoints;
    [SerializeField] public int attack;
    [SerializeField] public int cost;
    [SerializeField] public List<string> abilityList = new List<string>();
    [SerializeField] protected SpriteRenderer sprite;

    public virtual void ReceiveDamage(int damage)
    {
        healthPoints -= damage;
        if (healthPoints <= 0)
        {
            Die();
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

    public virtual void EnterPreCombat()
    {

    }

    public virtual void EnterPreAttack()
    {

    }
    public virtual void EnterAttack()
    {

    }
    public virtual void EnterPostAttack()
    {

    }
    private void OnMouseOver()
    {
        
    }
}
//use a collider to detect if cursor is hovering over animal sprite, then display tooltip with pet stats and abilities. Use OnMouseOver() method to detect hovering and OnMouseExit() to hide tooltip.
