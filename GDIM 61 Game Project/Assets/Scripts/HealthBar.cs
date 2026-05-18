using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] private float _healthBarHeight = 1;


    private float _currentHealth;
    private float _maxHealth;
    private Pet _currentPet;

    private float _standardHealthPointSize = 15;
    private Vector3 _originalScale;

    // Start is called before the first frame update
    void Start()
    {
        _originalScale = _barCanvas.transform.localScale;

        SetBarColor();

        _healthBar.maxValue = _maxHealth;
        _healthBar.value = _currentHealth;

        _lazyBar.maxValue = _maxHealth;
        _lazyBar.value = _currentHealth;

        UpdateBarScales();
    }

    // Update is called once per frame
    void Update()
    {

        _barCanvas.transform.position = transform.position + new Vector3(0, _healthBarHeight, 0);
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

    public void HideHealthBar()
    {
        _barCanvas.SetActive(false);
    }

    public void UpdateBarScales()
    {

        float barScale = (_currentPet.maxHealthPoints - _standardHealthPointSize) * 1/(_standardHealthPointSize * 2)  + 1f;

        _healthBar.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(barScale, 0.25f);
        _lazyBar.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(barScale, 0.25f);
        _barBackground.transform.localScale = new Vector3(barScale, 1, 1);
    }

    public void SetBarColor()
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

    }
    public IEnumerator TempSizeChange(float easeIn, float easeOut, float scaleIncrease)
    {
        float duration = easeIn;
        while (duration > 0)
        {
            duration -= Time.fixedDeltaTime;
            _barCanvas.transform.localScale = Vector3.Lerp(_originalScale, new Vector3(_originalScale.x + scaleIncrease, _originalScale.y + scaleIncrease, 0), 1 - duration/easeIn);
            yield return new WaitForFixedUpdate();
        }

        duration = easeOut;
        Vector3 newScale = transform.localScale;
        while (duration > 0)
        {
            duration -= Time.fixedDeltaTime;
            _barCanvas.transform.localScale = Vector3.Lerp(newScale, _originalScale, 1 - duration / easeOut);
            yield return new WaitForFixedUpdate();
        }
    }
    
}
