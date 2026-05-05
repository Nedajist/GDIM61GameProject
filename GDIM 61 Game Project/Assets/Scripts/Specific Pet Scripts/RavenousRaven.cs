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

    public override void ReceiveDamage(float damage, Pet aggressor)
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

            if (aggressor)
            {
                speed *= ravenousMultiplier;
                attack *= ravenousMultiplier;
            }

            StartCoroutine(FlashColor(0.1f, 0.1f, Color.magenta));
            transform.GetComponent<HealthBar>().StartCoroutine(transform.GetComponent<HealthBar>().TempSizeChange(0.15f, 0.15f, 0.5f));
        }

    }

    protected override string ReturnAbilityText()
    {
        _abilityText = "Ravenous Raven - Retaliate: If the user is attacked, its attributes are boosted significantly";
        return _abilityText;
    }
}
