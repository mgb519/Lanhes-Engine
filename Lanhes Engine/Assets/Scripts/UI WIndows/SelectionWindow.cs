﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class SelectionWindow : MenuWindow {

    public delegate void HandleSelection(ISelectable element);
    private HandleSelection onSelected;


    public ISelectable selected;


    [SerializeField]
    private Transform content;
    public void Refresh(List<ISelectable> elements, HandleSelection selectionHandler) {
        for (int i = 0; i < elements.Count; i++) {
            ISelectable current = elements[i];
            SelectionButton b = current.Render();
            b.gameObject.transform.SetParent(content, false); 
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
