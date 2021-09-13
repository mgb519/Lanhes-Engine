using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
public class PauseMenu : MenuWindow
{

    void Awake() {
        Time.timeScale = 0f;
    }

    public void UnPause() {
        Time.timeScale = 1f;
        CloseMenu();
    }

    public void LoadFromFile()
    {
        WindowManager.CreateLoadWindow(this);
    }

    public void SaveGameToFile()
    {
        WindowManager.CreateSaveWindow(this);
    }

    public void GoToMain() {
        GameSceneManager.GoToMain();
        UnPause();
    }
}
