using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public enum Side
{
    player, 
    ai
}

public class Pet : MonoBehaviour
{
    [SerializeField] public int healthPoints;
    [SerializeField] public int attack;
    [SerializeField] public int cost;
    [SerializeField] public float speed;
    [SerializeField] public Side petSide;
    [SerializeField] public float secondsFrozen = 0;
    [SerializeField] protected Rigidbody2D _rb;
    [SerializeField] protected SpriteRenderer _sprite;
    [SerializeField] protected GameObject petTooltipPrefab;
    [SerializeField] protected float _secondsBetweenMovement;
    public bool bought = false;
    public List<GameObject> teamList; // RELATIVE TO THIS PET
    public List<GameObject> enemyList; // RELATIVE TO THIS PETs
    public float speedMultiplier = 1f;
    public float speedBoostPerCollision = 0.2f;

    private bool _movementActivated = false;


    protected int _currentPosition = -1;
    protected string _abilityText = "Temp";
    private float timeRemaining = 0.5f;
    private Color originalColor;
    private float _movementTimer;

    
    

    public virtual void Start()
    {
        SetColor();
    }

    void Update()
    {
        /*

        NEW COMBAT
        physics based bey-blades sim
        - player clicks on pet
        - player moves cursor
        - update method sets the transform of the pet to match the cursor each frame

        */

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
        bool isClicked = false;

       
        if (hit.collider != null & petSide == Side.player && GameController.instance.currentGameState == GameState.BuyPhase)
        {
            if (hit.transform == transform)
            {
                if (Input.GetMouseButton(0))
                {
                    isClicked = true;
                    PurchaseCheck();
                }
            }
        }
        if (isClicked == true)
        {
            transform.position = mousePos;
        }

    }

    protected virtual void FixedUpdate()
    {
        if (GameController.instance.currentGameState == GameState.Combat)
        {
            _movementTimer -= Time.fixedDeltaTime;
            if (_movementTimer <= 0)
            {
                _movementTimer = _secondsBetweenMovement;
                Vector2 randomVector2 = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                randomVector2 = randomVector2.normalized;
                _rb.velocity = (randomVector2 * speed * speedMultiplier);       
            }
        }

    }



    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        Pet collidingPet = collision.transform.GetComponent<Pet>();

        _movementTimer = _secondsBetweenMovement; // resets auto move timer 
        speedMultiplier += speedBoostPerCollision;

        if (collidingPet != null) // colliding with pet confirmed
        {
            if (collidingPet.petSide != petSide)
            {
                collidingPet.ReceiveDamage(attack, transform.GetComponent<Pet>());
            }

        }


        if (GameController.instance.currentGameState == GameState.Combat) // knockback
        {
            Vector2 lineFromCollider = (Vector2)transform.position - collision.contacts[0].point;
            lineFromCollider = lineFromCollider.normalized;
            _rb.velocity = (lineFromCollider * speedMultiplier);

        }
       



    }


    public virtual void ReceiveDamage(int damage, Pet aggressor)
    {
        //damage sfx
        int previousHealthPoints = healthPoints;
        healthPoints -= damage;
        if (healthPoints <= 0)
        {
            Die();
        }
        

        if (damage > 0)
        {
            StartCoroutine(FlashColor(0.1f, 0.1f, Color.red));
        }


    }

    private void PurchaseCheck() // updates balance text, adds pet to playerteamlist 
    {
        if (bought == false)
        {
            GameController.instance.balance -= cost;
            GameController.instance.UI.balanceText.text = "Balance: " + GameController.instance.balance;
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

    protected IEnumerator FlashColor(float easeInDuration, float easeOutDuration, Color newColor)
    {
        float easeInTimer = easeInDuration;
        float easeOutTimer = easeInDuration;

        while (easeInTimer > 0)
        {
            easeInTimer -= Time.fixedDeltaTime;
            _sprite.color = Color.Lerp(originalColor, newColor, 1 - (easeInTimer / easeInDuration));
            yield return new WaitForFixedUpdate();
        }

        while (easeOutTimer > 0)
        {
            easeOutTimer -= Time.fixedDeltaTime;
            _sprite.color = Color.Lerp(newColor, originalColor, 1 - (easeOutTimer / easeOutDuration));
            yield return new WaitForFixedUpdate();
        }
        yield return null;
    }

    protected IEnumerator FadeAway(float duration)
    {
        float timer = duration;
        _rb.simulated = false; // disabled rigidbody
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            _sprite.color = Color.Lerp(_sprite.color, new Color(Color.red.r, Color.red.b, Color.red.g, timer / duration), timer / duration);
            yield return new WaitForFixedUpdate();
        }
        Destroy(gameObject);
    }




    protected void SetColor()
    {
        originalColor = _sprite.color;
    }



}

