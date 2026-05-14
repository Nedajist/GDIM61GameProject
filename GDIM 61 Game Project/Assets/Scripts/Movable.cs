using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D _rb;

    public Rigidbody2D getRigidbody()
    {
        return _rb;
    }
}
