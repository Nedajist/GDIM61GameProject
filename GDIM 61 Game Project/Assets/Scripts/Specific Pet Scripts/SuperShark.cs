using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperShark : Pet
{
    [SerializeField] private GameObject _whirlPool;
    [SerializeField] private float _projectileFireRadius;
    [SerializeField] private float _secondsBetweenProjectiles = 3f;


    private float _projectileTimer = 1f;

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
        if (GameController.instance.currentGameState != GameState.Combat)
        {
            return;
        }
        _projectileTimer -= Time.deltaTime;
        if (_projectileTimer <= 0)
        {
            GameObject target = GetNearestEnemy();
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

        if (collidingPet != null && collidingPet.petSide != petSide && approaching >= 0)
        {
            SmellBlood(collidingPet);
        }
    }
    void SmellBlood(Pet whoDidIRunInto)
    {
        GameController.instance.CullLists(enemyList);
        Vector2 randomVector2 = whoDidIRunInto.transform.position - transform.position;
        randomVector2 = randomVector2.normalized;
        _rb.velocity = (randomVector2 * speed * speedMultiplier);
    }
}
