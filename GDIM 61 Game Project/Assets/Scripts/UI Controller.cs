using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController instance = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    public void ShowStats(int health, int attack)
    {
        TMPro.TextMeshProUGUI healthText = GetComponent<TMPro.TextMeshProUGUI>();
        TMPro.TextMeshProUGUI attackText = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        healthText.text = health.ToString();
        attackText.text = attack.ToString();
        
        Debug.Log("Health: " + health + " Attack: " + attack);
    }
}
