using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CatCar : Pet
{
    private bool isDriving;
    private bool phase2;
    protected override void Start()
    {
        base.Start();
        isDriving = false;
        phase2 = false;
        speed = 0f;
    }
    void Update()
    {
        _rb.freezeRotation = true;
        if (isDriving == false && phase2 == false)
        {
            StartCoroutine(Drive());
        }
        if (_rb.velocity.x < 0f)
        {
            FaceLeft();
        }
        else
        {
            FaceRight();
        }
    }
    protected override void FixedUpdate()
    {
        
    }
    public override void FaceLeft()
    {
        _sprite.flipX = true;
    }

    public override void FaceRight()
    {
        _sprite.flipX = false;
    }

    // Update is called once per frame
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }

    private IEnumerator Drive()
    {

        yield return new WaitForSeconds(2f);

        SetVelocityInRandomDirection();
        isDriving = true;
        speed = 20f;

        yield return new WaitForSeconds(2f);

        speed = 0f;
        isDriving = false;
    }

}
