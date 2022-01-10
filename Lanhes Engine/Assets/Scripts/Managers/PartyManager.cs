using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json.Linq;


//keeps a list of the parties, each of which has its own inventory
//most games will only have one so it's a little moot
public class PartyManager : MonoBehaviour, ISaveable
{
    public static PartyManager instance = null;

    //FIXME: this *should* be private, but the EDITOR needs this to be public, apprently labvelling it serializable is not enough.
    //FIXME: this should be static.
    [SerializeField]
    public Party[] parties = new Party[7];// TODO couldn't this be a SerializableDisctionary and the player spanwer use a key rather than an index?

    private static Party partyThisScene;
    //points towards the party in this scene
    //maybe this one isn't the best place to put it?
    public static PlayerController playerInThisScene;




    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else if (instance != this) {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Sets the current party to the one at index id, and spawns it into the scene at an arbitrary point. Position should be handled by a later call, such as the load function.
    /// </summary>
    /// <param name="id">index of the party being spawned in</param>
    public static void SpawnPlayer(int id) {
        //Debug.Log("Spawning player by ID");
        if (playerInThisScene != null) {
            //a player already exists, do not spawn a new one. TODO maybe delete it and create a new one? idk anymore. Can this even happen?
            return;
        }

        partyThisScene = instance.parties[id];

        playerInThisScene = Instantiate(partyThisScene.avatar, instance.transform.position, instance.transform.rotation);
        playerInThisScene.gameObject.name = "Player";

    }

    /// <summary>
    /// Spawn the currently set player party in the current scene at the speceficied spawn point.
    /// </summary>
    /// <param name="marker">The name of the spawn point marker in the new scene.</param>
    public static void SpawnPlayer(string marker) {
        //Debug.Log("Spawning player by marker");

        if (playerInThisScene != null) {
            //Debug.Log("Aborting spawning by marker");
            //a player already exists, do not spawn a new one. TODO maybe delete it and create a new one? idk anymore. Can this even happen?
            return;
        }

        GameObject named = GameObject.Find(marker);
        PlayerSpawnMarker m = named.GetComponent<PlayerSpawnMarker>();
        if (m != null) {
            //partyThisScene should already be set, we just need to spawn its avatar in.
            playerInThisScene = Instantiate(partyThisScene.avatar, named.transform.position, named.transform.rotation);
            playerInThisScene.gameObject.name = "Player";
        }

    }

    public static Party GetParty() {
        return partyThisScene;
    }

    public JObject SaveToFile() {

        JObject node = new JObject();
        JObject partiesNode = new JObject();
        node.Add("parties", partiesNode);
        for (int i = 0; i < parties.Length; i++) {
            Party party = parties[i];
            JObject partyNode = new JObject();
            partiesNode.Add(i.ToString(), partyNode);
            partyNode.Add("active",party == partyThisScene);
            partyNode.Add("party", party.SaveToFile());
        }

        return node;
    }

    public void LoadFromFile(JObject node) {

        //dispose of the player party
        //Destroy(playerInThisScene.gameObject);
        //playerInThisScene = null;
        //The player party is already destroyed by virtue of a NEW SCENE BEING LOADED


        JObject partiesNode = (JObject)(node.Property("parties").Value);
        int partyidx = -1;

        foreach (JProperty element in partiesNode.Properties()) {
            int index = int.Parse(element.Name);//TODO secure this

            JObject partyNode = (JObject)(element.Value);

            bool active = partyNode.Property("active").Value.ToObject<bool>();

            //Load the party
            parties[index].LoadFromFile((JObject)(partyNode.Property("party").Value));

            //set active party is this is the active party
            if (active) {
                //TODO what if multiple actives?
                //that's just an edited save genius.
                //How do we respond to that?

                partyidx = index;

            }

        }

        if (partyidx != -1) {
            SpawnPlayer(partyidx);
        } else {

            //There is no active party!
            //This can't happen though.
            throw new NotImplementedException();

        }
    }


}

[System.Serializable]
public class Party : ISaveable
{
    public Inventory inventory;
    public PlayerController avatar;
    //TODO: characters in party

    public void LoadFromFile(JObject node) {

        JObject inventoryNode = (JObject)(node.Property("inventory").Value);
        inventory.LoadFromFile(inventoryNode);

        //We don't need to load avatars as those are static
    }

    public JObject SaveToFile() {
        JObject ret = new JObject();

        ret.Add("inventory", inventory.SaveToFile());
        //we don't need to save the avatars, they shouldn't change.
        //TODO But some games may want the avatar to change... They'll have to transfer the inventory to a new party with the new avatar, it seems.

        return ret;
    }
}
