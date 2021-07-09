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

    private static CanvasGroup loadingGroup;

    private static bool isLoading;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            loadingGroup = transform.GetChild(0).GetComponent<CanvasGroup>();
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
        DataManager.instance.RememberDialogues();
        instance.StartCoroutine(instance.LoadScene(newScene, spawnPositionName));
    }

    private IEnumerator LoadScene(string levelId, string spawnPositionName = null)
    {
        //Wait for Unity to finish loading the scene in the background
        AsyncOperation op = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(levelId);
        while (!op.isDone)
        {
            yield return null;
        }


        if (spawnPositionName != null)
        {
            //we have been supplied a spawn point name. This means we were called  by a scene transition (rather than a load) and thus need to player the player at the correct position.
            PartyManager.SpawnPlayer(spawnPositionName);
        }


        //now we have loaded the new scene, restore remembered dialogues in this scene
        DataManager.instance.RestoreDialogues();

        isLoading = false;
    }


    public static bool IsLoading()
    {
        return isLoading;
    }

    public XmlNode SaveToFile(XmlDocument doc)
    {
        XmlElement ret = doc.CreateElement("gamescene");


        return ret;
    }

    public void LoadFromFile(XmlNode node)
    {
        //nothing is saved, so nothing is loaded!
    }
}
