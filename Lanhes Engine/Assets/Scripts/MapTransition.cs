using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTransition : MonoBehaviour
{
    public string sceneToLoad;
    SceneManager manager;
    GameObject player;
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<SceneManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        print("Check");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("Check");
        if (collision.gameObject == player) {
            print("CheckAgain");
            manager.StartLoadScene(sceneToLoad);
        }
    }
}
