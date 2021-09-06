using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

//TODO finish this
//This should be overriden by the game, depending on how a save slot looks.
public class SaveInstanceButton : MonoBehaviour
{
    //TODO these should be taken out to an example file that overrides this!
    [SerializeField]
    private Text timeText;
    [SerializeField]
    private Text goldText;
    [SerializeField]
    private Text notes;

    private string path;
    public void Clicked() {
        SendMessageUpwards("SaveSelected", path);
    }

    public void SetPath(string newPath) {
        path = newPath;
        //populate the panel
        ReadHeader();
    }


    internal virtual void ReadHeader() {
        
        timeText.text = new FileInfo(path).LastWriteTime.ToString(); //TODO should this be stored in the header?
        StreamReader reader = new StreamReader(path);
        notes.text = reader.ReadLine();
        goldText.text = reader.ReadLine();
        reader.Close();

    }

    public void Delete() {
        SendMessageUpwards("DeleteSave", path);
    }

}
