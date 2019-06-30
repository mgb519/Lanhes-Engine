using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GUIListItem<T> {
    public T heldItem;
    public ListMenuEntryButton button;

    public GUIListItem(T t) {
        heldItem = t;
    }


    public ListMenuEntryButton Render(ListMenuEntryButton template) {
        ListMenuEntryButton ret = GameObject.Instantiate(template);
        ret.SetName(heldItem.ToString());
        button = ret;
        return ret;
    }
}
