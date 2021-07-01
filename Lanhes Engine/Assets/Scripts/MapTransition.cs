using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapTransition : MonoBehaviour
{
    public Scene sceneToLoad; //TODO this just asks for a handle in editor, make it so we drag an asset
    GameSceneManager manager;
    GameObject player;
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameSceneManager>();
        player = GameObject.FindGameObjectWithTag("Player");
    }


    //TODO specify a spawn point
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject == player)
        {
            Debug.Log(sceneToLoad.name);
            Debug.Log(sceneToLoad.buildIndex);
            manager.StartLoadScene(sceneToLoad.buildIndex);
        }
    }
}
