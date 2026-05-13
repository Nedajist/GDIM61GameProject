using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrizzlyGrizzly : Pet
{
    [SerializeField] private SpriteRenderer spriteRender;
    [SerializeField] private float growlDelay = 5f;
    [SerializeField] private GameObject growlOnam;
    [SerializeField] private float growlForce = 10f;
    //[SerializeField] private GameObject growlCollider;
    private Rigidbody2D rb;
    private bool isGrowling;
    [SerializeField] TeddyBear teddyBear;

    private List<GameObject> allyList;
    private bool _enraged; 
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
        if (teddyBear.isCubAlive == false && !_enraged)
        {
            RagePhase();
            if (isGrowling == false)
            {
                StartCoroutine(Growl(growlDelay));
            }
           // speedMultiplier += 0.5f;
            _chanceToTargetEnemyOnCollision = 0.8f;
            _enraged = true;
        }
    }
    void RagePhase()
    {
        spriteRender.color = new Color(0.831f, 0.082f, 0.255f);
        SetColor();
    }
    IEnumerator Growl(float cooldown)
    {
        /*while (cooldown >= 0)
        {
            cooldown -= Time.fixedDeltaTime;
            growlOnam.SetActive(true);
            foreach (GameObject pets in teamList)
            {
                Rigidbody2D rb = pets.GetComponent<Rigidbody2D>();
                if (pets.transform.position.y < transform.position.y)
                {
                    growlForce *= -1;
                }
                rb.AddForce(transform.up * growlForce);
            }
            yield return new WaitForFixedUpdate();
            growlOnam.SetActive(false);
        }*/
        isGrowling = true;

        while (true)
        {
            growlOnam.SetActive(true);
            foreach (GameObject pets in teamList)
            {
                Rigidbody2D rb = pets.GetComponent<Rigidbody2D>();
                if (pets.transform.position.x < transform.position.x)
                {
                    growlForce *= -1;
                }
                rb.AddForce(transform.up * growlForce);
                if (growlForce < 1)
                {
                    growlForce *= -1;
                }
            }     
            yield return new WaitForSeconds(1f);
            
            growlOnam.SetActive(false);

            yield return new WaitForSeconds(cooldown);
        }
    }
}
