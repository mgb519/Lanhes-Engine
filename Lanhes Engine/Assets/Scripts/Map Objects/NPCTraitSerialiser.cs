using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

//TODO: editor script preventing us from naimng two GameObjects with NPCTraitSerialisers the same; if we were allowed to, then it would screw up saving
/// <summary>
/// This is the entrypoint for saving traits of NPC behaviour that you want to be totally persistent. Without this, things like dialogue will be refreshed upon exiting the scene.
/// </summary>
public class NPCTraitSerialiser : MonoBehaviour, ISaveable
{
    public XmlNode SaveToFile(XmlDocument doc) {
        throw new System.NotImplementedException();
    }

    public void LoadFromFile(XmlNode node) {
        throw new System.NotImplementedException();
    }
}
