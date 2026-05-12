using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.VisualScripting;
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
    [SerializeField] public string petName;
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
    private float _chanceToTargetEnemyOnCollision = 0.3f;

    protected int _currentPosition = -1;
    protected string _abilityText = "Temp";
    protected float _movementTimer = 0f;
    private float timeRemaining = 0.5f;

    private float _damageMultiplier = 1;
    private float _maxSpeedMultiplier = 15f;
    private float _trueMaxHealth = 50f;

    private void Awake()
    {
        maxHealthPoints = healthPoints;
    }

    protected virtual void Start()
    {
        SetColor();
        StartCoroutine(MouseDetect());
    }

    protected void BoundaryCheck() // checks if pet is out of bounds
    {
        if (Mathf.Abs(transform.position.x) > 10 || Mathf.Abs(transform.position.y) > 6)
        {
            Die();
        }
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

        BoundaryCheck();

        if (GameController.instance.currentGameState == GameState.Combat)
        {
            _movementTimer -= Time.fixedDeltaTime;
            if (_movementTimer <= 0)
            {
                _movementTimer = _secondsBetweenMovement;
                if (Random.Range(0f, 1f) > _chanceToTargetEnemyOnCollision) 
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
        GameController.instance.CullLists(enemyList);
        if (enemyList.Count == 0) return;

        Vector2 randomVector2 = enemyList[Random.Range(0, enemyList.Count)].transform.position - transform.position;
        randomVector2 = randomVector2.normalized;
        _rb.velocity = (randomVector2 * speed * speedMultiplier);
    }

    protected void SetVelocityTowardsNearestEnemy()
    {
        
        GameObject nearestEnemy = GetNearestEnemy();

        if (nearestEnemy == null) return;
        if (nearestEnemy.GetComponent<Pet>() == null) return;

        GameController.instance.CullLists(enemyList);
        Vector2 randomVector2 = nearestEnemy.transform.position - transform.position;
        randomVector2 = randomVector2.normalized;
        _rb.velocity = (randomVector2 * speed * speedMultiplier);
    }

    protected GameObject GetNearestEnemy()
    {
        if (enemyList.Count == 0)
        {
            return new GameObject();
        }

        GameController.instance.CullLists(enemyList);
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in enemyList)
        {
            if (nearestEnemy == null && enemy != null)
            {
                nearestEnemy = enemy;
                continue;
            }

            if (Vector3.Distance(enemy.transform.position, transform.position) < Vector3.Distance(nearestEnemy.transform.position, transform.position))
            {
                nearestEnemy = enemy;
            }
        }
        return (nearestEnemy);
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
        speedMultiplier = Mathf.Clamp(speedMultiplier, 0, _maxSpeedMultiplier);
        _damageMultiplier = 1 + speedMultiplier / _maxSpeedMultiplier * 2; // right now, max damage boost pets get is 50% from max speed 
        attack = attack * _damageMultiplier;
        Debug.Log(attack);
        
        if (collidingRectangle != null) // colliding with drawn rectangle confirmed
        {
            collidingRectangle.ReceiveDamage(attack);
        }

        if (collidingPet != null) // colliding with pet confirmed
        {
            Vector2 lineToCollider = collision.contacts[0].point - (Vector2) transform.position;
            lineToCollider = lineToCollider.normalized;

            float approaching = Vector2.Dot(lineToCollider, collision.relativeVelocity); // linetocollider is moving from THIS object to the OTHER object. If the dot product between linetocollider and the OTHER object's relative velocity is negative, the OTHER object is not moving towards THIS object (i think) 

            if (approaching < 0)
            {
                DamageCheck(collidingPet);
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


    protected virtual void DamageCheck (Pet other) // given other pet that this pet has collided into, evaluates whether or not it should recieve dmg + if any of this pet's special abilities will activate
    {
        if (other.petSide != petSide)
        {
            other.ReceiveDamage(attack);
            GameController.instance.CullLists(teamList);
            AlertAlliesOfAttack();
        }
    }

    public void ReceiveHealing(float amount)
    {
        if (healthPoints + amount > _trueMaxHealth) return;
        healthPoints += amount;
        if (healthPoints > maxHealthPoints)
        {
            maxHealthPoints = healthPoints;
        }
        transform.GetComponent<HealthBar>().UpdateBarScales();
        transform.GetComponent<StatusBarManager>().StartStatus(StatusType.heal, 0.5f, "HEAL");
    }

    public void AlertAlliesOfAttack()
    {
        foreach (GameObject petObject in teamList)
        {
            if (petObject.transform.GetInstanceID() != transform.GetInstanceID())
            {
                petObject.GetComponent<Pet>().AllyAttacked();
            }
        }
    }

    public override void ReceiveDamage(float damage)
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
            GameController.instance.saveData.playerCoinBalance -= cost;
            UIController.Instance.UpdateCoinBalanceText();
            GameController.instance.playerTeamList.Add(transform.gameObject);
            GameController.instance.playerShopList.Remove(transform.gameObject);
            GameController.instance.saveData.playerTempTeamList.Add(petName);
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
        bool hoveredOnPreviousFrame = false;
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
                    hoveredOnPreviousFrame = true;
                    if (Input.GetMouseButton(0))
                    {
                        if (cost <= GameController.instance.saveData.playerCoinBalance || bought == true)
                        {

                            isClicked = true;
                            PurchaseCheck();
                        }
                    }
                }

            }
            else if (isClicked == false && hoveredOnPreviousFrame == true)
            {
                _sprite.color = originalColor;
                hoveredOnPreviousFrame = false;
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
        transform.GetComponent<StatusBarManager>().StartStatus(StatusType.freeze, duration, "FROZEN");
        StartCoroutine(FlashColor(duration, 0.1f, Color.cyan));
        float timer = duration;
        _rb.bodyType = RigidbodyType2D.Static;
        _rb.gravityScale = 0f;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            FreezeVelocity();
            yield return new WaitForFixedUpdate();
        }
        _rb.bodyType = RigidbodyType2D.Dynamic;
        _rb.gravityScale = 0f;
    }








}

