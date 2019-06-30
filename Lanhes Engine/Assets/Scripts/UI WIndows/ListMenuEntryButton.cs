using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ListMenuEntryButton : MonoBehaviour {
    private Button button;

    // Start is called before the first frame update
    void Awake() {
        button = gameObject.GetComponent<Button>();
    }

    public void SetName(string name) {
        button.GetComponentInChildren<Text>().text = name;
    }

    public void OptionSelected() {
        //get the ListSelectMenuWindow, and call the appropriate function on it that an option has been selected
        //well since generics fuck that up lets use a sendMessage call
        SendMessageUpwards("ReturnSelection", this);


        //ListSelectMenuWindow<type> listWindow = GetComponentInParent<ListSelectMenuWindow<type>>();
        //listWindow.ReturnSelection(this);

    }
}
