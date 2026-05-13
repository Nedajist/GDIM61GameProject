using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrizzlyGrizzly : Pet
{
    [SerializeField] private SpriteRenderer spriteRender;
    [SerializeField] private float growlDelay = 5f;
    [SerializeField] private GameObject growlOnam;
    //Onam position modifiers
            [SerializeField] private float xOnamPosModifier;
            [SerializeField] private float yOnamPosModifier;
    [SerializeField] private float growlForce = 10f;
    //[SerializeField] private GameObject growlCollider;
    private Rigidbody2D rb;
    [SerializeField] TeddyBear teddyBear;

    private List<GameObject> allyList;
    public override void FaceLeft()
    {
        _sprite.flipX = true;
    }

    public override void FaceRight()
    {
        _sprite.flipX = false;
    }
    protected override void Start()
    {
        base.Start();
    }
    void Update()
    {
        if (teddyBear.isCubAlive == false)
        {
            RagePhase();
            StartCoroutine(Growl(growlDelay));
        }
    }
    void RagePhase()
    {
        spriteRender.color = Color.red;
    }
    IEnumerator Growl(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);

        Instantiate(growlOnam, transform);       
        foreach (GameObject pets in allyList)
        {
            Rigidbody2D rb = pets.GetComponent<Rigidbody2D>();
            if (pets.transform.position.y < transform.position.y)
            {
                growlForce *= -1;
            }
            rb.AddForce(transform.up * growlForce);
        }
    }
}
