using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StringWindow : MenuWindow {

    public void Refresh(string displayMe) {

        //find text element and give it the correct text
        //TODO: spell everything out character by character, feedback  like audio, cursor, etc.
        TextMeshProUGUI text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = displayMe;
    }

    //check for the player advancing the screen by pressing input.
    public void Update() {
        if (Input.anyKeyDown) {
            CloseMenu();
        }
    }

}
