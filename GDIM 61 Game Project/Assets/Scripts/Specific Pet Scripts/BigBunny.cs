using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class BigBunny : Pet
{
    // Start is called before the first frame update


    public override void FaceLeft()
    {
        _sprite.flipX = true;
    }

    public override void FaceRight()
    {
        _sprite.flipX = false;
    }

    public override void AllyDied()
    {
        if (transform == null) return;
        StartCoroutine(FlashColor(0.1f, 0.1f, Color.blue));
        transform.GetComponent<StatusBarManager>().StartStatus(StatusType.charge, 0.5f, "CHARGING");
        speedMultiplier += 0.1f;
        _movementTimer = _secondsBetweenMovement;
        SetVelocityTowardsNearestEnemy();
    }

    protected override string ReturnAbilityText()
    {
        _abilityText = "Big Bunny - Jump kick: when a team member dies, bunny immediately charges towards the closest enemy";
        return _abilityText;
    }
}
