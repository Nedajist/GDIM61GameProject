using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI healthText;
    [SerializeField] private TMPro.TextMeshProUGUI attackText;
    [SerializeField] private TMPro.TextMeshProUGUI abilityText;
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
    
    public void ShowStats(int health, int attack, string ability)
    {
        healthText.text = health.ToString();
        attackText.text = attack.ToString();

        abilityText.text = ability;
        
        Debug.Log("Health: " + health + " Attack: " + attack);
    }
    public void HideStats()
    {
        healthText.text = "-";
        attackText.text = "-";

        abilityText.text = "Hover over an enemy, click, and press a digit key to rearrange team.";
    }
    
}
