using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;

public partial class SelectionWindow : MenuWindow {

    public delegate void HandleSelection(ISelectable element);
    private HandleSelection onSelected;


    public ISelectable selected;


    [SerializeField]
    private Transform content;

    [SerializeField]
    private TextMeshProUGUI promptBox;
    public void Refresh(List<ISelectable> elements, HandleSelection selectionHandler, string prompt) {


        string translated = LocalizationSettings.StringDatabase.GetLocalizedString(prompt);//TODO Async version...?
        translated = RenderChoices(elements, selectionHandler, translated);
    }

    public void Refresh(List<ISelectable> elements, HandleSelection selectionHandler, LocalizedString prompt) {
        string translated = prompt.GetLocalizedString();//TODO Async version...?
        translated = RenderChoices(elements, selectionHandler, translated);
    }

    private string RenderChoices(List<ISelectable> elements, HandleSelection selectionHandler, string translated) {
        for (int i = 0; i < elements.Count; i++) {
            ISelectable current = elements[i];
            SelectionButton b = current.Render();
            b.gameObject.transform.SetParent(content, false);
        }
        onSelected = selectionHandler;

        if (LocalizationSettings.SelectedLocale.Metadata.GetMetadata<IsRTL>().isRTL) {
            translated = RTLTranslation.RTLIfy(translated);
            promptBox.isRightToLeftText = true;
        }
        promptBox.text = translated;
        return translated;
    }

    public void HandleItem(ISelectable lastElement) {
        onSelected(lastElement);
    }

    public void ReturnAndClose(ISelectable element) {

        if (creator != null) {
            creator.lastSelection = element;
        } else {
            selected = element;
        }

        CloseMenu();

    }






}
