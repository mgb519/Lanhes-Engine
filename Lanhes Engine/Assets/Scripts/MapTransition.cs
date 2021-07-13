using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapTransition : MonoBehaviour
{
    public string sceneToLoad;
    public string entrypoint;
  
    
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject == PartyManager.playerInThisScene.gameObject)
        {
            GameSceneManager.StartLoadScene(sceneToLoad,entrypoint);
        }
    }
}
