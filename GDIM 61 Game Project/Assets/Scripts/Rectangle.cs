using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rectangle : Entity
{
    [SerializeField] BoxCollider2D _collider;
    [SerializeField] float _maxArea;
    public bool placed = false;
    public bool placeable = false;
    private float maxHealthPoints;

    // Update is called once per frame
    private void Start()
    {
        SetColor();
        maxHealthPoints = healthPoints;
    }
    public void Place()
    {
        _sprite.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1);
        originalColor = _sprite.color;
        _collider.isTrigger = false;
        placed = true;
    }
    public override void ReceiveDamage(float damage)
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
            originalColor = new Color(originalColor.r, originalColor.g, originalColor.b, healthPoints / maxHealthPoints);
            _sprite.color = originalColor;
            StartCoroutine(FlashColor(0.1f, 0.1f, Color.red));
        }

    }
    private void Update()
    {
        if (placed == false)
        {
            List<Collider2D> hits = new List<Collider2D>();
            ContactFilter2D filter = new ContactFilter2D();
            filter.NoFilter();

            Physics2D.OverlapCollider(_collider, filter, hits);



            if (hits.Count != 0 || Mathf.Abs(transform.localScale.x) * Mathf.Abs(transform.localScale.y) > _maxArea)
            {
                _sprite.color = new Color(Color.red.r, Color.red.g, Color.red.b, originalColor.a);
                placeable = false;
            }
            else
            {
                _sprite.color = originalColor;
                placeable = true;
            }
        }
    }



}
