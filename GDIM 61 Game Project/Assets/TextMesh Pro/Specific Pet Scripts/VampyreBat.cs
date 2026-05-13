using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VampyreBat : Pet
{
    private float maxHealing = 50;
    private float currentHealing = 0;
    public override void FaceLeft()
    {
        _sprite.flipX = false;
    }

    public override void FaceRight()
    {
        _sprite.flipX = true;
    }

    protected override void DamageCheck(Pet other)
    {
        if (other && currentHealing < maxHealing)
        {
            currentHealing += attack;
            other.ReceiveDamage(attack);
            other.transform.GetComponent<StatusBarManager>().StartStatus(StatusType.sap, 0.5f, "SAPPED");
            ReceiveHealing(attack);
            GameController.instance.CullLists(teamList);
            AlertAlliesOfAttack();
            StartCoroutine(FlashColor(0.15f, 0.15f, Color.yellow));
        }
    }

    protected override string ReturnAbilityText()
    {
        _abilityText = "Vampyre Bat - Omnomnom: Attacks allies and enemies. Heals for all damage dealt.";
        return _abilityText;
    }
}
