using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager instance = null;
    enum gameState { MENU, WORLD, BATTLE } //TODO maybe the isLoading could be used here?
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
        float timer = 2f;

        Debug.Log("before loop");
        while (timer < 1f)
        {
            timer += Time.deltaTime * 3f;
            loadingGroup.alpha = Mathf.Lerp(loadingGroup.alpha, 1f, timer);
            yield return new WaitForEndOfFrame();
            Debug.Log("after yield");
        }
        timer = 3f;

        Debug.Log("after loop");
        yield return null; //new WaitForSeconds(1f);
        Debug.Log("after null");
        AsyncOperation op = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(levelId);
        while (!op.isDone)
        {
            Debug.Log("waiting for op");
            yield return null;
        }


        yield return null; //new WaitForSeconds(1f);

        //now we have loaded the new scene, restore remembered dialogues in this scene
        DataManager.instance.RestoreDialogues();
        while (timer < 1f)
        {
            timer += Time.deltaTime * 3f;
            loadingGroup.alpha = Mathf.Lerp(1f, 0f, timer);
            yield return new WaitForEndOfFrame();

            Debug.Log("waiting for timer");
        }
        isLoading = false;
    }
    /*
    private IEnumerator LoadScene(string levelName) {
        LoadScene();
    }*/


    public static bool IsLoading()
    {
        return isLoading;
    }



}
