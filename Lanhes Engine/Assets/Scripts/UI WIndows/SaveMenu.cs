using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using System.IO;
public abstract class SaveMenu : MenuWindow
{
    public abstract void LoadMode();
    public abstract void SaveMode();



    //TODO: these two functions get overriden by your game
    internal void LoadGame(string path) {


        StreamReader file = new StreamReader(path);

        XmlDocument doc = new XmlDocument();
        //skip the header, then feed the rest of the file to the XmlDocument for loading
        SkipHeader(ref file);
        doc.Load(file);
        //XmlNode root = doc.FirstChild;
        DataManager.instance.LoadFromFile(doc);

        file.Close();
    }


    internal void SaveGame(string path) {
        StreamWriter file = new StreamWriter(path);

        XmlDocument doc = new XmlDocument();
        XmlNode root = DataManager.instance.SaveToFile(doc);
        doc.AppendChild(root);


        //write the header, then  write the complete XML save document
        SaveHeader(ref file);
        doc.Save(file);

        file.Close();
    }

    /// <summary>
    /// Writes a header to file contianing highlights of information for save slots to display, preceding the XML document
    /// </summary>
    /// <param name="file">The TextWriter object through which the file is written.</param>
    internal abstract void SaveHeader(ref StreamWriter file);

    /// <summary>
    /// Skips the lines of the file corresponding ot the header 
    /// </summary>
    /// <param name="file">The TextReader object through which the file is read.</param>
    internal abstract void SkipHeader(ref StreamReader file);


    public abstract void DeleteSave(string path);



}
