using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public abstract class SaveMenu : MenuWindow
{
    public abstract void LoadMode();
    public abstract void SaveMode();

    internal void LoadGame(string path) {
        Debug.Log("loading");
        //TODO dialog UI for selecting file
        XmlDocument doc = new XmlDocument();
        doc.Load(path);
        //XmlNode root = doc.FirstChild;
        DataManager.instance.LoadFromFile(doc);
    }


    internal void SaveGame() {    
        //TODO
    }

    public abstract void DeleteSave(string path);



}
