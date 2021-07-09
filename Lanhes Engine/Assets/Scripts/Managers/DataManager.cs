﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;

//TODO this class is getting a bit chunky
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
            foreach (XmlNode entry in dbNode.ChildNodes)
            {
                XmlNode keyNode = entry["key"];
                XmlNode valueNode = entry["value"];
                this.Add(keyNode.InnerText, StringToValue(valueNode.InnerText));
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





    [System.Serializable]
    public class InkStates : EditableDictionary<(int, string), string> { }


    [SerializeField]
    private InkStates inkStates = new InkStates();









    internal void Spew()
    {
        foreach (var i in intData.Keys)
        {
            Debug.Log(i + ":" + intData[i]);
        }
    }


    //TODO: would prefer a more concrete type than ISaveable this this is just used for saving anyway so no biggie rn
    (string, ISaveable)[] databases;
    (string, ISaveable)[] managers;


    public static DataManager instance = null;
    private static bool isLoading = false;

    public static bool IsLoading() { return isLoading; }
    void Awake()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            //register databases so that saveing and loading can see them
            databases = new (string, ISaveable)[] { ("ints", intData), ("strings", stringData), ("bools", boolData) };
            managers = new (string, ISaveable)[] { ("gamescenes", gameObject.GetComponent<GameSceneManager>()), ("parties", gameObject.GetComponent<PartyManager>()), ("windows", gameObject.GetComponent<WindowManager>()) }; //TODO is it even necessary to save some of these? (specifically, looking and window manager and scene manager)
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


    //TODO What about dialogues that aren't in Ink? I guess just don't have those then.

    //TODO this should be called when transitioning scenes too, so that Inks are held in memory to flush
    /// <summary>
    /// Collect dialogues in this scene and hold them in memory
    /// </summary>
    public void RememberDialogues()
    {
        DialogueEvent[] dialogueEvents = FindObjectsOfType<DialogueEvent>();
        int currentSceneId = SceneManager.GetActiveScene().buildIndex;
        foreach (DialogueEvent e in dialogueEvents)
        {
            string json = e.Save();
            inkStates.Add((currentSceneId, e.name), json);
        }

    }

    /// <summary>
    /// Find dialogues that are held in memory and flush them into the scene. They will no longer be held in memory, but will be remembered when we leave the scene.
    /// </summary>
    public void RestoreDialogues()
    {
        int sceneId = SceneManager.GetActiveScene().buildIndex;
        //get all npc names in this scene that have a remembered JSON
        List<(int, string)> keys = inkStates.keys.FindAll(x => x.Item1 == sceneId);

        foreach ((int, string) key in keys)
        {
            GameObject npc = GameObject.Find(key.Item2);

            DialogueEvent e = npc.GetComponent<DialogueEvent>();
            e.Load(inkStates[key]);
            inkStates.Remove(key);
        }

    }


    /// <summary>
    /// Flush dialogues in memory to file, as well is dialogues in scene.
    /// </summary>
    public XmlNode SaveDialogues(XmlDocument doc)
    {
        //save Ink dialogues

        XmlNode root = doc.CreateElement("inks");

        //Get all DialogueEvents in scene, serialise thier JSON



        //TODO: you should not remember Inks that have not been interacted with
        //FIXME lots of shared code between this and saving rememebered dialogues
        DialogueEvent[] dialogueEvents = FindObjectsOfType<DialogueEvent>();
        string currentSceneId = SceneManager.GetActiveScene().buildIndex.ToString();
        foreach (DialogueEvent e in dialogueEvents)
        {
            string json = e.Save();
            XmlElement inkNode = doc.CreateElement("ink");
            root.AppendChild(inkNode);

            inkNode.SetAttribute("scene", currentSceneId);
            inkNode.SetAttribute("name", e.name);

            inkNode.InnerText = json;
        }

        //store all remembered extra-scene dialogues
        foreach ((int, string) key in inkStates.Keys)
        {
            XmlElement inkNode = doc.CreateElement("ink");
            root.AppendChild(inkNode);

            inkNode.SetAttribute("scene", key.Item1.ToString());
            inkNode.SetAttribute("name", key.Item2);

            inkNode.InnerText = inkStates[key];

        }

        return root;


    }


    /// <summary>
    /// Load all dialogues from file and place them in the Ink memory
    /// </summary>
    public void LoadDialogues(XmlNode node)
    {
        //load Ink dialogues
        inkStates.Clear();

        XmlNode inksNode = node["inks"];

        foreach (XmlNode entry in inksNode.ChildNodes)
        {
            int sceneId = int.Parse(entry.Attributes["scene"].Value);
            string npcId = entry.Attributes["name"].Value;
            string json = entry.InnerText;

            inkStates.Add((sceneId, npcId), json);
        }


    }


    //TODO How about we use JSON instead huh? JSONhelper *exists*
    public XmlNode SaveToFile(XmlDocument doc)
    {


        XmlElement ret = doc.CreateElement("data");


        //Save the scene we are in
        XmlElement sceneId = doc.CreateElement("scene");
        ret.AppendChild(sceneId);
        sceneId.InnerText = SceneManager.GetActiveScene().name;


        //store DBs
        foreach ((string, ISaveable) dbData in databases)
        {
            XmlElement entry = doc.CreateElement(dbData.Item1);
            ret.AppendChild(entry);
            entry.AppendChild(dbData.Item2.SaveToFile(doc));
        }

        //call saves of other managers
        XmlElement managerHolderNode = doc.CreateElement("managers");
        ret.AppendChild(managerHolderNode); 
        foreach ((string, ISaveable) manData in managers)
        {
            XmlElement entry = doc.CreateElement(manData.Item1);
            managerHolderNode.AppendChild(entry);
            entry.AppendChild(manData.Item2.SaveToFile(doc));

            managerHolderNode.AppendChild(entry);
        }

        //save NPC positions
        XmlElement npcHolderNode = doc.CreateElement("npcs");
        ret.AppendChild(npcHolderNode);
        //Get all NPCs that can move; i.e they have pawn movement.
        PawnMovementController[] npcs = FindObjectsOfType<PawnMovementController>();
        foreach (PawnMovementController n in npcs)
        {
            XmlElement npcNode = doc.CreateElement("npc");
            npcHolderNode.AppendChild(npcNode);

            GameObject npc = n.gameObject;
            npcNode.SetAttribute("name", npc.name); //TODO: this requires that NPCs have unique editor names.

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

        //save dialogues
        XmlElement dialoguesHolderNode = doc.CreateElement("dialogues");
        ret.AppendChild(dialoguesHolderNode);

        dialoguesHolderNode.AppendChild(SaveDialogues(doc));

        return ret;
    }

    public IEnumerator LoadBody(XmlNode node)
    {


        Debug.Log("load body");
        isLoading = true;

        XmlElement dataNode = node["data"];


        //restore DBs
        foreach ((string, ISaveable) dbData in databases)
        {
            XmlElement dbNode = dataNode[dbData.Item1];
            dbData.Item2.LoadFromFile(dbNode);
        }


        //load the scene id and transition to that scene
        string sceneId = dataNode["scene"].InnerText;
        GameSceneManager.StartLoadScene(sceneId);

        yield return new WaitUntil(() => !GameSceneManager.IsLoading());

        Debug.Log("loaded scene");

        //load other managers
        XmlElement managerHolderNode = dataNode["managers"];
        foreach ((string, ISaveable) m in managers)
        {
            XmlElement managerNode = managerHolderNode[m.Item1];
            m.Item2.LoadFromFile(managerNode);
        }
        //player has been spawned correctly when loading PartyManager, so it exists in-scene.

        //load NPC positions
        XmlNode npcHolder = dataNode["npcs"];
        foreach (XmlNode npcNode in npcHolder.ChildNodes)
        {
            string name = npcNode.Attributes["name"].Value;

            GameObject npc = GameObject.Find(name);
            Debug.Log(name);
            if (npc == null)
            {
                Debug.LogError("NPC " + name + " not found!");
                continue;
            }
            float x = float.Parse(npcNode["x"].InnerText);
            float y = float.Parse(npcNode["y"].InnerText);
            float z = float.Parse(npcNode["z"].InnerText);
            //FIXME: player is not positioned correctly, and always returns to spawn point
            npc.transform.position = new Vector3(x, y, z);
        }


        //load Ink state
        LoadDialogues(dataNode["dialogues"]);
        RestoreDialogues();
        isLoading = false;
        //throw new NotImplementedException();
        Debug.Log("Finsihed load body");

    }


    public void LoadFromFile(XmlNode node)
    {
        Debug.Log("start load");
        StartCoroutine(LoadBody(node));
    }
}
