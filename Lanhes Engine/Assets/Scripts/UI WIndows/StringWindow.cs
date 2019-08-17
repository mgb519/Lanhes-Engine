using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StringWindow : MenuWindow {
    public string displayMe;

    public override void Refresh() {

        //find text element and give it the correct text
        //TODO: spell everything out character by character, feedback  like audio, cursor, etc.
        Text text = GetComponentInChildren<Text>();
        text.text = displayMe;
    }

    //check for the player advancing the screen by pressing input.
    public void Update() {
        if (Input.anyKeyDown) {
            CloseMenu();
        }
    }

}
