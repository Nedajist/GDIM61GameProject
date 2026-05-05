using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hyena : Pet
{
    [SerializeField] GameObject smallHyena;
    public override void FaceLeft()
    {
        _sprite.flipX = true;
    }

    public override void FaceRight()
    {
        _sprite.flipX = false;
    }

    protected override void DamageCheck(Pet other) // given other pet that this pet has collided into, evaluates whether or not it should recieve dmg + if any of this pet's special abilities will activate
    {
        if (other.petSide != petSide)
        {
            other.ReceiveDamage(attack);
            GameController.instance.CullLists(teamList);
            AlertAlliesOfAttack();
            SummonHyena(transform.position + new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), 0));
        }
    }


    protected override string ReturnAbilityText()
    {
        _abilityText = "Hyaena - Pack: Every time this animal attacks, it summons a little friend";
        return _abilityText;
    }

    void SummonHyena(Vector3 position)
    {
        GameObject instantiatedHyena = Instantiate(smallHyena, position, Quaternion.identity);
        teamList.Add(instantiatedHyena);
        Pet summon = instantiatedHyena.GetComponent<SmallHyena>();

        summon.petSide = petSide;
        summon.enemyList = enemyList;
        summon.teamList = teamList;
        summon.GetComponent<HealthBar>().ShowHealthBar();
        //Debug.Log("Rat summoned!");

        if (petSide == Side.player)
        {
            summon.FaceRight();
        }
        else
        {
            summon.FaceLeft();
        }

    }

}
