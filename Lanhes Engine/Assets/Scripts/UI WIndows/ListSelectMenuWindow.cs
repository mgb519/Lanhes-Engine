using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class ListSelectMenuWindow<T> : MenuWindow where T : class {


    public ListMenuEntryButton buttonTemplate;
    public List<GUIListItem<T>> data = new List<GUIListItem<T>>();

    public T selected = null;

    public void Refresh() {
        //find the Content object
        Transform contentWindow = gameObject.transform.Find("Panel");
        //render each button
        foreach (GUIListItem<T> item in data) {
            ListMenuEntryButton listMenuEntry = item.Render(buttonTemplate);
            //make it child of conetntWindow
            listMenuEntry.gameObject.transform.SetParent(contentWindow);
            PositionButton(ref listMenuEntry, contentWindow);
        }


    }

    public abstract void PositionButton(ref ListMenuEntryButton button, Transform contentWindow);

    public void ReturnSelection(ListMenuEntryButton ret) {
        foreach (GUIListItem<T> g in data) {
            if (g.button == ret) {
                if (creator != null) {
                    creator.lastSelection = g.heldItem;
                } else {
                    selected = g.heldItem;
                }

                break;
            }
        }
        CloseMenu();
    }
}
