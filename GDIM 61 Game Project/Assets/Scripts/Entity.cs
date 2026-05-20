using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : Movable // Pet and Rectangle inherits from this
{
    [SerializeField] public float healthPoints;
    [SerializeField] protected SpriteRenderer _sprite;
    public SpriteRenderer _getSprite;

    protected Color originalColor;
    protected void SetColor()
    {
        originalColor = _sprite.color;
    }

    public virtual void ReceiveDamage(float damage)
    {
        //damage sfx
        float previousHealthPoints = healthPoints;
        healthPoints -= damage;
        if (healthPoints <= 0)
        {
            StartCoroutine(FadeAway(0.5f));
        }


        if (damage > 0)
        {
            StartCoroutine(FlashColor(0.1f, 0.1f, Color.red));
            transform.GetComponent<HealthBar>().StartCoroutine(transform.GetComponent<HealthBar>().TempSizeChange(0.15f, 0.15f, 0.5f));
        }

    }


    public virtual IEnumerator FlashColor(float easeInDuration, float easeOutDuration, Color newColor)
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

    protected virtual IEnumerator FadeAway(float duration)
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

    public SpriteRenderer GetSprite()
    {
        _getSprite = GetComponent<SpriteRenderer>();
        return _getSprite;
    }

}
