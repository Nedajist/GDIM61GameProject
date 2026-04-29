using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI healthText;
    [SerializeField] private TMPro.TextMeshProUGUI attackText;
    [SerializeField] private TMPro.TextMeshProUGUI abilityText;
    [SerializeField] private TMPro.TextMeshProUGUI costText;
    [SerializeField] public TMPro.TextMeshProUGUI balanceText;
    [SerializeField] GameObject _commenceBattleButton;
    [SerializeField] GameObject _petHealthIcon;
    [SerializeField] GameObject _petAttackIcon;
    [SerializeField] GameObject _petCostIcon;
    [SerializeField] GameObject _nextLevelButton;
    [SerializeField] GameObject _retryLevelButton;

    [SerializeField] public int nextLevelSceneIndex;


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
    
    public void ShowStats(float health, float attack, float cost, string ability)
    {
        healthText.text = health.ToString();
        attackText.text = attack.ToString();
        costText.text = cost.ToString();
        abilityText.text = ability;
        
        //Debug.Log("Health: " + health + " Attack: " + attack);
    }
    public void HideStats()
    {
        healthText.text = "-";
        attackText.text = "-";

        abilityText.text = "Hover over an enemy, click, and press a digit key to rearrange team.";
    }
    
    public void CommenceBattleButtonPressed()
    {
        if (GameController.instance.playerTeamList.Count > 0)
        {
            _petHealthIcon.SetActive(false);
            _petCostIcon.SetActive(false);
            _petAttackIcon.SetActive(false);
            _commenceBattleButton.SetActive(false);
        }
        GameController.instance.CommenceBattleButtonPressed();
    }

    public void PlayerWon()
    {
        _nextLevelButton.SetActive(true);
    }

    public void PlayerLost()
    {
        _retryLevelButton.SetActive(true);
    }

    public void NextLevelButtonPressed()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(nextLevelSceneIndex);
    }

    public void RetryLevelButtonPressed()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(nextLevelSceneIndex - 1);
    }

}
