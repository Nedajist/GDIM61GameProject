using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrizzlyGrizzly : Pet
{
    [SerializeField] private SpriteRenderer spriteRender;
    [SerializeField] private float growlDelay = 5f;
    [SerializeField] private GameObject growlOnam;
    [SerializeField] private float growlForce = 100f;
    //[SerializeField] private GameObject growlCollider;
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
        if (teddyBear != null && teddyBear.isCubAlive == false && !_enraged)
        {
            RagePhase();
            StartCoroutine(Growl(growlDelay));
           
           // speedMultiplier += 0.5f;
            _chanceToTargetEnemyOnCollision = 0.8f;
            _enraged = true;
        }
    }
    void RagePhase()
    {
        _baseAttack += 0.5f;
        speed += 2;
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

        while (true)
        {
            growlOnam.SetActive(true);
            foreach (GameObject pets in enemyList)
            {
                Rigidbody2D __rb = pets.GetComponent<Rigidbody2D>();
                /*if (pets.transform.position.x < transform.position.x)
                {
                    growlForceX *= -1;
                }
                __rb.AddForce(transform.right * growlForceX);
                Debug.Log("Pushed " + pets);
                if (growlForceX < 1)
                {
                    growlForceX *= -1;
                }

                if (pets.transform.position.y < transform.position.y)
                {
                    growlForceY *= -1;
                }
                __rb.AddForce(transform.up * growlForceY);
                if (growlForceY < 1)
                {
                    growlForceY *= -1;
                }   */
                Vector2 direction = (pets.transform.position - transform.position).normalized;

                __rb.AddForce(direction * growlForce, ForceMode2D.Impulse);
            }
            speedMultiplier += 0.5f;
            yield return new WaitForSeconds(1f);
            
            growlOnam.SetActive(false);

            yield return new WaitForSeconds(cooldown);
        }
    }
}
