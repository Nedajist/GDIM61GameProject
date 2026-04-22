using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
public enum GameState
{
    BuyPhase, // User is still buying stuff
    PreCombat, // User has pressed the "commence" button
    PreAttack, // Combat has started, two pets are preparing to attack each other 
    Attack, // two pets are simultaneously attacking now
    PostAttack, // both pets have finished attacking
    PostCombat // a victor has emerged. One team has been eliminated. This will transition into buy phase. 
}

public class GameController : MonoBehaviour // this is a Singleton 
{
    public GameState currentGameState;

    [SerializeField] public List<GameObject> playerTeamList = new List<GameObject>(); // all pet classes can access this through GameController.instance.playerTeamList
    [SerializeField] public List<GameObject> playerShopList = new List<GameObject>(); // all pet classes can access this through GameController.instance.playerShopList
    [SerializeField] public List<GameObject> enemyTeamList = new List<GameObject>(); // all pet classes can access this through GameController.instance.enemyTeamList
    public static GameController instance = null;

    private float _secondsPassed = 0;
    private float _delayBetweenCombatPhases = 0.33f;
    public Vector3[] playerPositionList = { new Vector3(-0.5f, 0, 0), new Vector3(-1.8f, 0, 0), new Vector3(-3.2f, 0, 0), new Vector3(-4.62f, 0, 0), new Vector3(-6.08f, 0, 0), new Vector3(-7.37f, 0, 0) };
    public Vector3[] playerShopPositionList = { new Vector3(-1.0f, -2.5f, 0), new Vector3(-2.3f, -2.5f, 0), new Vector3(-3.7f, -2.5f, 0), new Vector3(-5.12f, -2.5f, 0), new Vector3(-6.58f, -2.5f, 0), new Vector3(-7.87f, -2.5f, 0) };
    public Vector3[] enemyPositionList = { new Vector3(1.05f, 0, 0), new Vector3(2.59f, 0, 0), new Vector3(4.03f, 0, 0), new Vector3(5.22f, 0, 0), new Vector3(6.24f, 0, 0), new Vector3(7.3f, 0, 0) };
    public int balance = 15;
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
        CullLists(playerTeamList, "player");
        CullLists(enemyTeamList, "enemy");

        Pet player_pet = playerTeamList[0].GetComponent<Pet>();
        Pet enemy_pet = enemyTeamList[0].GetComponent<Pet>();

        Debug.Log("LEAVING" + currentGameState.ToString());
        switch (newGameState){
            case GameState.BuyPhase:
                currentGameState = GameState.BuyPhase;
                for (int i = 0; i < playerShopList.Count; i++)
                {
                    playerShopList[i].transform.position = playerShopPositionList[i];
                }
                break;
            case GameState.PreCombat:
                currentGameState = GameState.PreCombat;

                PruneAllyTeamList();

                foreach (GameObject pet in playerTeamList)
                {
                    pet.GetComponent<Pet>().ally = true;
                }

                foreach (GameObject pet in enemyTeamList)
                {
                    pet.GetComponent<Pet>().ally = false;
                }



                Debug.Log("ENTERING PRECOMBAT");
                // player_pet.EnterPreCombat(); NOTE: since there are no pet abilities which activate pre-combat, I've removed those methods. 
                // enemy_pet.EnterPreCombat();

                break;

            case GameState.PreAttack:
                currentGameState = GameState.PreAttack;
                //Debug.Log("ENTERING PREATTACK");
                player_pet.EnterPreAttack();
                enemy_pet.EnterPreAttack();

                for (int i = 1; i <playerTeamList.Count; i++)
                {
                    playerTeamList[i].GetComponent<Pet>().AllyPetEnterPreAttack();
                }

                for (int i = 1; i < enemyTeamList.Count; i++)
                {
                    enemyTeamList[i].GetComponent<Pet>().AllyPetEnterPreAttack();
                }


                break;

            case GameState.Attack:
                currentGameState = GameState.Attack;
                //Debug.Log("ENTERING ATTACK");


                if (player_pet.frozen_turns == 0) // ensures pets aren't frozen before having them attack each other
                {

                    for (int i = 1; i < playerTeamList.Count; i++)
                    {
                        playerTeamList[i].GetComponent<Pet>().AllyPetEnterAttack();
                    }
                    player_pet.EnterAttack();
                    enemy_pet.ReceiveDamage(player_pet.attack, player_pet);
                }

                if (enemy_pet.frozen_turns == 0)
                {
                    for (int i = 1; i < enemyTeamList.Count; i++)
                    {
                        enemyTeamList[i].GetComponent<Pet>().AllyPetEnterAttack();
                    }
                    enemy_pet.EnterAttack();
                    player_pet.ReceiveDamage(enemy_pet.attack, enemy_pet);
                }


                break;

            case GameState.PostAttack:
                currentGameState = GameState.PostAttack;
                //Debug.Log("ENTERING POSTATTACK");
                player_pet.EnterPostAttack();
                enemy_pet.EnterPostAttack();

                for (int i = 1; i < playerTeamList.Count; i++)
                {
                    playerTeamList[i].GetComponent<Pet>().AllyPetEnterPostAttack();
                }

                for (int i = 1; i < enemyTeamList.Count; i++)
                {
                    enemyTeamList[i].GetComponent<Pet>().AllyPetEnterPostAttack();
                }

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
        _secondsPassed += Time.deltaTime;
        if (_secondsPassed > _delayBetweenCombatPhases)
        {
            _secondsPassed = 0;
            switch (currentGameState)
            {
                case GameState.PreCombat:
                    TransitionGameState(GameState.PreAttack);
                    break;
                case GameState.PreAttack:
                    TransitionGameState(GameState.Attack);
                    break;
                case GameState.Attack:
                    TransitionGameState(GameState.PostAttack);
                    break;
                case GameState.PostAttack:
                    if (playerTeamList.Count == 0 || enemyTeamList.Count == 0)
                    {
                        TransitionGameState(GameState.PostCombat);

                    }
                    else
                    {
                        TransitionGameState(GameState.PreAttack);
                    }
                    break;
            }
        }
    }
    void CullLists(List<GameObject> petList, string side) // removes dead pets 
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

        if (side == "player")
        {
            for (int i = 0; i < petList.Count; i++)
            {
                if (petList[i] == null)
                {
                    continue;
                }
                else
                {
                    petList[i].transform.position = playerPositionList[i];
                }
            }
        }

        if (side == "enemy")
        {
            for (int i = 0; i < petList.Count; i++)
            {
                if (petList[i] == null)
                {
                    continue;
                }
                else
                {
                    petList[i].transform.position = enemyPositionList[i];
                }
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

    private void PruneAllyTeamList() //deletes placeholder pets before combat
    {
        for (int i = 0; i < playerTeamList.Count; i++)
        {
            if (i >= playerTeamList.Count)
            {
                return;
            }

            if (playerTeamList[i].GetComponent<Pet>() == null)
            {
                playerTeamList.RemoveAt(i);
                i--;
            }
        }
    }

    IEnumerator Pause(float duration)
    {
        yield return new WaitForSeconds(duration);
    }

    public void ChangeOrder(MonoBehaviour pet, int newIndex)
    {
        if(newIndex > playerTeamList.Count - 1)
            {
                return; // Invalid index, do nothing
            }
        int currentIndex = playerTeamList.IndexOf(pet.gameObject);
        if (currentIndex == -1) return;

        GameObject movedPet = playerTeamList[currentIndex];

        playerTeamList.RemoveAt(currentIndex);
        playerTeamList.Insert(newIndex, movedPet);

        if (playerTeamList.Count == 7) // deletes placeholder pet
        {
            playerTeamList.RemoveAt(6);
        }

        for (int i = 0; i < playerTeamList.Count; i++)
        {
            playerTeamList[i].transform.position = playerPositionList[i];
        }


        Debug.Log("Moved pet to position: " + (newIndex + 1));
    }
}
