using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetTimerPair
{
    public GameObject petObject;
    public float petTimer = 0;
}

public class Shop : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] SpriteRenderer _sprite;
    [SerializeField] BoxCollider2D _collider;
    [SerializeField] float _timeToSell = 2f;
    [SerializeField] float _timeToFlash = 0.2f;


    private List<GameObject> _listOfRecipients = new List<GameObject>();
    private Dictionary<int, PetTimerPair> _recipientIDs = new Dictionary<int, PetTimerPair>();
    private float _flashTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject pet in _listOfRecipients)
        {
            if (pet == null) _listOfRecipients.Remove(pet);
        }

        foreach (KeyValuePair<int, PetTimerPair> pair in _recipientIDs)
        {
            _recipientIDs[pair.Key].petTimer -= Time.deltaTime;
            if (_recipientIDs[pair.Key].petTimer <= 0)
            {
                GameController.instance.saveData.playerCoinBalance += (_recipientIDs[pair.Key].petObject.GetComponent<Pet>().cost - 1); // sells pets for $1 less than what they're worth 
                Destroy(_recipientIDs[pair.Key].petObject);
                UIController.Instance.UpdateCoinBalanceText();
                _recipientIDs.Remove(pair.Key);
                return;
            }
        }

        _flashTimer -= Time.deltaTime;
        if (_flashTimer <= 0)
        {
            foreach (GameObject petobject in _listOfRecipients)
            {
                Pet pet = petobject.GetComponent<Pet>();
                pet.StartCoroutine(pet.FlashColor(0.1f, 0.1f, Color.red));
            }
            _flashTimer = _timeToFlash;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _listOfRecipients.Add(collision.transform.gameObject);
        PetTimerPair pair = new PetTimerPair();
        pair.petObject = collision.transform.gameObject;
        pair.petTimer = _timeToSell;
        _recipientIDs[collision.transform.GetInstanceID()] = pair;
        collision.transform.GetComponent<CircleCollider2D>().isTrigger = true;

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _listOfRecipients.Remove(collision.transform.gameObject);
        _recipientIDs[collision.transform.GetInstanceID()].petTimer = _timeToSell;
        collision.transform.GetComponent<CircleCollider2D>().isTrigger = false;
    }

}
