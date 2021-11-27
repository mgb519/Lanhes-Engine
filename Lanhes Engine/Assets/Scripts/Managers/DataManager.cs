using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json.Linq;
using System.Linq;
//using SerializableDictionary;

//TODO this class is getting a bit chunky
[RequireComponent(typeof(GameSceneManager)), RequireComponent(typeof(PartyManager)), RequireComponent(typeof(WindowManager)), RequireComponent(typeof(BattleManager))]
public class DataManager : MonoBehaviour, ISaveable
{

    [System.Serializable]
    public abstract class Database<TV> : EditableDictionary<string, TV>, ISaveable
    {
        void ISaveable.LoadFromFile(JObject node) {
            //clear the DB
            this.Clear();
            //load from the node
            JObject ret = node;
            foreach (JProperty entry in ret.Properties()) {
                string key = entry.Name;
                JToken value = entry.Value;
                this.Add(key, TokenToValue(value));
            }
        }

        internal abstract TV TokenToValue(JToken text);

        JObject ISaveable.SaveToFile() {
            JObject ret = new JObject();

            foreach (string key in this.Keys) {
                ret.Add(key, ValueToToken(this[key]));
            }

            return ret;
        }

        internal abstract JToken ValueToToken(TV value);
    };




    [System.Serializable]
    public class IntDatabase : Database<int>
    {
        internal override int TokenToValue(JToken text) {
            //TODO test for nulls... except that just means the save is modded.
            return text.ToObject<int>();
        }

        internal override JToken ValueToToken(int value) {
            return value;
        }
    };

    [SerializeField]
    private IntDatabase intData = new IntDatabase();

    [System.Serializable]
    public class StringDatabase : Database<string>
    {
        internal override string TokenToValue(JToken text) {
            return text.ToObject<string>();
        }

        internal override JToken ValueToToken(string value) {
            return value;
        }
    }

    [SerializeField]
    private StringDatabase stringData = new StringDatabase();


    [System.Serializable]
    public class BoolDatabase : Database<bool>
    {
        internal override bool TokenToValue(JToken text) {
            return text.ToObject<bool>();
        }

        internal override JToken ValueToToken(bool value) {

            return value;
        }
    }

    [SerializeField]
    private BoolDatabase boolData = new BoolDatabase();


    //[System.Serializable]
    public class NPCStates : EditableDictionary<(int, string), string> { } //TODO couldn't this just be ISaveable, and saved with the databases? On the other hand, this isn't stricty a DB so maybe don't place them together.


    //[SerializeField]
    private NPCStates npcStates = new NPCStates();



    //TODO maybe this field shouldn't be here of all places, this component is getting crowded already
    [SerializeField]
    private ItemDatabase itemDatabase;



    //TODO: would prefer a more concrete type than ISaveable this this is just used for saving anyway so no biggie rn
    private static (string, ISaveable)[] databases;
    private static (string, ISaveable)[] managers;


    public static DataManager instance = null;
    private static bool isLoading = false;

    public static bool IsLoading() { return isLoading; }
    void Awake() {

        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
            //register databases so that saveing and loading can see them
            databases = new (string, ISaveable)[] { ("ints", intData), ("strings", stringData), ("bools", boolData) };
            managers = new (string, ISaveable)[] { ("gamescenes", gameObject.GetComponent<GameSceneManager>()), ("parties", gameObject.GetComponent<PartyManager>()), ("windows", gameObject.GetComponent<WindowManager>()), ("battles", gameObject.GetComponent<BattleManager>()) }; //TODO is it even necessary to save some of these? (specifically, looking and window manager and scene manager)
        } else if (instance != this) {
            Destroy(gameObject);
        }
    }

    public static int GetInt(string key) { return GetVal(key, instance.intData); }
    public static void SetInt(string key, int newValue) { SetVal(key, newValue, instance.intData); }


    public static string GetString(string key) { return GetVal<string>(key, instance.stringData); }
    public static void SetString(string key, string newValue) { SetVal<string>(key, newValue, instance.stringData); }


    public static bool GetBool(string key) { return GetVal<bool>(key, instance.boolData); }
    public static void SetBool(string key, bool newValue) { SetVal<bool>(key, newValue, instance.boolData); }


    private static void SetVal<T>(string key, T newValue, IDictionary<string, T> dict) {
        if (dict.ContainsKey(key)) {
            dict[key] = newValue;
            return;
        } else {
            //give a warning, while a lot of the time this may be intentional, it may not always be, i.e typos
            //in that case, why not have initialisation be its own operation?
            Debug.LogWarning("Key " + key + " not found in " + typeof(int) + " database, creating with value " + newValue);
            dict.Add(key, newValue);
        }
    }

    //TODO: should be initialise the entry if it doesn't exist instead? or give a default value?
    private static T GetVal<T>(string key, IDictionary<string, T> dict) {
        if (dict.ContainsKey(key)) {
            return dict[key];
        } else {
            Debug.LogWarning("Key " + key + " not found in " + typeof(int) + " database!");
            throw new KeyNotFoundException();
        }

    }


    public static InventoryItem GetItemBySystemName(string systemName) {
        InventoryItem item = instance.itemDatabase.items.Find(x => x.systemName == systemName);
        if (item != null) {
            return item;
        } else {
            Debug.LogWarning("Item " + systemName + " not found, you must have an error in a script");
            throw new Exception();
        }

    }


    //TODO Method for culling dialogues that wont appear again (i.e chapters that you have passed)

    //this is be called when transitioning scenes, so that Inks are held in memory to flush
    /// <summary>
    /// Collect dialogues in this scene and hold them in memory
    /// </summary>
    public void RememberNPCStates() {
        //Get all NPCTraitSerialiser; i.e all objects that have an entrypoint to persistent data
        NPCTraitSerialiser[] dialogueEvents = FindObjectsOfType<NPCTraitSerialiser>();
        int currentSceneId = SceneManager.GetActiveScene().buildIndex;
        foreach (NPCTraitSerialiser e in dialogueEvents) {
            string saveString = e.Save();
            npcStates.Add((currentSceneId, e.name), saveString);
        }

    }

    /// <summary>
    /// Find dialogues that are held in memory and flush them into the scene. They will no longer be held in memory, but will be remembered when we leave the scene.
    /// </summary>
    public void RestoreNPCStates() {
        int sceneId = SceneManager.GetActiveScene().buildIndex;
        //get all npc names in this scene that have a remembered JSON
        ICollection<(int, string)> keys = npcStates.Keys;
        foreach ((int, string) key in keys) {
            if (key.Item1 != sceneId) { continue; } //if this NPC is not in the current scene, do not flush it

            GameObject npc = GameObject.Find(key.Item2);//TODO: what if no NPC was found? In that case, it implies we've spawned an NPC at runtime, which will break the whole thing...
            NPCTraitSerialiser e = npc.GetComponent<NPCTraitSerialiser>();
            e.Load(npcStates[key]);
            npcStates.Remove(key);
        }

    }


    /// <summary>
    /// Flush dialogues in memory to file, as well as dialogues in scene.
    /// </summary>
    private JObject SaveNPCStates() {
        //save Ink dialogues

        JObject root = new JObject();

        //Take NPC states in this scene and update the memories to reflect them
        RememberNPCStates();

        //store all  NPC states
        foreach ((int, string) key in npcStates.Keys) {
            //JObject content = new JObject();

            string sceneid = key.Item1.ToString();
            string npcid = key.Item2;
            Debug.Log("npc id = " + npcid);

            if (!root.Properties().Where(prop => prop.Name == sceneid).Any()) {
                root.Add(sceneid, new JObject());
            }
            JObject sceneProperty = (JObject)(root.Property(sceneid).Value);
            sceneProperty.Add(npcid, npcStates[key]);
            Debug.Log(npcStates[key]);
            Debug.Log(sceneProperty.ToString());

        }
        //Flush the NPC states back out of memories
        RestoreNPCStates();
        Debug.Log(root);
        return root;


    }


    /// <summary>
    /// Load all dialogues from file and place them in the Ink memory
    /// </summary>
    private void LoadNPCStates(JObject node) {
        npcStates.Clear();

        foreach (JProperty entry in node.Properties()) {
            int sceneId = int.Parse(entry.Name);

            JObject inner = (JObject)(entry.Value);
            foreach (JProperty l2entry in inner.Properties()) {
                string npcId = l2entry.Name;
                npcStates.Add((sceneId, npcId), l2entry.Value.ToObject<string>());
            }
        }
    }


    public JObject SaveToFile() {
        JObject root = new JObject();

        //store the scene we are in
        root.Add("scene", SceneManager.GetActiveScene().name); //TODO this could be an integer...

        //store DBs
        JObject dbs = new JObject();
        root.Add("dbs", dbs);
        foreach ((string, ISaveable) dbData in databases) {
            dbs.Add(dbData.Item1, dbData.Item2.SaveToFile());
        }

        //call saves of other managers
        JObject managersHolder = new JObject();
        root.Add("managers", managersHolder);
        foreach ((string, ISaveable) managerData in managers) {
            managersHolder.Add(managerData.Item1, managerData.Item2.SaveToFile());
        }

        //save NPC positions
        JObject npcHolder = new JObject();
        root.Add("npcs", npcHolder);

        //Get all NPCs that can move; i.e they have pawn movement.
        PawnMovementController[] npcs = FindObjectsOfType<PawnMovementController>();
        foreach (PawnMovementController n in npcs) {
            JObject npcNode = new JObject();
            GameObject npc = n.gameObject;
            npcHolder.Add(npc.name, npcNode);//TODO: this requires that NPCs have unique editor names.

            npcNode.Add("x", npc.transform.position.x);
            npcNode.Add("y", npc.transform.position.y);
            npcNode.Add("z", npc.transform.position.z);

            //TODO: NPC rotation? will I need that?
        }

        //save dialogues
        root.Add("dialogues", SaveNPCStates());

        return root;
    }

    public IEnumerator LoadBody(JObject root) {

        isLoading = true;



        //load the scene id and transition to that scene
        string sceneId = root.Property("scene").Value.ToObject<string>();
        GameSceneManager.StartLoadScene(sceneId);

        yield return new WaitUntil(() => !GameSceneManager.IsLoading());



        //restore DBs
        JObject dbs = root.Property("dbs").Value.ToObject<JObject>();
        foreach ((string, ISaveable) dbData in databases) {
            JObject dbNode = (JObject)(dbs.Property(dbData.Item1).Value);
            dbData.Item2.LoadFromFile(dbNode);
        }


        //load other managers
        JObject managersHolder = (JObject)(root.Property("managers").Value);
        foreach ((string, ISaveable) m in managers) {
            JObject managerNode = (JObject)(managersHolder.Property(m.Item1).Value);
            m.Item2.LoadFromFile(managerNode);
        }
        //player has been spawned correctly when loading PartyManager, so it exists in-scene.

        //load NPC positions
        JObject npcs = (JObject)(root.Property("npcs").Value);
        foreach (JProperty npcNode in npcs.Properties()) {
            string name = npcNode.Name;
            GameObject npc = GameObject.Find(name);
            if (npc == null) {
                Debug.LogError("NPC " + name + " not found!");
                continue;
            }
            JObject npcInnerNode = (JObject)(npcNode.Value);
            float x = npcInnerNode.Property("x").Value.ToObject<float>();
            float y = npcInnerNode.Property("y").Value.ToObject<float>();
            float z = npcInnerNode.Property("z").Value.ToObject<float>();
            //FIXME: player is not positioned correctly, and always returns to spawn point
            npc.transform.position = new Vector3(x, y, z);
        }


        //load Ink state
        LoadNPCStates((JObject)(root.Property("dialogues").Value));
        RestoreNPCStates();
        isLoading = false;
        //throw new NotImplementedException();
        Debug.Log("Finished load body");

    }


    public void LoadFromFile(JObject node) {
        Debug.Log("starting load");
        StartCoroutine(LoadBody(node));
    }
}
