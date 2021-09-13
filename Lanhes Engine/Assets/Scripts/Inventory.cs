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
    private InventoryContents items = new InventoryContents();


   

    public int HowManyOfItem(InventoryItem item) {
        if (items.ContainsKey(item)) { return items[item]; }
        return 0;
    }

   /// <summary>
   /// Adds the given amount of the given item to the inventory
   /// </summary>
   /// <param name="item"> The item to add</param>
   /// <param name="number"> The amount of it to add</param>
   /// <returns> The amount that was actually added to inventory</returns>
    public int AddItems(InventoryItem item, int number) {
        for (int i = 1; i <= number; i++) {
            if (!AddItem(item)) { return i; }
        }
        return number;
    }



    /// <summary>
    /// Adds 1 of the passed item to the inventory
    /// </summary>
    /// <param name="item">The item to add</param>
    /// <returns>True if the item was added, False if it could not be</returns>
    public bool AddItem(InventoryItem item) {
        //note: you may want to override if we want to impose a maximum total capacity restriction
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
    public IDictionary<InventoryItem, int> Contents() {
        return items;

    }

    public XmlNode SaveToFile(XmlDocument doc) {
        XmlElement root = doc.CreateElement("inventory");
        XmlElement itemsNode = doc.CreateElement("items");
        root.AppendChild(itemsNode);

        foreach (KeyValuePair<InventoryItem,int> kvp in items) {
            Debug.Log("saving "+kvp.Key.systemName);
            XmlElement entry = doc.CreateElement("entry");
            itemsNode.AppendChild(entry);

            XmlElement item = doc.CreateElement("item");
            item.InnerText = kvp.Key.systemName;
            entry.AppendChild(item);

            XmlElement num = doc.CreateElement("number");
            num.InnerText = kvp.Value.ToString();
            entry.AppendChild(num);
        }


        return root;
    }

    public void LoadFromFile(XmlNode node) {
        items.Clear();
        XmlElement root = node["inventory"];
        XmlElement itemsNode = root["items"];

        foreach (XmlElement entry in itemsNode) {
            XmlElement itemNode = entry["item"];
            XmlElement numNode = entry["number"];


            InventoryItem item = DataManager.GetItemBySystemName(itemNode.InnerText);
            int number = int.Parse(numNode.InnerText); //TODO make safer with TryParse
            int added = AddItems(item, number);
            if (added!=number) { 
                //TODO the save was modified or corrupted. There may be a problem.
            }
            
        }
    }
}