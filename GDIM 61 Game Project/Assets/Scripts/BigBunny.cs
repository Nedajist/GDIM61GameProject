using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class BigBunny : Pet
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void FaceLeft()
    {
        sprite.flipX = true;
    }

    public override void FaceRight()
    {
        sprite.flipX = false;
    }
}
