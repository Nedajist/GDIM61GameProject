using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.U2D;

public enum Side
{
    player, 
    ai
}

public class Pet : Entity
{
    [SerializeField] public float attack;
    [SerializeField] public int cost;
    [SerializeField] public float speed;
    [SerializeField] public Side petSide;
    [SerializeField] protected GameObject petTooltipPrefab;
    [SerializeField] protected float _secondsBetweenMovement;
    public bool bought = false;
    public float maxHealthPoints;
    public List<GameObject> teamList; // RELATIVE TO THIS PET
    public List<GameObject> enemyList; // RELATIVE TO THIS PETs
    public float speedMultiplier = 1f;
    public float speedBoostPerCollision = 0.2f;

    private bool _movementActivated = false;


    protected int _currentPosition = -1;
    protected string _abilityText = "Temp";
    protected float _movementTimer = 0f;
    private float timeRemaining = 0.5f;


    private void Awake()
    {
        maxHealthPoints = healthPoints;

    }

    public virtual void Start()
    {
        SetColor();
        StartCoroutine(MouseDetect());
    }

    protected virtual void FixedUpdate()
    {
        /*

        NEW COMBAT
        physics based bey-blades sim
        - player clicks on pet
        - player moves cursor
        - update method sets the transform of the pet to match the cursor each frame

        */

        if (GameController.instance.currentGameState == GameState.Combat)
        {
            _movementTimer -= Time.fixedDeltaTime;
            if (_movementTimer <= 0)
            {
                _movementTimer = _secondsBetweenMovement;
                if (Random.Range(1,4) > 1)
                {
                    SetVelocityInRandomDirection();
                }
                else
                {
                    SetVelocityTowardsRandomEnemy();
                }

            }

            if (_rb.velocity.magnitude < speed * speedMultiplier)
            {
                _rb.velocity = _rb.velocity.normalized * speed * speedMultiplier;
            }

        }
        


    }

    protected void SetVelocityInRandomDirection()
    {
        Vector2 randomVector2 = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        randomVector2 = randomVector2.normalized;
        _rb.velocity = (randomVector2 * speed * speedMultiplier);
    }

    protected void SetVelocityTowardsRandomEnemy()
    {
        if (enemyList.Count == 0)
        {
            return;
        }
        GameController.instance.CullLists(enemyList);
        Vector2 randomVector2 = enemyList[Random.Range(0, enemyList.Count)].transform.position - transform.position;
        randomVector2 = randomVector2.normalized;
        _rb.velocity = (randomVector2 * speed * speedMultiplier);
    }

    protected void SetVelocityTowardsNearestEnemy()
    {
        if (enemyList.Count == 0)
        {
            return;
        }

        GameController.instance.CullLists(enemyList);
        GameObject nearestEnemy = enemyList[0];
        foreach (GameObject enemy in enemyList)
        {
            if (Vector3.Distance(enemy.transform.position, transform.position) < Vector3.Distance(nearestEnemy.transform.position, transform.position))
            {
                nearestEnemy = enemy;
            }
        }

        GameController.instance.CullLists(enemyList);
        Vector2 randomVector2 = nearestEnemy.transform.position - transform.position;
        randomVector2 = randomVector2.normalized;
        _rb.velocity = (randomVector2 * speed * speedMultiplier);
    }


    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {

        if (GameController.instance.currentGameState != GameState.Combat) // no speed enhancements or timer resets if not in combat
        {
            return;
        }

        Pet collidingPet = collision.transform.GetComponent<Pet>();
        Rectangle collidingRectangle = collision.transform.GetComponent<Rectangle>();
        _movementTimer = _secondsBetweenMovement; // resets auto move timer 
        speedMultiplier += speedBoostPerCollision;

        if (collidingRectangle != null) // colliding with drawn rectangle confirmed
        {
            collidingRectangle.ReceiveDamage(attack, transform.GetComponent<Pet>());
        }

        if (collidingPet != null) // colliding with pet confirmed
        {
            Vector2 lineToCollider = collision.contacts[0].point - (Vector2) transform.position;
            lineToCollider = lineToCollider.normalized;

            float approaching = Vector2.Dot(lineToCollider, collision.relativeVelocity); // linetocollider is moving from THIS object to the OTHER object. If the dot product between linetocollider and the OTHER object's relative velocity is negative, the OTHER object is not moving towards THIS object (i think) 

            if (collidingPet.petSide != petSide && approaching < 0)
            {
                collidingPet.ReceiveDamage(attack, transform.GetComponent<Pet>());

                GameController.instance.CullLists(teamList);
                foreach (GameObject petObject in teamList)
                {
                    if (petObject.transform.GetInstanceID() != transform.GetInstanceID())
                    {
                        petObject.GetComponent<Pet>().AllyAttacked();
                    }

                }

            }

        }

        if (Random.Range(1, 4) > 1) // deflects
        {
            Vector2 lineFromCollider = (Vector2)transform.position - collision.contacts[0].point;
            lineFromCollider = lineFromCollider.normalized;
            _rb.velocity = (lineFromCollider * speedMultiplier);
        }
        else // prevents endless bouncing since timer reset each bounce and not enough time to auto target 
        {
            SetVelocityInRandomDirection();
        }



    }

    public void ReceiveHealing(float amount)
    {
        healthPoints += amount;
        maxHealthPoints += amount;
        transform.GetComponent<HealthBar>().UpdateBarScales();
    }

    public override void ReceiveDamage(float damage, Pet aggressor)
    {
        //damage sfx
        float previousHealthPoints = healthPoints;
        healthPoints -= damage;
        if (healthPoints <= 0)
        {
            Die();
        }
        

        if (damage > 0)
        {
            StartCoroutine(FlashColor(0.1f, 0.1f, Color.red));
            transform.GetComponent<HealthBar>().StartCoroutine(transform.GetComponent<HealthBar>().TempSizeChange(0.15f, 0.15f, 0.5f));
        }


    }

    private void PurchaseCheck() // updates balance text, adds pet to playerteamlist 
    {
        if (bought == false)
        {
            GameController.instance.balance -= cost;
            GameController.instance.UI.balanceText.text = GameController.instance.balance.ToString();
            GameController.instance.playerTeamList.Add(transform.gameObject);
            GameController.instance.playerShopList.Remove(transform.gameObject);
            bought = true;
        }
    }


    public virtual void Die()
    {
        StartCoroutine(FadeAway(1));
    }

    public virtual void FaceLeft()
    {

    }

    public virtual void FaceRight()
    {

    }


    public virtual void AllyDied() 
    {
        
    }

    public virtual void AllyAttacked()
    {

    }

    protected virtual string ReturnAbilityText()
    {
        return _abilityText;
    }

    private void DamageFlashTimer()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        if (timeRemaining <= 0)
        {
            _sprite.color = Color.white;
            timeRemaining = 0.5f;
        }
    }

    protected void FreezeVelocity()
    {
        _rb.velocity = Vector3.zero;
    }


    protected override IEnumerator FadeAway(float duration)
    {
        float timer = duration;
        _rb.simulated = false; // disabled rigidbody
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            _sprite.color = Color.Lerp(_sprite.color, new Color(Color.red.r, Color.red.b, Color.red.g, timer / duration), timer / duration);
            yield return new WaitForFixedUpdate();
        }

        foreach (GameObject petObject in teamList)
        {
            if (petObject.transform.GetInstanceID() != transform.GetInstanceID())
            {
                petObject.GetComponent<Pet>().AllyDied();
            }
        }

        Destroy(gameObject);
        teamList.Remove(transform.gameObject); // deletes self from teamlist 

        GameController.instance.CheckForVictor();



    }

    protected IEnumerator MouseDetect()
    {
        while (GameController.instance.currentGameState == GameState.BuyPhase)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            bool isClicked = false;


            if (hit.collider != null & petSide == Side.player && GameController.instance.currentGameState == GameState.BuyPhase)
            {
                if (hit.transform == transform)
                {

                    ReturnAbilityText();
                    UIController.Instance.ShowStats(healthPoints, attack, cost, _abilityText);
                    _sprite.color = Color.grey;

                    if (Input.GetMouseButton(0))
                    {
                        if (cost <= GameController.instance.balance || bought == true)
                        {

                            isClicked = true;
                            PurchaseCheck();
                        }
                    }
                }

            }
            else
            {
                _sprite.color = originalColor;
            }

            if (isClicked == true)
            {
                transform.position = mousePos;
            }
            yield return null;
        }
        _sprite.color = originalColor;
        yield return null;

    }



    public IEnumerator Freeze(float duration)
    {
        StartCoroutine(FlashColor(duration, 0.1f, Color.cyan));
        float timer = duration;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            FreezeVelocity();
            yield return new WaitForFixedUpdate();
        }
    }








}

