using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapTransition : MonoBehaviour, MapScript
{
    public string sceneToLoad;
    public string entrypoint;
    public void Action() {
        GameSceneManager.StartLoadScene(sceneToLoad, entrypoint);
    }
}
