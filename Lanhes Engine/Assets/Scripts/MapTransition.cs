using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapTransition : MonoBehaviour
{
    public string sceneToLoad; 
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
            manager.StartLoadScene(sceneToLoad);
        }
    }
}
