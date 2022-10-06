using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using UnityEngine.Localization.Settings;
using UnityEngine.Localization;
using System;

public class StringWindow : MenuWindow {


    internal void Refresh(LocalizedString message) {
        ShowText(message.GetLocalizedString());
    }
    public void Refresh(string displayMe) {
        //find text element and give it the correct text
        //TODO: spell everything out character by character, feedback  like audio, cursor, etc.
        string translated = LocalizationSettings.StringDatabase.GetLocalizedString(displayMe);//TODO Async version...?
        ShowText(translated);
    }

    private void ShowText(string translated) {
        TextMeshProUGUI text = GetComponentInChildren<TextMeshProUGUI>();
        if (LocalizationSettings.SelectedLocale.Metadata.GetMetadata<IsRTL>().isRTL) {
            translated = RTLTranslation.RTLIfy(translated);
            text.isRightToLeftText = true;
        }
        text.text = translated;
    }

    //check for the player advancing the screen by pressing input.
    public void Update() {
        if (Input.anyKeyDown) {
            CloseMenu();
        }
    }

   
}
