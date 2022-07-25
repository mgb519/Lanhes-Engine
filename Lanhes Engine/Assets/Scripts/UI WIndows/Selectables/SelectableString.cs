using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;

public class SelectableString :ISelectable
{
    public LocalizedString data;

    public SelectableString(LocalizedString d) {
        data = d;
    }

    public SelectionButton Render() {
        SelectionButton button = WindowManager.BaseButton();
        string translated = data.GetLocalizedString();
        if (LocalizationSettings.SelectedLocale.Metadata.GetMetadata<IsRTL>().isRTL) {
            translated = RTLTranslation.RTLIfy(translated);
            button.GetComponentInChildren<TextMeshProUGUI>().isRightToLeftText = true;
        }
        button.GetComponentInChildren<TextMeshProUGUI>().text = translated; 
        button.dat = this;
        return button;
    }
}
