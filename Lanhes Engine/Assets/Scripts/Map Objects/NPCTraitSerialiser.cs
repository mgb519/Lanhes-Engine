using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System.Linq;


//TODO: editor script preventing us from naimng two GameObjects with NPCTraitSerialisers the same; if we were allowed to, then it would screw up saving
/// <summary>
/// This is the entrypoint for saving traits of NPC behaviour that you want to be totally persistent. Without this, things like dialogue will be refreshed upon exiting the scene.
/// </summary>
public class NPCTraitSerialiser : MonoBehaviour
{
    internal string Save() {
        JObject output = new JObject();

        NPCTrait[] traits = GetComponents<NPCTrait>();
        foreach (NPCTrait trait in traits) {
            string uid = trait.UniqueID;
            JObject content = trait.Save();
            output.Add(uid, content);
        }
        return output.ToString();//TODO this is pretty-printed. I do not strictly want that.
    }

    internal void Load(string v) {
        JObject parsed = JObject.Parse(v);
        NPCTrait[] traits = GetComponents<NPCTrait>();
        foreach (var child in parsed.Properties()) {
            string uid = child.Name;
            NPCTrait matched = traits.Where(item => item.UniqueID == uid).First();            
            
            JObject content = child.Value.ToObject<JObject>();

        }
    }

}
