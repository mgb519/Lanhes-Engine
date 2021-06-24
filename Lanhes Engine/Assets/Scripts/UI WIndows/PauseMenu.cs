using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
public class PauseMenu : MenuWindow
{

    void Awake() {
        Time.timeScale = 0f;
    }

    public void UnPause() {
        DataManager.instance.Spew();
        Time.timeScale = 1f;
        CloseMenu();
    }

    public void LoadFromFile() {
        //TODO dialog UI for selecting file
        string path = "savefile.sav";
        XmlDocument doc = new XmlDocument();
        doc.Load(path);
        XmlNode root = doc.FirstChild;
        DataManager.instance.LoadFromFile(root);
    }

    public void SaveGameToFile()
    {
        //TODO dialog UI for selecting file
        string path = "savefile.sav";
        XmlDocument doc = new XmlDocument();
        XmlNode root =  DataManager.instance.SaveToFile(doc);
        doc.AppendChild(root);
        doc.Save(path);
    }
}
