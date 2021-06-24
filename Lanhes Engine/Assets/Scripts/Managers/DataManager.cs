using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class DataManager : MonoBehaviour ,ISaveable{


    [System.Serializable]
    public class IntDatabase : EditableDictionary<string, int> { };

    [SerializeField]
    public IntDatabase intData = new IntDatabase();

    [System.Serializable]
    public class StringDatabase : EditableDictionary<string,string> { }

    [SerializeField]
    public StringDatabase stringData = new StringDatabase();


    [System.Serializable]
    public class BoolDatabase : EditableDictionary<string,bool> { }

    [SerializeField]
    public BoolDatabase boolData = new BoolDatabase();


    internal void Spew() {
        foreach (var i in intData.Keys) {
            Debug.Log(i + ":" + intData[i]);
        }
    }

    public static DataManager instance = null;

    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else if (instance != this) {
            Destroy(gameObject);
        }
    }

    public int GetInt(string key) {return GetVal(key, intData);}
    public void SetInt(string key, int newValue) {SetVal(key, newValue, intData); }


    public string GetString(string key) { return GetVal<string>(key, stringData); }
    public void SetString(string key, string newValue) { SetVal<string>(key, newValue, stringData); }


    public bool GetBool(string key) { return GetVal<bool>(key, boolData); }
    public void SetBool(string key, bool newValue) { SetVal<bool>(key, newValue, boolData); }


    public void SetVal<T>(string key, T newValue, IDictionary<string,T> dict) {
        if (dict.ContainsKey(key)) {
            dict.Remove(key);
            dict.Add(key, newValue);
            return;
        } else {
            //give a warning, while a lot of the time this may be intentional, it may not always be, i.e typos
            //in that case, why not have initialisation be its own operation?
            Debug.LogWarning("Key " + key + " not found in " + typeof(int) + " database, creating with value " + newValue);
            dict.Add(key, newValue);
        }
    }
    
    //TODO: should be initialise the entry if it doesn't exist instead?
    public T GetVal<T>(string key, IDictionary<string,T> dict) {
        if (dict.ContainsKey(key)) {
            return dict[key];
        } else {
            Debug.LogWarning("Key " + key + " not found in " + typeof(int) + " database!");
            throw new KeyNotFoundException();
        }

    }


    




    //TODO: serialise and restoring databases
    public XmlNode SaveToFile(XmlDocument doc) {
        //TODO not sure if creating a new XmlElement and returnign it is a good idea, maybe use an inbuilt method?
        XmlElement ret = doc.CreateElement("data");
        //TODO call save on databases
        ret.AppendChild(intData.SaveToFile(doc));

        //TODO call saves of other data managers



        return ret;
    }

    public void LoadFromFile(XmlNode node)
    {
        throw new NotImplementedException();
    }
}
