using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class ScriptData {
    private IDictionary<string, int> ints;
    private IDictionary<string, string> strings;
    private IDictionary<string, bool> bools;

    public ScriptData() {
        ints = new Dictionary<string, int>();
        strings = new Dictionary<string, string>();
        bools = new Dictionary<string, bool>();
    }

    public int GetInt(string key) {
        if (ints.ContainsKey(key)) {
            int returnValue = ints[key];
            Debug.Log("Getting int " + key + ", returned " + returnValue);
            return returnValue;
        } else {
            //do we return an error or initialise the variable to 0? Shouldn't the latter case only happen with setting?
        }

    }

    public void SetInt(string key, int newValue) {
        if (ints.ContainsKey(key)) {
            Debug.Log("Setting int " + key + "from " + ints[key] + " to " + newValue);
            ints[key] = newValue;

        } else {
            Debug.LogWarning("int " + key + " does not exist, initializing to " + newValue);
            ints.Add(key, newValue);
        }
    }


}
