using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuWindow : MonoBehaviour {

    //TODO: access the item selected by the menu?

    public MenuWindow creator = null;



    //when this is null; nothing has been selected, when this is not null, we have recieved a selection from the last called window and can continue with whatever we wanted to do with the selection
    public ISelectable lastSelection = null;

    private RectTransform hider = null;
    public T CreateWindow<T>(T other) where T:MenuWindow {
        hider = WindowManager.CreateHider();
        T subwindow = GameObject.Instantiate(other,WindowManager.instance.transform);
        subwindow.creator = this;
        return subwindow;
    }

    public void CloseMenu() {
        //destroy this object, we are returning to the previous layer of menu
        //Debug.Log("closing menu");
        GameObject.Destroy(gameObject);
    }


    /// <summary>
    /// close all the menus leading up to this; return to the game.
    /// </summary>
    public void CollapseMenu() {
        CloseMenu();
        if (creator != null) {
            creator.CollapseMenu();
        }
    }

    private void OnDestroy() {
        
        if (creator != null) {
            //go to the original window

            Destroy(creator.hider.gameObject);
        } else {
            //we are exiting a base window
            WindowManager.instance.WindowClosed();
        }
    }
    private bool CheckLastSelection() {
        Debug.Log(lastSelection);
        return lastSelection != null;
    }
    
}
