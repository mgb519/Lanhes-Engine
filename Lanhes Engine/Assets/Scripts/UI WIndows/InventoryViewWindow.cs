using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class InventoryViewWindow : MenuWindow
{
    public SelectionButton buttonTemplate;

    public Transform contentWindow;


    //Set this when you spawn one.
    public Inventory inventory;

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

            SelectionButton button = item.Render();
            button.gameObject.transform.SetParent(GameObject.Find("Content").transform);

        }
    }




}
