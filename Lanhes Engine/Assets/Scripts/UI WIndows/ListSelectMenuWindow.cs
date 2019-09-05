using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class ListSelectMenuWindow<T> : MenuWindow where T : class {


    public ListMenuEntryButton buttonTemplate;
    public List<T> data = new List<T>();
    public Transform contentWindow;
    public T selected = null;

    public override void Refresh() {
        //render each button
        foreach (T item in data) {
            ListMenuEntryButton listMenuEntry = Render(item);
            //make it child of conetntWindow
            listMenuEntry.gameObject.transform.SetParent(contentWindow);
            PositionButton(ref listMenuEntry, contentWindow);
        }


    }

    public virtual ListMenuEntryButton Render(T data) {
        ListMenuEntryButton ret = GameObject.Instantiate(buttonTemplate);
        ret.SetData(data);
        return ret;
    }

    public abstract void PositionButton(ref ListMenuEntryButton button, Transform contentWindow);


    //TODO: perhaps this should be a delegate? This would allow you to have windows that spawn windows that spawn windows and so on
    public void ReturnSelection(object ret) {

        if (creator != null) {
            creator.lastSelection = ret;
        } else {
            selected = (T)ret;
        }
        CloseMenu();
    }
}
