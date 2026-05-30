using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        /*Pet shotPet = other.transform.GetComponent<Pet>();
        if (shotPet != null)
        {
            Destroy(gameObject);
        }
        */
        Destroy(gameObject);
    }
}
