using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class DataManager : MonoBehaviour, ISaveable
{

    [System.Serializable]
    public abstract class Database<TV> : EditableDictionary<string, TV>, ISaveable
    {
        void ISaveable.LoadFromFile(XmlNode node)
        {
            //clear the DB
            this.Clear();
            //load from the node
            XmlNode dbNode = node["db"];
            foreach(XmlNode entry in dbNode.ChildNodes) {
                XmlNode keyNode = entry["key"];
                XmlNode valueNode = entry["value"];
                this.Add(keyNode.InnerText,StringToValue(valueNode.InnerText));
            }
        }

        internal abstract TV StringToValue(string text);

        XmlNode ISaveable.SaveToFile(XmlDocument doc)
        {
            XmlElement ret = doc.CreateElement("db");

            foreach (string key in this.Keys)
            {
                XmlElement entry = doc.CreateElement("entry");
                ret.AppendChild(entry);

                XmlElement k = doc.CreateElement("key");
                entry.AppendChild(k);
                //TODO absolutely disgusting, why are we using InnerText? 
                k.InnerText = key;

                XmlElement v = doc.CreateElement("value");
                entry.AppendChild(v);
                v.InnerText = ValueToString(this[key]);
            }

            return ret;
        }

        internal abstract string ValueToString(TV value);
    };




    [System.Serializable]
    public class IntDatabase : Database<int>
    {
        internal override int StringToValue(string text)
        {
            //TODO use TryParse and then give a nice error message
            return int.Parse(text);
        }

        internal override string ValueToString(int value)
        {
            return value.ToString();
        }
    };

    [SerializeField]
    private IntDatabase intData = new IntDatabase();

    [System.Serializable]
    public class StringDatabase : Database<string>
    {
        internal override string StringToValue(string text)
        {
            return text;
        }

        internal override string ValueToString(string value)
        {
            return value;
        }
    }

    [SerializeField]
    private StringDatabase stringData = new StringDatabase();


    [System.Serializable]
    public class BoolDatabase : Database<bool>
    {
        internal override bool StringToValue(string text)
        {
            return Boolean.Parse(text);
        }

        internal override string ValueToString(bool value)
        {
           
            return value.ToString();
        }
    }

    [SerializeField]
    private BoolDatabase boolData = new BoolDatabase();


    internal void Spew()
    {
        foreach (var i in intData.Keys)
        {
            Debug.Log(i + ":" + intData[i]);
        }
    }


    //TODO: would prefer a more concrete type than ISaveable this this is just used for saving anyway so no biggie rn
    (string, ISaveable)[] databases;



    public static DataManager instance = null;

    void Awake()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            //register databases so that saveing and loading can see them
            databases = new (string, ISaveable)[] { ("ints", intData), ("strings", stringData), ("bools", boolData) };
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public int GetInt(string key) { return GetVal(key, intData); }
    public void SetInt(string key, int newValue) { SetVal(key, newValue, intData); }


    public string GetString(string key) { return GetVal<string>(key, stringData); }
    public void SetString(string key, string newValue) { SetVal<string>(key, newValue, stringData); }


    public bool GetBool(string key) { return GetVal<bool>(key, boolData); }
    public void SetBool(string key, bool newValue) { SetVal<bool>(key, newValue, boolData); }


    public void SetVal<T>(string key, T newValue, IDictionary<string, T> dict)
    {
        if (dict.ContainsKey(key))
        {
            dict.Remove(key);
            dict.Add(key, newValue);
            return;
        }
        else
        {
            //give a warning, while a lot of the time this may be intentional, it may not always be, i.e typos
            //in that case, why not have initialisation be its own operation?
            Debug.LogWarning("Key " + key + " not found in " + typeof(int) + " database, creating with value " + newValue);
            dict.Add(key, newValue);
        }
    }

    //TODO: should be initialise the entry if it doesn't exist instead?
    public T GetVal<T>(string key, IDictionary<string, T> dict)
    {
        if (dict.ContainsKey(key))
        {
            return dict[key];
        }
        else
        {
            Debug.LogWarning("Key " + key + " not found in " + typeof(int) + " database!");
            throw new KeyNotFoundException();
        }

    }






    //TODO How about we use JSON instead huh? JSONhelper *exists*
    //TODO: serialise and restoring databases
    public XmlNode SaveToFile(XmlDocument doc)
    {

        //TODO save the scene ID we are in


        XmlElement ret = doc.CreateElement("data");

        //store DBs
        foreach((string,ISaveable) dbData in databases) {
            XmlElement entry = doc.CreateElement(dbData.Item1);
            ret.AppendChild(entry);
            entry.AppendChild(dbData.Item2.SaveToFile(doc));
        }

        //TODO call saves of other managers

        //save NPC positions
        XmlElement npcHolderNode = doc.CreateElement("npcs");
        ret.AppendChild(npcHolderNode);
        //Get all NPCs that can move; i.e they have pawn movement.
        PawnMovementController[] npcs = FindObjectsOfType<PawnMovementController>(); ;
        foreach(PawnMovementController n in npcs) {
            XmlElement npcNode = doc.CreateElement("npc");
            npcHolderNode.AppendChild(npcNode);

            GameObject npc = n.gameObject;
            npcNode.SetAttribute("name",npc.name); //TODO: this requires that NPCs have unique editor names.

            XmlElement x = doc.CreateElement("x");
            npcNode.AppendChild(x);
            x.InnerText = npc.transform.position.x.ToString();

            XmlElement y = doc.CreateElement("y");
            npcNode.AppendChild(y);
            y.InnerText = npc.transform.position.y.ToString();

            XmlElement z = doc.CreateElement("z");
            npcNode.AppendChild(z);
            z.InnerText = npc.transform.position.z.ToString();

            //TODO: NPC rotation? will I need that?
        }

        //TODO How do we store ink state? Especially since we will not save every scene, what about Ink state from previous scenes?
        //TODO we'll have to put all ink state in memory when leaving a scene, then restore those when entering the scene. These must be stored until flushed via saving

        return ret;
    }

    public void LoadFromFile(XmlNode node)
    {
        //TODO make this async somehow?
        //TODO load the scene id and transition to that scene


        XmlElement dataNode = node["data"];

        //restore DBs
        foreach ((string, ISaveable) dbData in databases)
        {
            XmlElement dbNode = dataNode[dbData.Item1];
            dbData.Item2.LoadFromFile(dbNode);
        }

        //TODO load other managers

        //load NPC positions
        XmlNode npcHolder = dataNode["npcs"];
        foreach(XmlNode npcNode in npcHolder.ChildNodes) {
            string name = npcNode.Attributes["name"].Value;

            GameObject npc = GameObject.Find(name);
            if(npc == null) {
                Debug.LogError("NPC "+name+" not found!");
                continue;
            }
            float x = float.Parse(npcNode["x"].InnerText);
            float y = float.Parse(npcNode["y"].InnerText);
            float z = float.Parse(npcNode["z"].InnerText);

            npc.transform.position= new Vector3(x,y,z);
        }



        //TODO load Ink state...?

        //throw new NotImplementedException();
    }
}
