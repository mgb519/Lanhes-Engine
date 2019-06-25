using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuWindow : MonoBehaviour {

    //TODO: access the item selected by the menu?

    public MenuWindow creator = null;

    protected void CreateWindow(MenuWindow other) {
        MenuWindow subwindow = GameObject.Instantiate(other);
        subwindow.creator = this;
        gameObject.SetActive(false);
    }

    public void CloseMenu() {
        //destroy this object, we are returning to the previous layer of menu
        GameObject.Destroy(gameObject);
    }
    private void OnDestroy() {
        if (creator != null) {
            //go to the original window
            creator.gameObject.SetActive(true);
        } else {
            //we are exiting a base window
            Time.timeScale = 1;
        }
    }

}
