using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpawnInGameManager 
{
   [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Spawning() {
        const string PATH = "Assets/Prefabs/GameManager.prefab";
        GameObject manager = AssetDatabase.LoadAssetAtPath<GameObject>(PATH);
        GameObject.Instantiate(manager);



        //FIXME HOLY FUCK THIS IS BAD WE SPAWN IN A PLAYER JUST TO SET playerInThisScene
        PartyManager pm = manager.GetComponent<PartyManager>();
        PartyManager.SpawnPlayer(0); //TODO this is not correct for all games. Then again, this is also just for testing       
        GameObject.DestroyImmediate(PartyManager.playerInThisScene.gameObject);


        string spawnPoint = GameObject.FindObjectOfType<PlayerSpawnMarker>().name;
        PartyManager.SpawnPlayer(spawnPoint);
    }
}
