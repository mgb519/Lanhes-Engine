using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;


//keeps a list of the parties, each of which has its own inventory
//most games will only have one so it's a little moot
public class PartyManager : MonoBehaviour, ISaveable
{
    public static PartyManager instance = null;

    //FIXME: this *should* be private, but the EDITOR needs this to be public, apprently labvelling it serializable is not enough.
    [SerializeField]
    public Party[] parties = new Party[7];// TODO couldn't this be a SerializableDisctionary and the player spanwer use a key rather than an index?

    private Party partyThisScene;
    //points towards the party in this scene
    //maybe this one isn't the best place to put it?
    public static PlayerController playerInThisScene;




    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Sets the current party to the one at index id, and spawns it into the scene at an arbitrary point. Position should be handled by a later call, such as the load function.
    /// </summary>
    /// <param name="id">index of the party being spawned in</param>
    public static void SpawnPlayer(int id)
    {
        Debug.Log("Spawning player by ID");
        if (playerInThisScene != null)
        {
            //a player already exists, do not spawn a new one. TODO maybe delete it and create a new one? idk anymore. Can this even happen?
            return;
        }

        instance.partyThisScene = instance.parties[id];

        playerInThisScene = Instantiate(instance.partyThisScene.avatar, instance.transform.position, instance.transform.rotation);
        playerInThisScene.gameObject.name = "Player";

        //Destroy any spawners in the level; if we let them live, then they will spawn in a party. This is a bad thing; we've just spawned in the correct one!
        //FIXME this isn't necessary anymore since they have no behaviour. Maybe use a Tag instead?
        PlayerSpawnMarker[] spawners = GameObject.FindObjectsOfType<PlayerSpawnMarker>();
        foreach (PlayerSpawnMarker spawner in spawners)
        {
            Destroy(spawner);
        }
    }

    /// <summary>
    /// Spawn the currently set player party in the current scene at the speceficied spawn point.
    /// </summary>
    /// <param name="marker">The name of the spawn point marker in the new scene.</param>
    public static void SpawnPlayer(string marker)
    {
        Debug.Log("Spawning player by marker");

        if (playerInThisScene != null)
        {
            Debug.Log("Aborting spawning by marker");
            //a player already exists, do not spawn a new one. TODO maybe delete it and create a new one? idk anymore. Can this even happen?
            return;
        }

        GameObject named = GameObject.Find(marker);
        PlayerSpawnMarker m = named.GetComponent<PlayerSpawnMarker>();
        if (m != null) {
            //partyThisScene should already be set, we just need to spawn its avatar in.
            playerInThisScene = Instantiate(instance.partyThisScene.avatar, named.transform.position, named.transform.rotation);
            playerInThisScene.gameObject.name = "Player";
        }

    }


    public Party GetParty()
    {
        return partyThisScene;
    }

    public XmlNode SaveToFile(XmlDocument doc)
    {

        XmlElement ret = doc.CreateElement("partyman");
        XmlElement partiesNode = doc.CreateElement("parties");
        ret.AppendChild(partiesNode);
        for (int i = 0; i < parties.Length; i++)
        {
            Party party = parties[i];
            XmlElement partyNode = doc.CreateElement("party");
            partiesNode.AppendChild(partyNode);
            partyNode.SetAttribute("idx", i.ToString());
            partyNode.SetAttribute("active", (party == partyThisScene).ToString());
            partyNode.AppendChild(party.SaveToFile(doc));
        }

        return ret;
    }

    public void LoadFromFile(XmlNode node)
    {

        //dispose of the player party
        //Destroy(playerInThisScene.gameObject);
        //playerInThisScene = null;
        //The player party is already destroyed by virtue of a NEW SCENE BEING LOADED



        XmlNode baseNode = node["partyman"];
        XmlNode partiesNode = baseNode["parties"];
        int partyidx = -1;
        foreach (XmlElement element in partiesNode.ChildNodes)
        {
            int index = int.Parse(element.GetAttribute("idx"));//TODO secure this
            bool active = Boolean.Parse(element.GetAttribute("active"));//TODO secure this

            //Load the party
            parties[index].LoadFromFile(element);

            //set active party is this is the active party
            if (active)
            {
                //TODO what if multiple actives?
                //that's just an edited save genius

                partyidx = index;

            }

        }

        if (partyidx != -1)
        {
            SpawnPlayer(partyidx);
        }
        else
        {

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

    public void LoadFromFile(XmlNode node)
    {
        //TODO
    }

    public XmlNode SaveToFile(XmlDocument doc)
    {
        XmlNode ret = doc.CreateElement("party");



        return ret;
    }
}
