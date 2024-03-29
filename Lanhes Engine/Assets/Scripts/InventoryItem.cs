﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;


//[CreateAssetMenu(fileName = "New Item" ,menuName ="Inventory Item")]
public abstract class InventoryItem : ScriptableObject,ISelectable {
    public string systemName;
    public LocalizedString readableName;
    public int maxStack;

    public virtual SelectionButton Render() { //TODO your game *will* override this
        SelectionButton button = GameObject.Instantiate(WindowManager.BaseButton());
        button.GetComponentInChildren<Text>().text = readableName.GetLocalizedString();
        button.dat = this;
        return button;
    }



}
