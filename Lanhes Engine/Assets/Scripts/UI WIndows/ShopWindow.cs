using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopWindow : MenuWindow {


    bool buying = true;

    public ShopEntryButton buttonTemplate;

    //TODO: show item costs when you hover over an item

    public List<ItemCost> buyButtons;
    public List<ItemCost> sellButtons;

    //the inventory to give and take items from; i.e the players
    public Inventory inventory;

    public Transform contentWindow;


    //public float yPadFromTop = 10;
    //public float xPadFromLeft = 10;
    //TODO: this does not follow the principles of our other selector windows, please refactor

    public void Refresh() {
        //clear all buttons
        for (int i = 0; i < contentWindow.childCount; i++) { Destroy(contentWindow.GetChild(i).gameObject); }

        if (buying) {
            foreach (ItemCost item in buyButtons) {
                Debug.Log("Making buy button:" + item.item.name);
                ShopEntryButton entry = Render(item);
                //make it child of contentWindow                
                entry.gameObject.transform.SetParent(contentWindow);
            }
        } else {
            foreach (ItemCost item in sellButtons) {
                //TODO: perhaps do show items that can be sold to this vendor but you don't have?
                if (inventory.HasItem(item.item, 1)) {
                    Debug.Log("Making sell button:" + item.item.name);
                    ShopEntryButton entry = Render(item);
                    //make it child of contentWindow
                    entry.gameObject.transform.SetParent(contentWindow);
                }
            }
        }

    }

    private ShopEntryButton Render(ItemCost data) {
        ShopEntryButton ret = Instantiate(buttonTemplate);
        ret.SetData(data);
        return ret;
    }

   
    public void Transaction(ItemCost transaction) {
        if (buying) {
            foreach (CostComponent i in transaction.costs) {

                if (!inventory.HasItem(i.costType, i.quantity)) {
                    //TODO: show rejection
                    return;
                }
            }
            if (inventory.AddItem(transaction.item)) {
                //the inventory has the required items, so take them
                foreach (CostComponent i in transaction.costs) {
                    for (int j = 0; j < i.quantity; j++) {
                        inventory.RemoveItem(i.costType);
                    }
                }
            }
        } else {
            if (!inventory.HasItem(transaction.item, 1)) {
                //This should never happen, since the buttons are made from the inventory contents
                //unless, I guess, we don't refresh the list every transaction, then we can try to sell items we sold all of
                //show rejection
                return;
            }
            inventory.RemoveItem(transaction.item);
            foreach (CostComponent i in transaction.costs) {
                for (int j = 0; j < i.quantity; j++) {
                    inventory.AddItem(i.costType);
                }
            }
            //Refresh the buttons? This will at least allow us to keep in sync with what is actually in the inventory
            Refresh();
        }
        //TODO: refresh buttons here every transaction so we can grey buttons out?


        foreach (KeyValuePair<InventoryItem, int> x in inventory.Contents()) {
            Debug.Log(x);
        }

    }


    public void GoToSellWindow() {
        Debug.Log("Going to sell...");
        buying = false;
        Refresh();
    }

    public void GoToBuyWindow() {
        Debug.Log("Going to buy...");
        buying = true;
        Refresh();
    }


}


[System.Serializable]
public class ItemCost {
    public InventoryItem item;
    public List<CostComponent> costs;
    public ItemCost(InventoryItem item, List<CostComponent> costs) {
        this.item = item;
        this.costs = costs;
    }
}


[System.Serializable]
public class CostComponent {
    public InventoryItem costType;
    public int quantity;

    public CostComponent(InventoryItem costType, int quantity) {
        this.costType = costType;
        this.quantity = quantity;
    }
}
