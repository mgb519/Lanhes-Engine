using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapTransition : MonoBehaviour
{
    public string sceneToLoad;
    public string entrypoint;
    GameObject player;


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }


    //TODO specify a spawn point
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject == player)
        {
            GameSceneManager.StartLoadScene(sceneToLoad,entrypoint);
        }
    }
}
