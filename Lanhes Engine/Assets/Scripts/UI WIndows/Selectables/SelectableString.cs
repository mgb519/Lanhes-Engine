using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Settings;

public class SelectableString :ISelectable
{
    public string data;

    public SelectableString(string d) {
        data = d;
    }

    public SelectionButton Render() {
        SelectionButton button = WindowManager.BaseButton();
        button.GetComponentInChildren<TextMeshProUGUI>().text = LocalizationSettings.StringDatabase.GetLocalizedString(data); 
        button.dat = this;
        return button;
    }
}
