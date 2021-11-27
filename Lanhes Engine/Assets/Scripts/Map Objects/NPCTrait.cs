using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;


//TODO editor script to register this with NPCTraitSerialsiser
//[RequireComponent(typeof(NPCTraitSerialiser))]
/// <summary>
/// Base class for persistent NPC traits: data stored here will be stored long-term in memories through the NPCTraitSerailiser; without this, data will be lost upon leaving the scene.
/// </summary>
public interface NPCTrait
{
    [SerializeField]
    public string UniqueID { get; }

    public abstract JObject Save();
    public abstract void Load(JObject saveString);
}


/*
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
            Debug.LogError("NPC object "+this.name+" has no NPCTraitSerialiser!");
        }
    }

    #endregion boilerplate 
 */
