using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class UsableItem : InventoryItem {

    //TODO: refine thsi into items usable in battle and in the map; have seperate Use functions for those, and taking a chatarcter as the target
    

    //TODO: this will need a character as a target
    abstract public void Use();
}
