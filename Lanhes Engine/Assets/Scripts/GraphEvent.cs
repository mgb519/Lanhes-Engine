using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphEvent : MapScript, NPCTrait
{

    [SerializeField]
    private EventScriptingCanvas graph;


    private Dictionary<(EventNode,string), int> state = new Dictionary<(EventNode, string), int>();


    public override IEnumerator Action() {
        EventNode currentNode = graph.rootNode;
        IEnumerator pc = currentNode.Execute(state);
        do {           
            Debug.Log(pc.Current);
            if (pc.Current is EventNode node) {
                currentNode = node;
                Debug.Log("changed node");
                pc = currentNode.Execute(state);
            } else {
                yield return pc.Current ;
            }
        } while (pc.MoveNext());
        Debug.Log("Done!");
        yield break;
    }





    #region boilerplate
    /// <summary>
    /// This is boilerplate required for every class that inherits NPCTrait
    /// </summary>

    [SerializeField]
    private string uniqueID;
    public string UniqueID => uniqueID;
    public void OnValidate() {
        NPCTrait[] traits = GetComponents<NPCTrait>();
        foreach (NPCTrait trait in traits) {
            if (trait != this) {
                if (trait.UniqueID == this.UniqueID) {
                    Debug.LogError("Multiple NPCTraits have the same name: " + UniqueID + " \n on object " + this.name + ".");
                }
            }
        }

        NPCTraitSerialiser serialiser = GetComponent<NPCTraitSerialiser>();
        if (serialiser == null) {
            Debug.LogError("NPC object " + this.name + " has no NPCTraitSerialiser!");
        }
    }

    #endregion boilerplate

    //We do not save the shops or enemyParties variables, since they do not change.

    public void Load(JObject saveString) {
        state = new Dictionary<(EventNode, string), int>();
        foreach (KeyValuePair<string,JToken> b in saveString) {
            string[] compoundKey = b.Key.Split('_');
            int value = b.Value.ToObject<int>();
            EventNode keyNode = (EventNode)graph.nodes[int.Parse(compoundKey[0])];
            string index = compoundKey[1];

            state.Add((keyNode, index), value);
        }

    }

    public JObject Save() {

        JObject top = new JObject();
        foreach (KeyValuePair<(EventNode,string),int> k in state) {
            string propkey = graph.nodes.IndexOf(k.Key.Item1).ToString() + "_" + k.Key.Item2.ToString();           
            top.Add(propkey, k.Value.ToString());
        }
        return top;
    }
}
