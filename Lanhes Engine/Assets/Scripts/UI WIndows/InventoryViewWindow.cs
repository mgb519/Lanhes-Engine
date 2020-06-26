using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class InventoryViewWindow : MenuWindow {
    public SelectionButton buttonTemplate;

    public Transform contentWindow;

    private float newButtonPos;
    public float yPadFromTop = 10;
    public float xPadFromLeft = 10;

    //Set this when you spawn one.
    public Inventory inventory;

    //TODO: perhaps we only need one tag in practise?
    public List<InventoryItem.Tags> filter = new List<InventoryItem.Tags>();

    public void Awake() {
        newButtonPos = yPadFromTop;
    }

    public abstract void HandleSelection(InventoryItem selected);


    private void ItemClicked(InventoryItem selected) {
        HandleSelection(selected);
        //TODO: what if this is a consumable item?
        Refresh();
    }

    public void Refresh() {
        //render each item
        foreach (InventoryItem item in inventory.Contents().Keys) {
            //check that every tag we are filtering for is present
            if (filter.TrueForAll(x => item.tags.Contains(x))) {
                SelectionButton button = item.Render();
                button.gameObject.transform.SetParent(GameObject.Find("Content").transform);
            }
        }
    }

    


}
