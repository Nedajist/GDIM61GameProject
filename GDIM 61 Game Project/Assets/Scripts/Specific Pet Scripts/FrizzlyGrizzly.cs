using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrizzlyGrizzly : Pet
{
    [SerializeField] SpriteRenderer spriteRender;
    private TeddyBear teddyBear;
    void Update()
    {
        if (teddyBear.isCubAlive == false)
        {
            RagePhase();
        }
    }
    void RagePhase()
    {
        spriteRender.color = Color.red;
    }
}
