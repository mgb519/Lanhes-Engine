using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowManager : MonoBehaviour {

    public static WindowManager instance = null;

    public StringWindow stringWindow;
    public ShopWindow shopWindow;
    public PauseMenu pauseMenu;

    public SelectionButton selectionButton;
    public SelectionWindow selectionWindow;

    private MenuWindow baseWindow = null;

    //TODO: should this be here? since it's also  used by PC script to know if they can continue
    public bool ContinuePlay() {
        return baseWindow==null;
    }

    public void WindowClosed() {
        baseWindow = null;
    }


    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else if (instance != this) {
            Destroy(gameObject);
        }
    }


    private static MenuWindow CreateWindow(MenuWindow other) {
        MenuWindow subwindow = GameObject.Instantiate(other);
        return subwindow;
    }



    public static StringWindow CreateStringWindow(string text) {
        StringWindow dialog = (StringWindow)CreateWindow(instance.stringWindow);
        dialog.Refresh(text);
        instance.baseWindow = dialog;
        return dialog;
    }

   
    public static ShopWindow CreateShopWindow(List<ItemCost> forSale, List<ItemCost> willBuy, Inventory playerInventory) {
        Debug.Log("Creating shop window");
        ShopWindow window = (ShopWindow)CreateWindow(instance.shopWindow);
        window.buyButtons.AddRange(forSale);
        window.sellButtons.AddRange(willBuy);
        window.inventory = playerInventory;
        window.Refresh();
        instance.baseWindow = window;

        //TODO: NOTE: Do we want the shop to block NPC behavoir too?
        return window;

    }

    public static SelectionWindow CreateStringSelection(List<string> list) {
        List<ISelectable> processed = new List<ISelectable>();
        foreach (string s in list) {
            processed.Add(new SelectableString(s));
        }
        return CreateSelection(processed);
    }

    public static SelectionWindow CreateSelection(List<ISelectable> list) {
        SelectionWindow window = Instantiate(instance.selectionWindow);
        window.Refresh(list,window.ReturnAndClose);
        instance.baseWindow = window;
        return window;
    }

    public static SelectionButton BaseButton() {
        SelectionButton b = Instantiate(instance.selectionButton);
        return b;
    }

    public static void CreatePauseWindow() {
        // spawn  pause menu object
        MenuWindow window = CreateWindow(instance.pauseMenu);
        
        instance.baseWindow = window;
        //pause the game is handled by the creation ofthe window
    }
}
