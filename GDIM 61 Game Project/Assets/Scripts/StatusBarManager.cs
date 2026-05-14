using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum StatusType
{
    stun,
    heal,
    sap,
    freeze,
    speed,
    charge,
    slow
}

public class StatusBarManager : MonoBehaviour
{
    [SerializeField] GameObject _statusBarObject;

    private Pet _currentPet;
    private Dictionary<StatusType, GameObject> _statusBarDict = new Dictionary<StatusType, GameObject>();
    [SerializeField] float _startingHeight = 1.2f;
    private float _heightPerBar = 0.4f;


    // Start is called before the first frame update
    void Start()
    {
        _currentPet = transform.GetComponent<Pet>();


    }

    public void StartStatus(StatusType type, float duration, string text)
    {
        if (_statusBarDict.ContainsKey(type))
        {
            StatusBar existingBar = _statusBarDict[type].GetComponent<StatusBar>();
            existingBar.statusCount += 1;
            existingBar.UpdateText();

            existingBar.duration += duration;
            existingBar.UpdateDuration();

        }

        else
        {

            GameObject instantiatedStatusBar = Instantiate(_statusBarObject, _currentPet.transform.position, Quaternion.identity, _currentPet.transform);
            instantiatedStatusBar.transform.localScale = new Vector3(0.8f / transform.localScale.x, 0.8f / transform.localScale.y, 0);
            StatusBar bar = instantiatedStatusBar.GetComponent<StatusBar>();
            bar.duration = duration;
            bar.thisStatus = type;
            bar.currentPet = transform.GetComponent<Pet>();
            bar.barText.text = text;
            bar.defaultBarText = text;

            _statusBarDict[type] = instantiatedStatusBar;
            //instantiatedStatusBar.transform.position = _currentPet.transform.position + new Vector3(0, _startingHeight + _heightPerBar * _statusBarDict.Count, 0);
        }

    }

    // Update is called once per frame
    void Update()
    {
        float barCount = 0;
        foreach (KeyValuePair<StatusType, GameObject> pair in _statusBarDict)
        {
            StatusBar bar = pair.Value.GetComponent<StatusBar>();
            bar.targetPosition = _currentPet.transform.position + new Vector3(0, _startingHeight + _heightPerBar * barCount, 0);

            barCount += 1;
            if (bar.duration <= 0)
            {
                Destroy(pair.Value);
                _statusBarDict.Remove(pair.Key);
                return;
            }


        }





    }
}
