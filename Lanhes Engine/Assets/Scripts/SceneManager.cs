using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public static SceneManager instance = null;
    enum gameState { MENU, WORLD, BATTLE }
    static gameState GameState;

    public static ScriptData globalVariables;

    private static CanvasGroup loadingGroup;

    private static bool isLoading;

    void Awake()
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
            loadingGroup = transform.GetChild(0).GetComponent<CanvasGroup>();

            globalVariables = new ScriptData();
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
    }

    public void StartLoadScene(string newScene)
    {
        if (isLoading)
            return;
        isLoading = true;
        instance.StartCoroutine(instance.LoadScene(newScene));
    }

    private IEnumerator LoadScene(int levelId)
    {
        float timer = 0f;

        while (timer < 1f)
        {
            timer += Time.deltaTime * 3f;
            loadingGroup.alpha = Mathf.Lerp(loadingGroup.alpha, 1f, timer);
            yield return new WaitForEndOfFrame();
        }
        timer = 0f;

        yield return new WaitForSeconds(1f);
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(levelId);
        }

        yield return new WaitForSeconds(1f);

        while (timer < 1f)
        {
            timer += Time.deltaTime * 3f;
            loadingGroup.alpha = Mathf.Lerp(1f, 0f, timer);
            yield return new WaitForEndOfFrame();
        }
        isLoading = false;
    }

    private IEnumerator LoadScene(string levelName)
    {
        float timer = 0f;

        while (timer < 1f)
        {
            timer += Time.deltaTime * 3f;
            loadingGroup.alpha = Mathf.Lerp(loadingGroup.alpha, 1f, timer);
            yield return new WaitForEndOfFrame();
        }
        timer = 0f;

        yield return new WaitForSeconds(1f);
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(levelName);
        }

        yield return new WaitForSeconds(1f);

        while (timer < 1f)
        {
            timer += Time.deltaTime * 3f;
            loadingGroup.alpha = Mathf.Lerp(1f, 0f, timer);
            yield return new WaitForEndOfFrame();
        }
        isLoading = false;
    }
}
