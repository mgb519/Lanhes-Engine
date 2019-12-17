using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class SelectionWindow : MenuWindow {


    public int yStart;
    public int yOffset;

    public int xStart;
    public int xOffset;


    public delegate void HandleSelection(ISelectable element);
    private HandleSelection onSelected;


    public ISelectable selected;

    public void Refresh(List<ISelectable> elements, HandleSelection selectionHandler) {
        for (int i = 0; i < elements.Count; i++) {
            ISelectable current = elements[i];
            SelectionButton b = current.Render();
            b.gameObject.transform.SetParent(GameObject.Find("Content").transform,false);
            Debug.Log(b.transform.parent.name);
            b.transform.position = new Vector3(xStart + xOffset * i, yStart + yOffset * i, 0);
            //b.onClick.AddListener(HandleItem);
        }
        onSelected = selectionHandler;
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
