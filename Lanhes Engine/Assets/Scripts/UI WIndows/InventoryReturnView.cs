using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryReturnView : InventoryViewWindow {

    public override void HandleSelection(InventoryItem selected) {
        creator.lastSelection = selected;
        CloseMenu();
    }
}
