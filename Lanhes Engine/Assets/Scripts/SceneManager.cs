using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour {
    public static SceneManager instance = null;
    enum gameState { MENU, WORLD, BATTLE }
    static gameState GameState;

    private static CanvasGroup loadingGroup;

    private static bool isLoading;



    public StringSelectWindow stringSelectWindow;
    public StringWindow stringWindow;
    public ShopWindow shopWindow;

    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
            loadingGroup = transform.GetChild(0).GetComponent<CanvasGroup>();
        } else if (instance != this) {
            Destroy(gameObject);
        }
    }

    public void StartLoadScene(string newScene) {
        if (isLoading)
            return;
        isLoading = true;
        instance.StartCoroutine(instance.LoadScene(newScene));
    }

    private IEnumerator LoadScene(int levelId) {
        float timer = 0f;

        while (timer < 1f) {
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

        while (timer < 1f) {
            timer += Time.deltaTime * 3f;
            loadingGroup.alpha = Mathf.Lerp(1f, 0f, timer);
            yield return new WaitForEndOfFrame();
        }
        isLoading = false;
    }

    private IEnumerator LoadScene(string levelName) {
        float timer = 0f;

        while (timer < 1f) {
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

        while (timer < 1f) {
            timer += Time.deltaTime * 3f;
            loadingGroup.alpha = Mathf.Lerp(1f, 0f, timer);
            yield return new WaitForEndOfFrame();
        }
        isLoading = false;
    }




    private static MenuWindow CreateWindow(MenuWindow other) {
        MenuWindow subwindow = GameObject.Instantiate(other);
        return subwindow;
    }

    public static StringWindow CreateStringWindow(string text) {
        StringWindow dialog = (StringWindow)CreateWindow(instance.stringWindow);
        dialog.displayMe = text;//TODO: have a fucntion to update text which auto-refreshes; if might see use in spelling dialogue out
        dialog.Refresh();
        Time.timeScale = 0;
        return dialog;
    }

    public static StringSelectWindow CreateStringSelectWindow(List<string> options) {
        StringSelectWindow dialog = (StringSelectWindow)CreateWindow(instance.stringSelectWindow);
        foreach (string i in options) {
            dialog.data.Add(i);
        }
        dialog.Refresh();
        Time.timeScale = 0;
        return dialog;
    }

    public static ShopWindow CreateShopWindow(List<ItemCost> forSale, Inventory playerInventory) {
        Debug.Log("Creating shop window");
        ShopWindow window = (ShopWindow)CreateWindow(instance.shopWindow);
        foreach (ItemCost item in forSale) {
            //TODO: create item button
            window.shopButtons.Add(item);
        }

        window.inventory = playerInventory;
        window.Refresh();
        Time.timeScale = 0;
        return window;

    }

}
