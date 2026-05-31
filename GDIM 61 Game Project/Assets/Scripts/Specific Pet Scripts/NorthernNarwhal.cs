using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class NorthernNarwhal : Pet
{
    [SerializeField] private float _secondsBetweenProjectiles = 3f;
    private bool phase2;
    protected override void Start()
    {
        base.Start();
        phase2 = false;
    }


    private void Update()
    {
        _rb.freezeRotation = true;
        if (_rb.velocity.x < 0f)
        {
            FaceLeft();
        }
        else
        {
            FaceRight();
        }
        if (healthPoints <= maxHealthPoints * 0.5f)
        {
            phase2 = true;
        }
    }


    private bool isCharging = false;
    public override void FaceLeft()
    {
        _sprite.flipX = false;
    }

    public override void FaceRight()
    {
        _sprite.flipX = true;
    }

    // Update is called once per frame
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        Spearhead(isCharging);
        GameObject otherPet;
        otherPet = collision.gameObject;
        Pet component = otherPet.GetComponent<Pet>();
        if (component != null && phase2 == true)
        {
            otherPet.GetComponent<Pet>().StartCoroutine(otherPet.GetComponent<Pet>().Freeze(1.5f));     
        }
    }
    void Spearhead(bool charge)
    {
        if (charge == false)
        {
            speedMultiplier *= 3f;
            isCharging = true;
        }
        else
        {
            speedMultiplier /= 3f;
            isCharging = false;
        }
    }
   /* private IEnumerator FreezePet(Pet otherPet)
    {
        otherPet = RigidbodyType2D.Static;

        yield return new WaitForSeconds(3f);

        otherRb.bodyType = RigidbodyType2D.Dynamic;
    }
    */
}
