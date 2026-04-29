using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider _healthBar;
    [SerializeField] private Slider _lazyBar;
    [SerializeField] private float _rate_of_change;
    [SerializeField] private SpriteRenderer _barBackground;
    [SerializeField] GameObject _barCanvas;

    [SerializeField] private Image _healthBarImage;



    private float _currentHealth;
    private float _maxHealth;
    private Pet _currentPet;

    private float _standardHealthPointSize = 15;

    // Start is called before the first frame update
    void Start()
    {
        _currentPet = transform.GetComponent<Pet>();
        if (_currentPet.petSide == Side.player)
        {
            _healthBarImage.color = Color.green;

        }
        else
        {
            _healthBarImage.color = Color.yellow;
        }

        _healthBar.maxValue = _maxHealth;
        _healthBar.value = _currentHealth;

        _lazyBar.maxValue = _maxHealth;
        _lazyBar.value = _currentHealth;

        UpdateBarScales();
    }

    // Update is called once per frame
    void Update()
    {

        _barCanvas.transform.position = transform.position + new Vector3(0, 1f, 0);
        _barCanvas.transform.rotation = Quaternion.identity;

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

    public void ShowHealthBar()
    {
        _barCanvas.SetActive(true);
    }

    public void UpdateBarScales()
    {

        float barScale = (_currentPet.maxHealthPoints - _standardHealthPointSize) * 1/(_standardHealthPointSize * 2)  + 1f;

        _healthBar.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(barScale, 0.25f);
        _lazyBar.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(barScale, 0.25f);
        _barBackground.transform.localScale = new Vector3(barScale, 1, 1);
    }
}
