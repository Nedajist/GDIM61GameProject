using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkAOECircle : Circle
{
    [SerializeField] private float _damage;
    [SerializeField] private float _secondsBetweenDamage;
    [SerializeField] private float _maxScale;
    [SerializeField] private float _secondsToMaxScale;
    [SerializeField] private float _pullForce;
    [SerializeField] private float _deadZoneRadius;
    [SerializeField] private float _speed;
    [SerializeField] public Vector2 direction;
    [SerializeField] public GameObject originator;

    private float _growthTimer = 0;
    private float _damageTimer = 0;

    private void Update()
    {
        _lifespan -= Time.deltaTime;
        _damageTimer -= Time.deltaTime;

        if (_sprite.transform.localScale.x < _maxScale)
        {
            _growthTimer += Time.deltaTime;
            _sprite.transform.localScale = Vector3.Lerp(Vector3.zero, new Vector3(_maxScale, _maxScale, 0), _growthTimer / _secondsToMaxScale);
    
        }

        _sprite.color = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, _lifespan / _maxLifespan);
        _rb.AddForce(_speed * direction);

        for (int i = 0; i < _listOfRecipients.Count; i++)
        {
            Movable targetEntity = _listOfRecipients[i].GetComponent<Movable>();
            if (targetEntity != null && targetEntity.transform != originator.transform)
            {
                float _pullModifier = 1;
                Vector2 lineToSelf = transform.position - targetEntity.transform.position;
                if (targetEntity.GetComponent<Pet>() != null) _pullModifier *= targetEntity.GetComponent<Pet>().speedMultiplier / 2;
                targetEntity.getRigidbody().AddForce(lineToSelf * _pullForce * _pullModifier);
            }
        }
        


        if (_damageTimer <= 0)
        {
            for (int i = 0; i < _listOfRecipients.Count; i++)
            {
                Pet targetPet = _listOfRecipients[i].GetComponent<Pet>();
                if (targetPet != null && targetPet.petSide != _circleSide)
                {
                    targetPet.ReceiveDamage(_damage);
                    targetPet.speedMultiplier *= 0.5f;
                    targetPet.speedMultiplier = Mathf.Clamp(targetPet.speedMultiplier, 0.5f, 99);
                    targetPet.GetComponent<StatusBarManager>().StartStatus(StatusType.slow, 1f, "SLOWED");
                }
            }
            _damageTimer = _secondsBetweenDamage;
        }

        if (_lifespan <= 0)
        {
            Destroy(gameObject);
        }
    }




}
