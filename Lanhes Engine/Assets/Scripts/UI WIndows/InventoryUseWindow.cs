using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUseWindow : InventoryViewWindow
{
    public override void HandleSelection(InventoryItem selected) {
        if (selected is UsableItem i) {
            i.Use();//TODO: refine thsi with targeting
        }               
    }
}
