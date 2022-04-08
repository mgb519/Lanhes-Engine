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
        string translated = LocalizationSettings.StringDatabase.GetLocalizedString(data);
        if (LocalizationSettings.SelectedLocale.Metadata.GetMetadata<IsRTL>().isRTL) {
            translated = RTLTranslation.RTLIfy(translated);
            button.GetComponentInChildren<TextMeshProUGUI>().isRightToLeftText = true;
        }
        button.GetComponentInChildren<TextMeshProUGUI>().text = translated; 
        button.dat = this;
        return button;
    }
}
