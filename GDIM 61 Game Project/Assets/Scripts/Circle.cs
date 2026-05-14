using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Circle : MonoBehaviour
{
    [SerializeField] protected float _lifespan;
    [SerializeField] protected SpriteRenderer _sprite;
    [SerializeField] protected Rigidbody2D _rb;
    [HideInInspector] public Side _circleSide = Side.ai;

    protected List<Movable> _listOfRecipients = new List<Movable>();
    protected float _maxLifespan;

    protected virtual void Start()
    {
        _maxLifespan = _lifespan;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision) // living things added to list
    {
        if (collision.transform.GetComponent<Movable>() != null)
        {
            _listOfRecipients.Add(collision.transform.GetComponent<Movable>());
        }

    }

    protected virtual void OnTriggerExit2D(Collider2D collision) // living things removed from list 
    {
        if (collision.transform.GetComponent<Movable>() != null)
        {
            _listOfRecipients.Remove(collision.transform.GetComponent<Movable>());
        }
    }
}

