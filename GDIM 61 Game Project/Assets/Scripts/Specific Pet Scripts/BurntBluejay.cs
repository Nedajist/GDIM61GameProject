using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurntBluejay : Pet
{
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private float launchForce = 3f;
    [SerializeField] private float fireballCooldown = 2f;
    private float fireballTimer;
    protected override void Start()
    {
        base.Start();
        fireballTimer = fireballCooldown;
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
        fireballTimer -= Time.deltaTime;
        if (fireballTimer <= 0)
        {
            GameObject fireballObject = Instantiate(fireballPrefab, transform.position + transform.right * 2, Quaternion.identity);
            Projectile fireball = fireballObject.GetComponent<Projectile>();
            fireball.side = petSide;
            fireballTimer = fireballCooldown;
        }
    }

    protected override string ReturnAbilityText()
    {
        _abilityText = "Burnt Bluejay - Flaming Spirit: lobs fireballs at enemies";
        return _abilityText;
    }

}
