using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MenuWindow
{

    void Awake() {
        Time.timeScale = 0f;
    }

    public void UnPause() {
        Time.timeScale = 1f;
        CloseMenu();
    }
}
