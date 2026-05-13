using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [SerializeField] protected float _lifespan = 6;
    [SerializeField] protected float _movementSpeed = 4; // right now Speed is redundant and not used by projectile 
    [SerializeField] protected float _minMovementSpeed = 2;
    [SerializeField] bool _initialLockon = true;
    [SerializeField] public float damage;
    [SerializeField] public Rigidbody2D _rb;


    [HideInInspector] public Side side;

    private GameObject _targetEnemy;


    // Start is called before the first frame update
    void Start()
    {
        if (_initialLockon == true) // only used by enemies/boss
        {
            TargetNearestEnemy();
            if (_targetEnemy == null) return;
            transform.right = _targetEnemy.transform.position - transform.position;
        }
    }

    private void FixedUpdate()
    {
        _rb.AddForce(transform.right * _movementSpeed);
        _lifespan -= Time.fixedDeltaTime;

        if (_lifespan <= 0) Destroy(gameObject);

        if (_rb.velocity.magnitude < _minMovementSpeed)
        {
            _rb.velocity = _rb.velocity.normalized * _minMovementSpeed;
        }

    }


    private void TargetNearestEnemy()
    {
        List<GameObject> enemyList;
        if (side == Side.player)
        {
            enemyList = GameController.instance.enemyTeamList;
        }
        else
        {
            enemyList = GameController.instance.playerTeamList;
        }


        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemyList)
        {
            if (enemy == null) continue;

            if (nearestEnemy == null)
            {
                nearestEnemy = enemy;
                continue;
            }

            if (Vector3.Distance(enemy.transform.position, transform.position) < Vector3.Distance(nearestEnemy.transform.position, transform.position))
            {
                nearestEnemy = enemy;
            }

        }
        if (nearestEnemy == null) return;
        _targetEnemy = nearestEnemy;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Pet collidingPet = collision.transform.GetComponent<Pet>();
        if (collidingPet != null && collidingPet.petSide != side)
        {
            collidingPet.ReceiveDamage(damage);
            Destroy(gameObject);
        }

        if (collision.transform.GetComponent<Rectangle>() != null)
        {
            collision.transform.GetComponent<Rectangle>().ReceiveDamage(damage);
            Destroy(gameObject);
        }

        if (collision.transform.GetComponent<Obstacle>() != null)
        {
            collision.transform.GetComponent<Obstacle>().ReceiveDamage(damage);
            Destroy(gameObject);
        }

        else
        {
            if (Random.Range(1, 4) > 1) // deflects
            {
                Vector2 lineFromCollider = (Vector2)transform.position - collision.contacts[0].point;
                lineFromCollider = lineFromCollider.normalized;
                _rb.velocity = (lineFromCollider * _minMovementSpeed);
            }
        }

    }
}
