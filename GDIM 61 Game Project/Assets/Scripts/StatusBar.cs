using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



public class StatusBar : MonoBehaviour
{
    [SerializeField] public StatusType thisStatus;
    [SerializeField] private Slider _statusBar;
    [SerializeField] public GameObject barCanvas;
    [SerializeField] public float duration;
    [SerializeField] public TextMeshProUGUI barText;
    [SerializeField] public int statusCount = 1;
    [HideInInspector] public Vector3 targetPosition;
    private float _standardHealthPointSize = 15;
    private float _lerpDuration = 0.2f; // seconds it takes for a bar to move from its actual position to target position
    private float _lerpTimer = 0;
    private Vector3 _originalPosition;
    public Pet currentPet;
    public string defaultBarText;
    private Vector3 _originalScale;

    private void Start()
    {
        UpdateBarScales();
        UpdateDuration();
        _lerpTimer = _lerpDuration;
        _originalPosition = transform.position;
        _originalScale = transform.localScale;
    }
    public void UpdateBarScales()
    {
        float barScale = (currentPet.maxHealthPoints - _standardHealthPointSize) * 1 / (_standardHealthPointSize * 2) + 1f;

        _statusBar.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(barScale, 0.1f);
    }

    public void UpdateText()
    {
        barText.text = defaultBarText + " X" + statusCount.ToString();
        StartCoroutine(TempSizeChange(0.1f, 0.1f, 0.3f));
    }

    public void UpdateDuration()
    {
        _statusBar.maxValue = duration;
        _statusBar.value = duration;
    }

    private void Update()
    {
        duration -= Time.deltaTime;
        _statusBar.value = duration;

        transform.rotation = Quaternion.identity;

        if (Vector3.Distance(transform.position, targetPosition) >= 0.1f)
        {
            transform.position = Vector3.Lerp(_originalPosition, targetPosition, _lerpTimer / _lerpDuration);
            _lerpTimer += Time.deltaTime;
        }
        else
        {
            _lerpTimer = 0;
            _originalPosition = transform.position;
        }

    }

    public IEnumerator TempSizeChange(float easeIn, float easeOut, float scaleIncrease)
    {

        float duration = easeIn;
        while (duration > 0)
        {
            duration -= Time.fixedDeltaTime;
            transform.localScale = Vector3.Lerp(_originalScale, new Vector3(_originalScale.x + scaleIncrease, _originalScale.y + scaleIncrease, 0), 1 - duration / easeIn);
            yield return new WaitForFixedUpdate();
        }

        duration = easeOut;
        Vector3 newScale = transform.localScale;
        while (duration > 0)
        {
            duration -= Time.fixedDeltaTime;
            transform.localScale = Vector3.Lerp(newScale, _originalScale, 1 - duration / easeOut);
            yield return new WaitForFixedUpdate();
        }
    }

}
