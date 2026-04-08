using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet : MonoBehaviour
{
    [SerializeField] public int healthPoints;
    [SerializeField] public int attack;

    ArrayList ability_list;


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


}
