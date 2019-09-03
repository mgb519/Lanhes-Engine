using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour {

    public static WindowManager instance = null;


    public StringSelectWindow stringSelectWindow;
    public StringWindow stringWindow;
    public ShopWindow shopWindow;
    public MenuWindow pauseMenu;

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
        dialog.displayMe = text;//TODO: have a fucntion to update text which auto-refreshes; if might see use in spelling dialogue out
        dialog.Refresh();
        Time.timeScale = 0;
        return dialog;
    }

    public static StringSelectWindow CreateStringSelectWindow(List<string> options) {
        StringSelectWindow dialog = (StringSelectWindow)CreateWindow(instance.stringSelectWindow);
        foreach (string i in options) {
            dialog.data.Add(i);
        }
        dialog.Refresh();
        Time.timeScale = 0;
        return dialog;
    }

    public static ShopWindow CreateShopWindow(List<ItemCost> forSale, List<ItemCost> willBuy, Inventory playerInventory) {
        Debug.Log("Creating shop window");
        ShopWindow window = (ShopWindow)CreateWindow(instance.shopWindow);
        window.buyButtons.AddRange(forSale);
        window.sellButtons.AddRange(willBuy);
        window.inventory = playerInventory;
        window.Refresh();
        Time.timeScale = 0;
        return window;

    }

    public static void CreatePauseWindow() {
        // spawn  pause menu object
        CreateWindow(instance.pauseMenu);
        //pause the game
        Time.timeScale = 0;
    }
}
