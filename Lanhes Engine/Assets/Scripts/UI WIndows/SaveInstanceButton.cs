using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;


//TODO finish this
//This should be overriden by the game, depending on how a save slot looks.
public class SaveInstanceButton : MonoBehaviour
{

    [SerializeField]
    private Text timeText;

    private string path;
    public void Clicked() {
        SendMessageUpwards("SaveSelected", path);
    }

    public void SetPath(string newPath) {
        path = newPath;
        //populate the panel

        //maybe figure out a way to not have to read the whole file in? My best solution is a header to the file, so that we can just pipe in the first few lines rather than LOADING AN ENTIRE XML DOCUMENT INTO MEMORY
        //TODO implement the above
        XmlDocument doc = new XmlDocument();
        doc.Load(path);

        //TODO fill this out further (even though it is just a demo, it'll be edited by the end user)

        timeText.text = new System.IO.FileInfo(path).LastWriteTime.ToString();

    }

    public void Delete() {
        SendMessageUpwards("DeleteSave", path);
    }

}
