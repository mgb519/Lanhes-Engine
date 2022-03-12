using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization.Settings;

public class SelectableBool : ISelectable
{
    public bool data;

    public SelectableBool(bool d) { data = d; }

    public SelectionButton Render() {
        SelectionButton button = WindowManager.BaseButton();
        button.GetComponentInChildren<TextMeshProUGUI>().text = LocalizationSettings.StringDatabase.GetLocalizedString(data ? "SYSTEM_YES" : "SYSTEM_NO");//TODO Async version...? 
        button.dat = this;
        return button;
    }
}
