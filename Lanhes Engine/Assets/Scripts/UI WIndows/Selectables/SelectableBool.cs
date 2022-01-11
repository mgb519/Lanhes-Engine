using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SelectableBool : ISelectable
{
    public bool data;

    public SelectableBool(bool d) { data = d; }

    public SelectionButton Render() {
        SelectionButton button = WindowManager.BaseButton();
        button.GetComponentInChildren<TextMeshProUGUI>().text = data? "Yes":"No"; //TODO localisation, how to implment here? 
        button.dat = this;
        return button;
    }
}
