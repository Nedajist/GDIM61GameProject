using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ThreeRats : Pet
{
    [SerializeField] GameObject _one_rat;

    private GameObject _instantiated_rat;
    bool alive = true; // to prevent infinite recursion via big bunny ability chaining


    public override void FaceLeft()
    {
        _sprite.flipX = false;
    }

    public override void FaceRight()
    {
        _sprite.flipX = true;
    }

    public override void Die()
    {
        alive = false;
        //Debug.Log("Die called");
        Destroy(gameObject);
        SummonRat(transform.position + new Vector3(1, 1, 0)); // first rat summon
        SummonRat(transform.position + new Vector3(-1, -1, 0)); // first rat summon

    }

    void SummonRat(Vector3 position)
    {
        _instantiated_rat = Instantiate(_one_rat, position, Quaternion.identity);
        _instantiated_rat.GetComponent<Pet>().petSide = petSide;

        //Debug.Log("Rat summoned!");

        if (petSide == Side.player)
        {
            _instantiated_rat.GetComponent<Pet>().FaceRight();
        }
        else
        {
            _instantiated_rat.GetComponent<Pet>().FaceLeft();
        }

    }

    protected override string ReturnAbilityText()
    {
        _abilityText = "Swarm - Upon death, create two 1 / 1 copies of itself without this ability";
        return _abilityText;
    }

}
