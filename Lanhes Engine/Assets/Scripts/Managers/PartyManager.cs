using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//keeps a list of the parties, each of which has its own inventory
//most games will only have one so it's a little moot
public class PartyManager : MonoBehaviour
{
    public static PartyManager instance = null;

    public List<Party> parties = new List<Party>();
    private Party partyThisScene;

    //points towards the party in this scene
    //maybe this one isn't the best place to put it?
    public PlayerController playerInThisScene;




    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        //SpawnPlayer();


    }

    void SpawnPlayer()
    {
        //TODO: choose marker based on data passed through from last scene (i.e what entrance)
        PlayerSpawnMarker spawner = GameObject.FindObjectOfType<PlayerSpawnMarker>();
        partyThisScene = parties[spawner.partyIndex];

        playerInThisScene = Instantiate(partyThisScene.avatar, gameObject.transform.position, gameObject.transform.rotation);
        playerInThisScene.gameObject.name = "Player";

        Destroy(spawner);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SpawnPlayer();
    }

    public Party GetParty()
    {
        return partyThisScene;
    }




    //TODO: serialization

}

[System.Serializable]
public class Party
{
    public Inventory inventory;
    public PlayerController avatar;
    //TODO: characters in party
}
