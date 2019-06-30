using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuWindow : MonoBehaviour {

    //TODO: access the item selected by the menu?

    public MenuWindow creator = null;

    public StringSelectWindow stringTest;


    //when this is null; nothing has been selected, when this is not null, we have recieved a selection and can continue with whatever we wanted to do with the selection
    public object lastSelection = null;


    protected MenuWindow CreateWindow(MenuWindow other) {
        MenuWindow subwindow = GameObject.Instantiate(other);
        subwindow.creator = this;
        gameObject.SetActive(false);
        return subwindow;

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

    public void StringTest() {
        StartCoroutine(StringTestBody());
    }

    IEnumerator StringTestBody() {
        StringSelectWindow s = (StringSelectWindow)CreateWindow(stringTest);
        s.data.Add(new GUIListItem<string>("Foo"));
        s.data.Add(new GUIListItem<string>("Bar"));
        s.Refresh();
        Debug.Log("going to wait...");

        yield return new WaitUntil(() => CheckLastSelection());

        Debug.Log("The player selected:" + (string)lastSelection);

        yield break;
    }

    private bool CheckLastSelection() {
        Debug.Log(lastSelection);
        return lastSelection != null;
    }
}
