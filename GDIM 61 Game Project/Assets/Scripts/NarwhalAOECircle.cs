using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarwhalAOECircle : Circle
{
    [SerializeField] private float _damage;
    [SerializeField] private float _maxScale;

    private void Update()
    {
        _sprite.transform.localScale = Vector3.Lerp(Vector3.zero, new Vector3(_maxScale, _maxScale, 0), 1 - _lifespan / _maxLifespan);
        _sprite.color = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, 1 - _lifespan / _maxLifespan);
        _lifespan -= Time.deltaTime;
        if (_lifespan <= 0)
        {
            for (int i = 0; i < _listOfRecipients.Count; i++)
            {
                Pet targetPet = _listOfRecipients[i].GetComponent<Pet>();
                if (targetPet != null && targetPet.petSide != _circleSide)
                {
                    targetPet.ReceiveDamage(_damage);
                }
            }
            Destroy(gameObject);
        }
    }
}
