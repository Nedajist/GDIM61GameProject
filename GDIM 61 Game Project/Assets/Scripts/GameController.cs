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
    public GameState currentGameState = GameState.BuyPhase;

    [SerializeField] public List<GameObject> playerTeamList = new List<GameObject>(); // all pet classes can access this through GameController.instance.playerTeamList
    [SerializeField] public List<GameObject> enemyTeamList = new List<GameObject>(); // all pet classes can access this through GameController.instance.enemyTeamList
    public static GameController instance = null;

    private float _secondsPassed = 0;
    private float _delayBetweenCombatPhases = 0.33f;
    public Vector3[] playerPositionList = { new Vector3(-0.5f, 0, 0), new Vector3(-1.8f, 0, 0), new Vector3(-3.2f, 0, 0), new Vector3(-4.62f, 0, 0), new Vector3(-6.08f, 0, 0), new Vector3(-7.37f, 0, 0) };
    public Vector3[] enemyPositionList = { new Vector3(1.05f, 0, 0), new Vector3(2.59f, 0, 0), new Vector3(4.03f, 0, 0), new Vector3(5.22f, 0, 0), new Vector3(6.24f, 0, 0), new Vector3(7.3f, 0, 0) };

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


    public void CommenceBattleButtonPressed()
    {
        if (currentGameState == GameState.BuyPhase)
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
            case GameState.PreCombat:
                currentGameState = GameState.PreCombat;

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
                Debug.Log("ENTERING PREATTACK");
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
                Debug.Log("ENTERING ATTACK");
                player_pet.EnterAttack();
                enemy_pet.EnterAttack();
                enemy_pet.ReceiveDamage(player_pet.attack);
                player_pet.ReceiveDamage(enemy_pet.attack);

                for (int i = 1; i < playerTeamList.Count; i++)
                {
                    playerTeamList[i].GetComponent<Pet>().AllyPetEnterAttack();
                }

                for (int i = 1; i < enemyTeamList.Count; i++)
                {
                    enemyTeamList[i].GetComponent<Pet>().AllyPetEnterAttack();
                }


                break;

            case GameState.PostAttack:
                currentGameState = GameState.PostAttack;
                Debug.Log("ENTERING POSTATTACK");
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
                Debug.Log("ENTERING POSTCOMBAT");
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


        for (int i =0; i < petList.Count; i++)
        {
            if (petList[i] == null)
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
                petList[i].transform.position = playerPositionList[i];
            }
        }

        if (side == "enemy")
        {
            for (int i = 0; i < petList.Count; i++)
            {
                petList[i].transform.position = enemyPositionList[i];
            }
        }


        for (int x = 0; x < deathCount; x++) // alerts all pets of a team that an ally has fallen
        {
            for (int i = 0; i < petList.Count; i++)
            {
                petList[i].GetComponent<Pet>().AllyDied();
            }
        }



    }

    IEnumerator Pause(float duration)
    {
        yield return new WaitForSeconds(duration);
    }

    void ChangeOrder()
    {
        //player hovers over pet and clicks a digit
        //digit corresponds to list position (i - 1)
        ///////
        /// //create a reference to on mousover from pet script
        int newIndex;

        if (Input.GetKey(KeyCode.Alpha1))
        {
            newIndex = 0;
            playerTeamList[newIndex].transform.position = playerPositionList[newIndex];
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            newIndex = 1;
            playerTeamList[newIndex].transform.position = playerPositionList[newIndex];
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            newIndex = 2;
            playerTeamList[newIndex].transform.position = playerPositionList[newIndex];
        }
        else if (Input.GetKey(KeyCode.Alpha4))
        {
            newIndex = 3;
            playerTeamList[newIndex].transform.position = playerPositionList[newIndex];
        }
        else if (Input.GetKey(KeyCode.Alpha5))
        {
            newIndex = 4;
            playerTeamList[newIndex].transform.position = playerPositionList[newIndex];
        }
        else if (Input.GetKey(KeyCode.Alpha6))
        {
            newIndex = 5;
            playerTeamList[newIndex].transform.position = playerPositionList[newIndex];
        }
    }
}
