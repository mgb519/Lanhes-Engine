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
        string translated = LocalizationSettings.StringDatabase.GetLocalizedString(data ? "SYSTEM_YES" : "SYSTEM_NO");
        if (LocalizationSettings.SelectedLocale.Metadata.GetMetadata<IsRTL>().isRTL) {
            translated = RTLTranslation.RTLIfy(translated);
            button.GetComponentInChildren<TextMeshProUGUI>().isRightToLeftText = true;
        }
        button.GetComponentInChildren<TextMeshProUGUI>().text = translated;
        button.dat = this;
        return button;
    }
}
