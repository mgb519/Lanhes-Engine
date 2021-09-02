using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class WindowManager : MonoBehaviour, ISaveable {  //FIXME does this need to be Saveable?

    public static WindowManager instance = null;

    public StringWindow stringWindow;
    public ShopWindow shopWindow;
    public PauseMenu pauseMenu;

    public SelectionButton selectionButton;
    public SelectionWindow selectionWindow;

    public SaveMenu saveMenu;

    private MenuWindow baseWindow = null;



    //TODO: should this be here? since it's also  used by PC script to know if they can continue
    public bool ContinuePlay() {
        return baseWindow == null;
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


    private static T CreateWindow<T>(T other, MenuWindow creator) where T : MenuWindow {
        T subwindow = GameObject.Instantiate(other, instance.transform);

        if (creator == null) {
            instance.baseWindow = subwindow;
        } else {
            subwindow.creator = creator;
        }
        return subwindow;
    }



    public static StringWindow CreateStringWindow(string text, MenuWindow creator) {
        StringWindow dialog = CreateWindow(instance.stringWindow, creator);
        dialog.Refresh(text);
        return dialog;
    }


    public static ShopWindow CreateShopWindow(List<ItemCost> forSale, List<ItemCost> willBuy, Inventory playerInventory, MenuWindow creator) {
        //Debug.Log("Creating shop window");
        ShopWindow window = CreateWindow(instance.shopWindow, creator);
        window.buyButtons.AddRange(forSale);
        window.sellButtons.AddRange(willBuy);
        window.inventory = playerInventory;
        window.Refresh();

        //TODO: NOTE: Do we want the shop to block NPC behavoir too?
        return window;

    }

    public static SelectionWindow CreateStringSelection(List<string> list, MenuWindow creator) {
        List<ISelectable> processed = new List<ISelectable>();
        foreach (string s in list) {
            processed.Add(new SelectableString(s));
        }
        return CreateSelection(processed, creator);
    }

    public static SelectionWindow CreateSelection(List<ISelectable> list, MenuWindow creator) {
        SelectionWindow window = CreateWindow(instance.selectionWindow, creator);
        window.Refresh(list, window.ReturnAndClose);
        return window;
    }

    public static SelectionButton BaseButton() {
        SelectionButton b = Instantiate(instance.selectionButton);
        return b;
    }

    public static void CreatePauseWindow() {
        // spawn  pause menu object
        MenuWindow window = CreateWindow(instance.pauseMenu, null); //pause menus aren't created by other menus

        instance.baseWindow = window;
        //pause the game is handled by the creation ofthe window
    }

    public static SaveMenu CreateLoadWindow(MenuWindow creator) {
        SaveMenu window = CreateWindow(instance.saveMenu, creator);
        window.LoadMode();
        return window;
    }


    public static SaveMenu CreateSaveWindow(MenuWindow creator) {
        SaveMenu window = CreateWindow(instance.saveMenu, creator);
        window.SaveMode();
        return window;
    }

    public XmlNode SaveToFile(XmlDocument doc) {
        XmlElement ret = doc.CreateElement("windows");


        return ret;
    }

    public void LoadFromFile(XmlNode node) {
        //nothing is saved, so nothing is loaded!
    }
}
