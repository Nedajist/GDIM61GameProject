using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
public enum GameState
{
    BuyPhase, // User is still buying stuff
    PreCombat, // User has pressed the "commence" button
    Combat, // Combat has started, the two sides have begun to move
    PostCombat // a victor has emerged. One team has been eliminated. This will transition into buy phase. 
}

public class GameController : MonoBehaviour // this is a Singleton 
{
    public GameState currentGameState;

    [SerializeField] public List<GameObject> playerTeamList = new List<GameObject>(); // all pet classes can access this through GameController.instance.playerTeamList
    [SerializeField] public List<GameObject> playerShopList = new List<GameObject>(); // all pet classes can access this through GameController.instance.playerShopList
    [SerializeField] public List<GameObject> enemyTeamList = new List<GameObject>(); // all pet classes can access this through GameController.instance.enemyTeamList
    [SerializeField] GameObject dividingWall;
    [SerializeField] GameObject rectangleDrawer;
    [SerializeField] bool _firstLevel;

    public PlayerData saveData;
    public static GameController instance = null;

    private float _secondsPassed = 0;
    private float _delayBetweenCombatPhases = 0.33f;
    private Vector3[] playerShopPositionList = { new Vector3(-1.0f, -2.5f, 0), new Vector3(-2.3f, -2.5f, 0), new Vector3(-3.7f, -2.5f, 0), new Vector3(-5.12f, -2.5f, 0), new Vector3(-6.58f, -2.5f, 0), new Vector3(-7.87f, -2.5f, 0),
                                                 new Vector3(-1.0f, -4.5f, 0), new Vector3(-2.3f, -4.5f, 0), new Vector3(-3.7f, -4.5f, 0), new Vector3(-5.12f, -4.5f, 0), new Vector3(-6.58f, -4.5f, 0), new Vector3(-7.87f, -4.5f, 0)};
    public int levelCompleteCoinBonus = 5;
    public int buildingBalance = 3;

    public UIController UI;

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

        if (_firstLevel) saveData.ResetEverything();

        saveData.InstantiateSavedPlayerTeam();

        UI = GameObject.FindAnyObjectByType<UIController>();
        TransitionGameState(GameState.BuyPhase);

    }


    public void CommenceBattleButtonPressed()
    {
        if (currentGameState == GameState.BuyPhase && playerTeamList.Count > 0)
        {
            TransitionGameState(GameState.PreCombat);
        }
    }

    private void TransitionGameState(GameState newGameState)
    {

        Debug.Log("Switching from " + currentGameState + " to " + newGameState);
        switch (newGameState){
            case GameState.BuyPhase:
                currentGameState = GameState.BuyPhase;


                for (int i = 0; i < playerShopList.Count; i++)
                {
                    playerShopList[i].transform.position = playerShopPositionList[i];
                }

                for (int i = 0; i < enemyTeamList.Count; i++)
                {
                    enemyTeamList[i].transform.GetComponent<Pet>().FaceLeft();
                }


                break;
            case GameState.PreCombat:
                currentGameState = GameState.PreCombat;
                dividingWall.SetActive(false);
                UIController.Instance.HideStats();

                foreach (GameObject pet in playerTeamList)
                {
                    pet.GetComponent<Pet>().petSide = Side.player;
                    pet.GetComponent<Pet>().teamList = playerTeamList;
                    pet.GetComponent<Pet>().enemyList = enemyTeamList;
                    pet.GetComponent<HealthBar>().ShowHealthBar();
                }

                foreach (GameObject pet in enemyTeamList)
                {
                    pet.GetComponent<Pet>().petSide = Side.ai;
                    pet.GetComponent<Pet>().teamList = enemyTeamList;
                    pet.GetComponent<Pet>().enemyList = playerTeamList;
                    pet.GetComponent<HealthBar>().ShowHealthBar();
                }

                foreach (GameObject pet in playerShopList)
                {
                    Destroy(pet);
                }

                TransitionGameState(GameState.Combat);

                break;

            case GameState.Combat:
                currentGameState = GameState.Combat;
                rectangleDrawer.SetActive(true);
                break;

            case GameState.PostCombat:
                currentGameState = GameState.PostCombat;
                //Debug.Log("ENTERING POSTCOMBAT");
                playerTeamList = null;
                enemyTeamList = null;
                break;

        }

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        
    }
    public void CullLists(List<GameObject> petList) // removes dead pets 
    {
        int deathCount = 0;

        if (petList.Count == 0)
        {
            return;
        }

        for (int i =0; i < petList.Count; i++)
        {
            if (currentGameState == GameState.BuyPhase)
            {
                break;
            }
            else if (petList[i] == null || petList[i].GetComponent<Pet>().healthPoints <= 0)
            {
                petList.RemoveAt(i);
                i -= 1;
                deathCount += 1;
            }
        }

        
        for (int x = 0; x < deathCount; x++) // alerts all pets of a team that an ally has fallen
        {
            //Debug.Log("all pets alerted of death");
            for (int i = 0; i < petList.Count; i++)
            {
                petList[i].GetComponent<Pet>().AllyDied();
            }
        }
    }



    public void CheckForVictor()
    {
        if (playerTeamList.Count == 0) // even if its a draw, the player still loses 
        {
            Time.timeScale = 0;
            UIController.Instance.PlayerLost();
        }

        else if (enemyTeamList.Count == 0)
        {
            Time.timeScale = 0;
            UIController.Instance.PlayerWon();
            saveData.LevelBeaten();
        }

    }



}
