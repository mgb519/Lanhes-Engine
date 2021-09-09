using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml;

[Serializable]
public class Inventory : ISaveable {




    [Serializable]
    public class InventoryContents : EditableDictionary<InventoryItem, int> { }

    [SerializeField]
    public InventoryContents items = new InventoryContents();


   

    public int HowManyOfItem(InventoryItem item) {
        if (items.ContainsKey(item)) { return items[item]; }
        return 0;
    }

    /// <summary>
    /// Adds 1 of the passed item to the inventory
    /// </summary>
    /// <param name="item">The item to add</param>
    /// <returns>True if the item was added, False if it could not be</returns>
    public bool AddItem(InventoryItem item) {
        //note: you may want to override if we want to impose a maximum capacity restriction
        if (items.ContainsKey(item)) {
            if (items[item] >= item.maxStack) {
                return false;
            }
            items[item] += 1;
            return true;
        } else {
            items.Add(item, 1);
            return true;
        }
    }

    /// <summary>
    /// removes 1 of the passed item from the inventory
    /// </summary>
    /// <param name="item">The item to remove</param>  
    public void RemoveItem(InventoryItem item) {
        if (items.ContainsKey(item)) {
            items[item] -= 1;
            if (items[item] == 0) { items.Remove(item); }
        }
    }

    /// <summary>
    /// returns true if the inventory has at least that amount of item
    /// </summary>
    /// <param name="item">The item to check for</param>  
    /// <param name="quantity">the amount to check for</param>
    public bool HasItem(InventoryItem item, int quantity) {
        //note: you may want to override if we want to impose a maximum capacity restriction
        return HowManyOfItem(item) >= quantity;
    }


    //TODO: ?????
    public Dictionary<InventoryItem, int> Contents() {
        Dictionary<InventoryItem, int> ret = new Dictionary<InventoryItem, int>();

        foreach (var k in items.Keys) {
            ret.Add(k, items[k]);
        }
        
        return ret;

    }

    public XmlNode SaveToFile(XmlDocument doc) {
        //TODO finish
        return doc.CreateElement("a");
        throw new NotImplementedException();
    }

    public void LoadFromFile(XmlNode node) {
        //TODO finish
        return;
        throw new NotImplementedException();
    }
}