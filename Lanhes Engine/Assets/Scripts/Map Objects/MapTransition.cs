using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapTransition :  MapScript
{
    public string sceneToLoad;
    public string entrypoint;
    public override IEnumerator Action() {
        GameSceneManager.StartLoadScene(sceneToLoad, entrypoint);
        yield break;
    }
}
