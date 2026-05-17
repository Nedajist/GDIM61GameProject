using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeddyBear : Pet
{
    public bool isCub = false;
    public bool isCubAlive = true;
    private int _maxPossibleHealing = 50;
    private float _currentHealing = 0;
    [SerializeField] float _healCooldown = 0.25f;

    private float _healTimer = 0f;
    private void Update()
    {
        _healTimer -= Time.deltaTime;
    }
    public override void FaceLeft()
    {
        _sprite.flipX = true;
    }

    public override void FaceRight()
    {
        _sprite.flipX = false;
    }

    
    protected override string ReturnAbilityText()
    {
        _abilityText = "Teddy Bear - Nourishment: Heals friendly pets by colliding into them";
        return _abilityText;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        Pet petCollider = collision.transform.GetComponent<Pet>();

        if (GameController.instance.currentGameState == GameState.Combat)
        {
            if (petCollider != null && petCollider.petSide == petSide && _currentHealing < _maxPossibleHealing && _healTimer <= 0)
            {
                petCollider.ReceiveHealing(3);
                _currentHealing += 3;
                _healTimer = _healCooldown;
            }
        }
    }
    public override void Die()
    {
        base.Die();
        if (isCub == true)
        {
            isCubAlive = false;
        }
    }




}
