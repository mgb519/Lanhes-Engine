using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LanguageSelection : MonoBehaviour
{
    // Start is called before the first frame update

    //TODO dsiplay the current langauge
    void Awake() {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale(PlayerPrefs.GetString("language"));
    }


    public void SelectLanguage(Locale selected) {
        LocalizationSettings.SelectedLocale = selected;
        PlayerPrefs.SetString("language", selected.Identifier.Code);
    }
}
