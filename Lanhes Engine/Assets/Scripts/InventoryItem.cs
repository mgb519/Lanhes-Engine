using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//[CreateAssetMenu(fileName = "New Item" ,menuName ="Inventory Item")]
public abstract class InventoryItem : ScriptableObject,ISelectable {
    new public string name;

    public int maxStack;

    //TODO: is this strictly necessary for the engine? maybe this should be in derived objects instead? not all games need an icon.
    public Sprite icon;

    public List<Tags> tags;

    public SelectionButton Render() {
        SelectionButton button = GameObject.Instantiate(WindowManager.BaseButton());
        button.GetComponentInChildren<Text>().text = name;
        button.dat = this;
        return button;
    }

    public enum Tags {
        KeyItem,
        Material,
        Armour,
        WondrousItem,
        Weapon

    }
}
