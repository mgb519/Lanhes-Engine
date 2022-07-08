using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using Newtonsoft.Json.Linq;
[RequireComponent(typeof(ScriptMachine))]
public class GraphEvent : MapScript, NPCTrait
{

    public override IEnumerator Action() {
        CustomEvent.Trigger(gameObject,"MapEvent");
        yield break;
        //throw new System.NotImplementedException();
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

    public void Load(JObject saveString) {
        throw new System.NotImplementedException();
    }

    public JObject Save() {
        throw new System.NotImplementedException();
    }
}
