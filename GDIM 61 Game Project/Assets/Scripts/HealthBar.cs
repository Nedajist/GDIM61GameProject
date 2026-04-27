using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider _healthBar;
    [SerializeField] private Slider _lazyBar;
    [SerializeField] private float _rate_of_change;
    

    private float _currentHealth;
    private float _maxHealth;
    private Pet _currentPet;


    // Start is called before the first frame update
    void Start()
    {
        _currentPet = transform.GetComponent<Pet>();
        if (_currentPet.petSide == Side.player)
        {
            _healthBar.image.color = Color.green;

        }
        else
        {
            _healthBar.image.color = Color.yellow;
        }

        _healthBar.maxValue = _maxHealth;
        _healthBar.value = _currentHealth;

        _lazyBar.maxValue = _maxHealth;
        _lazyBar.value = _currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        _currentHealth = _currentPet.healthPoints;
        _maxHealth = _currentPet.maxHealthPoints;

        _healthBar.value = _currentHealth;
        _healthBar.maxValue = _maxHealth;

        _lazyBar.maxValue = _maxHealth;

        if (_lazyBar.value > _healthBar.value)
        {
            _lazyBar.value -= _rate_of_change * Time.deltaTime;
        }
        if (_lazyBar.value < _healthBar.value)
        {
            _lazyBar.value = _healthBar.value;
        }
    }
}
