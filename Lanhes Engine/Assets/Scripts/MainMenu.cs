using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{


    public int partyID;
    public string firstScene;
    public string firstPawnPoint;
    public void NewGame()
    {
        PartyManager.SpawnPlayer(partyID);
        GameSceneManager.StartLoadScene(firstScene, firstPawnPoint);

    }
}
