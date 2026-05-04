using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeachBall : ParentObstacle
{
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rb = collision.rigidbody;
        rb.velocity *= -1;
    }
}
    

