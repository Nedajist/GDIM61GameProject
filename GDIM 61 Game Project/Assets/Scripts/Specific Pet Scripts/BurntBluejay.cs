using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurntBluejay : Pet
{
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private float maxTime = 3f;
    [SerializeField] private float launchForce = 3f;
    private float fireBallTime;
    protected override void Start()
    {
        base.Start();
        fireBallTime = maxTime;
    }
    public override void FaceLeft()
    {
        _sprite.flipX = true;
    }
    public override void FaceRight()
    {
        _sprite.flipX = false;
    }
    void Update()
    {
        if (GameController.instance.currentGameState == GameState.Combat)
        {
            Fireball();
        }
    }
    void Fireball()
    {
        fireBallTime -= Time.deltaTime;
        if (fireBallTime >= 0)
        {
            GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
            Vector2 randomDirection  = Random.insideUnitCircle.normalized;
            rb.velocity = randomDirection * launchForce;
        }
    }

    protected override string ReturnAbilityText()
    {
        _abilityText = "Burnt Bluejay - Flaming Spirit: lobs fireballs at enemies";
        return _abilityText;
    }

}
