using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Whenever we create an InventoryItem, add it to the ItemDatabase. Whenever we delete an InventoryItem, remove it from the ItemDatabase
/// </summary>
class MyAllPostprocessor : AssetPostprocessor
{
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) {
        bool dirtied = false;
        const string PATH = "Assets/ItemDatabase.asset";
        ItemDatabase db = AssetDatabase.LoadAssetAtPath<ItemDatabase>(PATH);
        if (db == null) {
            db =ScriptableObject.CreateInstance<ItemDatabase>();
            AssetDatabase.CreateAsset(db, PATH);
        }


        foreach (string assetPath in importedAssets) {
            //Debug.Log("Reimported Asset: " + assetPath);
            InventoryItem item = AssetDatabase.LoadAssetAtPath<InventoryItem>(assetPath);
            if (item != null) {
                //Debug.Log("Asset " + assetPath + " was an InventoryItem");

                //The asset that was imported was an item. Add it to the ItemDatabase.
                InventoryItem inList = db.items.Find(x => x.systemName == item.systemName);

                if (inList == item) {
                    //the item is already in the list, do not add it. Presumably, it was just edited.
                    continue;
                }

                if (inList !=null && inList != item) {
                    Debug.LogError("Two items have the same system name: " + item.systemName + ". " + assetPath + " was not added to the item database.");
                    continue;
                }

                db.items.Add(item);
                dirtied = true;
                Debug.Log("Added item: " + assetPath);
            }
        }
        foreach (string assetPath in deletedAssets) {
            //Debug.Log("Deleted Asset: " + assetPath);
            //TODO I have no fucking clue how *this* works Surely the asset is gone, I can't access it
            InventoryItem item = AssetDatabase.LoadAssetAtPath<InventoryItem>(assetPath);
            if (item != null) {
                bool s = db.items.Remove(item);
                dirtied = true;
                Debug.Log("Removed item: "+assetPath+" :"+s);
            }
        }

        for (int i = 0; i < movedAssets.Length; i++) {
            //Debug.Log("Moved Asset: " + movedAssets[i] + " from: " + movedFromAssetPaths[i]);
            //This shouldn't be an issue since the ItemDatabase already has a reference to the asset           
        }
        if (dirtied) {
            EditorUtility.SetDirty(db);            
        }
    }
}
