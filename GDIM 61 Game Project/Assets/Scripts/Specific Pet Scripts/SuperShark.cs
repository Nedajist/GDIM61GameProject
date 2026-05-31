using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperShark : Pet
{
    [SerializeField] private GameObject _whirlPool;
    [SerializeField] private float _projectileFireRadius;
    [SerializeField] private float _secondsBetweenProjectiles = 3f;
    Pet whoDidIRunInto;

    private float _projectileTimer = 1f;
    private bool isChasing;
    private bool phase2;

    protected override void Start()
    {
        base.Start();
        phase2 = false;
    }

    public override void FaceLeft()
    {
        _sprite.flipX = false;
    }

    public override void FaceRight()
    {
        _sprite.flipX = true;
    }

    private void Update()
    {
        SmellBlood();
        if(healthPoints <= 0.5 * maxHealthPoints)
        {
            phase2 = true;
        }
        if (GameController.instance.currentGameState != GameState.Combat)
        {
            return;
        }
        _projectileTimer -= Time.deltaTime;
        if (_projectileTimer <= 0 && phase2 == true)
        {
            GameObject target = GetNearestEnemy();
            if (target == null) return;
            Vector3 lineToTarget = target.transform.position - transform.position;
            lineToTarget = lineToTarget.normalized * _projectileFireRadius;

            GameObject instantiated_whirlpool = Instantiate(_whirlPool, transform.position + lineToTarget, Quaternion.identity);
            SharkAOECircle whirlpool = instantiated_whirlpool.GetComponent<SharkAOECircle>();
            whirlpool.originator = transform.gameObject;
            whirlpool.direction = (target.transform.position - whirlpool.transform.position).normalized;
            _projectileTimer = _secondsBetweenProjectiles;
            Debug.Log("Whirlpool instantiated");
        }
    }


    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        Vector2 lineToCollider = collision.contacts[0].point - (Vector2) transform.position;
        Pet collidingPet = collision.transform.GetComponent<Pet>();
        float approaching = Vector2.Dot(lineToCollider, collision.relativeVelocity); 

        if (collidingPet != null && collidingPet.petSide != petSide)
        {
            isChasing = true;
            whoDidIRunInto = collidingPet;
        }
    }
    void SmellBlood()
    {
        if (isChasing == true && whoDidIRunInto != null)
        {
            GameController.instance.CullLists(enemyList);
            Vector2 direction = ((Vector2)whoDidIRunInto.transform.position - _rb.position).normalized;

            _rb.MovePosition(_rb.position + direction * speed * Time.fixedDeltaTime);
        }
        if (whoDidIRunInto == null || whoDidIRunInto.healthPoints <= 0)
        {
            isChasing = false;
        }
    }
}
