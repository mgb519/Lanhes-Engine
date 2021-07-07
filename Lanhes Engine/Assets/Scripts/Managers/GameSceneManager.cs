using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager instance = null;
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

    public void StartLoadScene(string newScene)
    {
        if (isLoading)
        {
            return;
        }
        isLoading = true;

        //hoover up this scene's dialogues and store them in the inter-scene memory
        DataManager.instance.RememberDialogues();
        instance.StartCoroutine(instance.LoadScene(newScene));
    }

    private IEnumerator LoadScene(string levelId)
    {
        //Wait for Unity to finish loading the scene in the background
        AsyncOperation op = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(levelId);
        while (!op.isDone)
        {
            yield return null;
        }

        //now we have loaded the new scene, restore remembered dialogues in this scene
        DataManager.instance.RestoreDialogues();

        isLoading = false;
    }


    public static bool IsLoading()
    {
        return isLoading;
    }



}
