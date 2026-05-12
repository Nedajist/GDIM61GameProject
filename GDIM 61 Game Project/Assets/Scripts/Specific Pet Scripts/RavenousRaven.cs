using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RavenousRaven : Pet
{
    [SerializeField] private float ravenousMultiplier = 1.05f;
    public override void FaceLeft()
    {
        _sprite.flipX = false;
    }
    public override void FaceRight()
    {
        _sprite.flipX = true;
    }
    void RapidAcceleration()
    {
        StartCoroutine(FlashColor(0.1f, 0.1f, Color.magenta));
        speed *= ravenousMultiplier;
        attack *= ravenousMultiplier;
    }

    public override void ReceiveDamage(float damage)
    {
        //damage sfx
        float previousHealthPoints = healthPoints;
        healthPoints -= damage;
        if (healthPoints <= 0)
        {
            Die();
        }


        if (damage > 0)
        {
            RapidAcceleration();
            Pet nearestEnemy = GetNearestEnemy().GetComponent<Pet>();
            if (nearestEnemy != null)
            {
                nearestEnemy.speed = nearestEnemy.speed * 0.8f;
                nearestEnemy.StartCoroutine(nearestEnemy.FlashColor(0.2f, 0.2f, Color.magenta));
                nearestEnemy.GetComponent<StatusBarManager>().StartStatus(StatusType.slow, 0.6f, "SLOWED");
            }
            transform.GetComponent<HealthBar>().StartCoroutine(transform.GetComponent<HealthBar>().TempSizeChange(0.15f, 0.15f, 0.5f));
        }

    }

    protected override string ReturnAbilityText()
    {
        _abilityText = "Ravenous Raven - Retaliate: If this raven gets attacked, its attacker gets slowed while it speeds up!";
        return _abilityText;
    }
}
