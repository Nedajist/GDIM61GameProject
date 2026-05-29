using System.Collections;
using System.Collections.Generic;
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
    }
    void Update()
    {
        if (isDriving == false && phase2 == false)
        {
            StartCoroutine(Drive());
        }
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

        isDriving = true;
        speed = 10f;

        yield return new WaitForSeconds(2f);

        speed = 0f;
        isDriving = false;
    }

}
