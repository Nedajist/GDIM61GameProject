using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    private static UIController instance;
    public static UIController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIController>();
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject();
                    instance = singletonObject.AddComponent<UIController>();
                    singletonObject.name = typeof(UIController).ToString() + " (Singleton)";
                }
            }
            return instance;
        }
    }
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
        TMPro.TextMeshProUGUI healthText = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        TMPro.TextMeshProUGUI attackText = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        healthText.text = health.ToString();
        attackText.text = attack.ToString();
        
        Debug.Log("Health: " + health + " Attack: " + attack);
    }
    
}
