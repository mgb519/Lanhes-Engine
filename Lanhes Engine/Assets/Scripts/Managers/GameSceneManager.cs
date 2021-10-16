using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour, ISaveable //TODO: does this need to be Saveable?
{
    private static GameSceneManager instance = null;
    enum gameState { MENU, WORLD, BATTLE } //TODO maybe the functionality of isLoading could be moved here?
    static gameState GameState;



    private static LoadingScreen loadingGroup;

    private static bool isLoading;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            loadingGroup = transform.GetChild(0).GetComponent<LoadingScreen>(); //TODO more robust method for getting the loading screen!
            loadingGroup.gameObject.SetActive(false); //We don't need a loading screen on startup
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public static void StartLoadScene(string newScene, string spawnPositionName = null)
    {
        if (isLoading)
        {
            return;
        }
        isLoading = true;

        //hoover up this scene's dialogues and store them in the inter-scene memory

        //show loading screen
        loadingGroup.gameObject.SetActive(true);

        DataManager.instance.RememberNPCStates();
        instance.StartCoroutine(instance.LoadScene(newScene, spawnPositionName));
    }

    private IEnumerator LoadScene(string levelId, string spawnPositionName = null)
    {
        //Wait for Unity to finish loading the scene in the background
        AsyncOperation op = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(levelId);       
        while (!op.isDone)
        {
            loadingGroup.UpdateProgress(op.progress);
            yield return null;
        }


        if (spawnPositionName != null)
        {
            //we have been supplied a spawn point name. This means we were called  by a scene transition (rather than a load) and thus need to spawn the player at the correct position.
            PartyManager.SpawnPlayer(spawnPositionName);
        }


        //now we have loaded the new scene, restore remembered dialogues in this scene
        DataManager.instance.RestoreNPCStates();

        //remove the loading screen
        loadingGroup.gameObject.SetActive(false);

        isLoading = false;
    }



    public static bool IsLoading()
    {
        return isLoading;
    }

    public JObject SaveToFile()
    {
        JObject ret = new JObject();


        return ret;
    }

    public void LoadFromFile(JObject node)
    {
        //nothing is saved, so nothing is loaded!
    }

    public static void GoToGameOver() {
        StartLoadScene("Scenes/GameOver");//We are assumign this is always the name of the Game Over scene. This may not always be the case.
    }


    public static void GoToMain() {
        StartLoadScene("Scenes/Title");//We are assumign this is always the name of the main menu scene. This may not always be the case.
    }
}
