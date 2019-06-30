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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player) {
            manager.StartLoadScene(sceneToLoad);
        }
    }
}
