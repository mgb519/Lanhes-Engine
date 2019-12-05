using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class InventoryViewWindow : MenuWindow {
    public InventoryButton buttonTemplate;

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

    public override void Refresh() {
        //render each item
        foreach (InventoryItem item in inventory.Contents().Keys) {
            //check that every tag we are filtering for is present
            if (filter.TrueForAll(x => item.tags.Contains(x))) {
                InventoryButton button = Render(item);
                button.gameObject.transform.SetParent(contentWindow);
                PositionButton(ref button, contentWindow);
            }
        }
    }

    public virtual void PositionButton(ref InventoryButton button, Transform contentWindow) {
        Button b = button.GetComponent<Button>();
        RectTransform myRect = b.GetComponent<RectTransform>();
        b.transform.position = new Vector3(xPadFromLeft + myRect.sizeDelta.x, -myRect.sizeDelta.y - newButtonPos);
        newButtonPos += myRect.sizeDelta.y;
    }


    protected virtual InventoryButton Render(InventoryItem data) {
        InventoryButton ret = Instantiate(buttonTemplate);
        ret.SetData(data);
        return ret;
    }


}
