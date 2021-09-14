using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//TODO editor script to register this with NPCTraitSerialsiser
[RequireComponent(typeof(NPCTraitSerialiser))]
/// <summary>
/// Base class for persistent NPC traits: data stored here will be stored long-term in memories through the NPCTraitSerailiser; without this, data will be lost upon leaving the scene.
/// </summary>
public  abstract class NPCTrait : MonoBehaviour
{
    //TODO

    [SerializeField]
    readonly string traitName;

    public abstract string Save();
    public abstract void Load(string saveString);
}
