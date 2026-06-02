using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CatCar : Pet
{
    [SerializeField] private Sprite planeSprite;
    [SerializeField] private GameObject bullet;
    [SerializeField] private float bulletOffset;
    private SpriteRenderer spriteRenderer;
    private bool isDriving;
    private bool phase2;
    private bool notShooting = true;
    protected override void Start()
    {
        base.Start();
        speedBoostPerCollision = 0f;
        spriteRenderer = GetComponent<SpriteRenderer>();
        isDriving = false;
        phase2 = false;
        if (phase2 == false)
        {
            speed = 0f;
        }
        isCatCar = true;
    }
    void Update()
    {
        _rb.freezeRotation = true;
        if (healthPoints <= 0.5f * maxHealthPoints)
        {
            if (phase2 == false)
            {
                phase2 = true;
                speed = 5f;
            }
        }
        if (isDriving == false)
        {
            isDriving = true;
            StartCoroutine(Drive());
        }

        if (phase2 == true && GameController.instance.currentGameState == GameState.Combat)
        {
            spriteRenderer.sprite = planeSprite;
            if (notShooting)
            {
                notShooting = false;
                StartCoroutine(WalkEmDown());
            }
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

        yield return new WaitForSeconds(0.2f);

        speed = 20f;
        SetVelocityTowardsNearestEnemy();

        yield return new WaitForSeconds(0.5f);

        speed = 0f;
        isDriving = false;
    }
    private IEnumerator WalkEmDown()
    {
        yield return new WaitForSeconds(.2f);

        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y - bulletOffset, transform.position.z);
        GameObject thisBullet = Instantiate(bullet, spawnPos, Quaternion.identity);
        Rigidbody2D bulletRb = thisBullet.GetComponent<Rigidbody2D>();
        if(_sprite.flipX == true)
        {
            bulletRb.velocity = transform.right * 20f;
        }
        else
        {
            bulletRb.velocity = transform.right * -20f;
        }

        notShooting = true;

        yield return new WaitForSeconds(1f);

        Destroy(thisBullet);
    }
    
}
