using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopWindow : MenuWindow {


    public ShopEntryButton buttonTemplate;

    //TODO: show item costs when you hover over an item
    public List<ItemCost> shopButtons;

    public Inventory inventory;
    public Transform contentWindow;


    public float yPadFromTop = 10;
    public float xPadFromLeft = 10;


    public override void Refresh() {


        float newButtonPos = 0;
        foreach (ItemCost item in shopButtons) {
            ShopEntryButton entry = Render(buttonTemplate, item);
            //make it child of conetntWindow
            entry.gameObject.transform.SetParent(contentWindow);
            PositionButton(ref entry, contentWindow, ref newButtonPos);
        }
    }

    private ShopEntryButton Render(ShopEntryButton buttonTemplate, ItemCost data) {
        ShopEntryButton ret = Instantiate(buttonTemplate);
        ret.SetData(data);
        return ret;
    }

    public void PositionButton(ref ShopEntryButton button, Transform contentWindow, ref float newButtonPos) {
        Button b = button.GetComponent<Button>();
        RectTransform myRect = b.GetComponent<RectTransform>();
        b.transform.position = new Vector3(xPadFromLeft + myRect.sizeDelta.x, -myRect.sizeDelta.y - newButtonPos);
        newButtonPos += myRect.sizeDelta.y;
    }

    public void Buy(ItemCost bought) {
        foreach (CostComponent i in bought.costs) {
            if (!inventory.HasItem(i.costType, i.quantity)) {
                //TODO: show rejection
                return;
            }
        }

        if (inventory.AddItem(bought.item)) {
            //the inventory has the required items, so take them
            foreach (CostComponent i in bought.costs) {
                for (int j = 0; j < i.quantity; j++) {
                    inventory.RemoveItem(i.costType);
                }
            }
        }
        foreach (KeyValuePair<InventoryItem,int> x in inventory.Contents()) {
            Debug.Log(x);
        }

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
