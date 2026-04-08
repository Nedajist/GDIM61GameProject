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
    public void ReceiveDamage(int damage)
    {
        healthPoints -= damage;
        if (healthPoints <= 0)
        {
            Die();
        }
    }

    public void DealDamage()
    {

    }

    public void Die()
    {
        Destroy(transform);
    }

    public virtual void FaceLeft()
    {

    }

    public virtual void FaceRight()
    {

    }

}
