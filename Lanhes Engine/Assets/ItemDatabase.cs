using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : ScriptableObject
{
    [SerializeField]
    public List<InventoryItem> items = new List<InventoryItem>();

}
