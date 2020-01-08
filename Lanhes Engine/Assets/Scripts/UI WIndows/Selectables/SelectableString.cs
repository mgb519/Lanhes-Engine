using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SelectableString :ISelectable
{
    public string data;

    public SelectableString(string d) {
        data = d;
    }

    public SelectionButton Render() {
        SelectionButton button = WindowManager.BaseButton();
        button.GetComponentInChildren<Text>().text = data;
        button.dat = this;
        return button;
    }
}
