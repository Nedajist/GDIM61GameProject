using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData")] 
public class PlayerData : ScriptableObject // this is a scriptable object which contains player data that persists BETWEEN scene transitions AND 
{
    public List<string> playerSavedTeamList;
    public List<string> playerTempTeamList; // gets saved to playerSavedTeamList AFTER successfully beating a level 

    public List<GameObject> allPetsList;
    public int savedPlayerCoinBalance = 0; // the true player coin balance. Only changes after completing a level, and at the start of the very first level.
    public int playerCoinBalance = 0; // player coin balance carries over between scenes. Every level grants them +X coins + all leftover coins 
    public int startingPlayerBalance = 20;
    private Vector3 startingSpawnCoordinate = new Vector3(-6.51f, 3.51f, 0);

    public void ResetEverything()
    {
        playerTempTeamList = new List<string>();
        playerSavedTeamList = new List<string>();
        playerCoinBalance = startingPlayerBalance;
        savedPlayerCoinBalance = startingPlayerBalance;
        UIController.Instance.UpdateCoinBalanceText();
    }

    public void LevelBeaten()
    {
        foreach (string petName in playerTempTeamList)
        {
            playerSavedTeamList.Add(petName);
        }
        playerTempTeamList.Clear();
        playerCoinBalance += GameController.instance.levelCompleteCoinBonus;
        savedPlayerCoinBalance = playerCoinBalance;
    }

    public void ResetPlayerCoinBalance()
    {
        playerCoinBalance = savedPlayerCoinBalance;
    }

    public void ResetPlayerTempTeamList()
    {
        playerTempTeamList.Clear();
    }

    public void InstantiateSavedPlayerTeam()
    {
        Vector3 spawnCoordinates = startingSpawnCoordinate;
        int spawnCount = 0;
        foreach (string petName in playerSavedTeamList)
        {
            foreach (GameObject petObject in allPetsList) // NEVER edit petObject directly - these are the prefabs
            {
                if (petName == petObject.GetComponent<Pet>().petName)
                {
                    GameObject instantiated_pet = Instantiate(petObject, spawnCoordinates, Quaternion.identity);
                    spawnCoordinates += new Vector3(1.5f, 0, 0);
                    spawnCount += 1;
                    GameController.instance.playerTeamList.Add(instantiated_pet);
                    instantiated_pet.GetComponent<Pet>().bought = true;

                    if (spawnCount > 4)
                    {
                        spawnCount = 0;
                        spawnCoordinates += new Vector3(0, -2f, 0);
                        spawnCoordinates = new Vector3(startingSpawnCoordinate.x, spawnCoordinates.y, spawnCoordinates.z);
                    }

                }
            }

        }





    }

}