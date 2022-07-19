using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphEvent : MapScript, NPCTrait
{

    [SerializeField]
    private EventScriptingCanvas graph;
    public override IEnumerator Action() {
        EventNode currentNode = graph.rootNode;
        IEnumerator pc = currentNode.Execute();
        do {           
            Debug.Log(pc.Current);
            if (pc.Current is EventNode node) {
                currentNode = node;
                Debug.Log("changed node");
                pc = currentNode.Execute();
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
        throw new System.NotImplementedException();
    }

    public JObject Save() {
        throw new System.NotImplementedException();
    }
}
