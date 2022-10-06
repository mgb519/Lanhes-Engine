using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using System;
using Newtonsoft.Json.Linq;
using System.Linq;

public class DialogueEvent : MapScript, NPCTrait
{

    GameObject player {
        get {
            return PartyManager.playerInThisScene.gameObject;
        }
    }

    [SerializeField]
    private ShopData[] shops;
    [SerializeField]
    private IOpponentGroup[] enemyParties;

    // Set this file to your compiled json asset
    public TextAsset inkAsset;

    // The ink story that we're wrapping
    Story _inkStory;


    private HashSet<PawnMovementController> overridenNPCs = new HashSet<PawnMovementController>();


    //TODO: we need to reset the Ink story to its head node when the event is over generally
    //TODO: serialise state like "which branches should be removed since they've been traversed once"
    //TODO: at what point do I realise that what I should do is just block user interaction with an "event is occuring" flag
    void Awake() {
        _inkStory = new Story(inkAsset.text);

        //bind some getter functions
        _inkStory.BindExternalFunction("getInt", (string key) => DataManager.GetInt(key));
        _inkStory.BindExternalFunction("getStr", (string key) => DataManager.GetString(key));
        _inkStory.BindExternalFunction("getBol", (string key) => DataManager.GetBool(key));
        _inkStory.BindExternalFunction("getBattleResult", () => (int)BattleManager.GetResultOfLastBattle());
    }


    public override IEnumerator Action() {
        yield return HandleScript();
    }

    IEnumerator HandleScript() {
        string promptBuffer = "";
        Debug.Log("entered script");
        while (true) {
            if (_inkStory.canContinue) {
                string command = _inkStory.Continue();
                //fetch the next window
                //Debug.Log(command);
                if (!command.StartsWith("$")) {
                    Debug.Log("Showing dialogue:" + command);
                    //this is a dialogue
                    //TODO: get name and picture, etc
                    WindowManager.CreateStringWindow(command.Trim(), null);
                    yield return new WaitUntil(() => WindowManager.ContinuePlay());
                } else {
                    // parse command
                    //TODO: this won't really like strings with spaces as arguments..
                    //TODO: maybe split by comma?
                    command = command.Trim();
                    string[] args = command.Split(' ');
                    string function = args[0];
                    if (function == "$SHOP") {
                        //TODO: make safer for debugging purposes; this will fail if int.Parse fails. Although that may be a good thing, it's a clear indicator of a malformed script.
                        int index = int.Parse(args[1]);
                        WindowManager.CreateShopWindow(shops[index].buyPrices, shops[index].sellPrices, PartyManager.GetParty().inventory, null);
                        yield return new WaitUntil(() => WindowManager.ContinuePlay());
                    } else if (function == "$NPCWALK") {
                        //NPC walks to positon

                        //TODO what if some other event triggers while the NPC is walking? This leaves that option open...
                        string npcName = args[1];
                        GameObject g = GameObject.Find(npcName);
                        if (g == null) { Debug.LogWarning("script " + inkAsset.name + ", did not find NPC " + npcName); break; }
                        PawnMovementController controller = g.GetComponent<PawnMovementController>();
                        if (g == null) { Debug.LogWarning("script " + inkAsset.name + ", NPC " + npcName + " can't be directed"); break; }
                        Vector3 w = new Vector3(float.Parse(args[2]), float.Parse(args[3]), float.Parse(args[4]));
                        overridenNPCs.Add(controller);
                        controller.AddWaypoint(w);
                        Debug.Log("told " + npcName + "to move to " + w.ToString());


                    } else if (function == "$WAIT") {
                        //we wait for all NPCs that are currently being directed to finish thier movement.

                        //TODO: getting the Y ever so slightly wrong can result in this never being triggered, as the agent cannot actually move freely on Y.
                        yield return new WaitUntil(() => overridenNPCs.All(x => x.ClearedPath()));
                        foreach (PawnMovementController controller in overridenNPCs) {
                            controller.Release();  //TODO presumably, we may want the NPC to stay in pace. maybe then we shouldn't free the waypoint, in case its normal AI takes hold? In this case, the waypoint needs to be freed up at *some* point. Except for cases where we alter patrol paths?
                        }
                        overridenNPCs.Clear();

                    } else if (function == "$NPCTELE") {
                        //NPC is teleported to location
                        string npcName = args[1];
                        GameObject g = GameObject.Find(npcName);
                        if (g == null) { Debug.LogWarning("script " + inkAsset.name + ", did not find NPC " + npcName); break; }
                        Vector3 w = new Vector3(float.Parse(args[2]), float.Parse(args[3]), float.Parse(args[4]));
                        g.transform.position = w;
                        //happens instantly, do not need to wait
                    } else if (function == "$SETINT") {
                        string key = args[1];
                        //TODO: make safer for debugging purposes
                        int val = int.Parse(args[2]);
                        DataManager.SetInt(key, val);

                    } else if (function == "$SETSTR") {
                        string key = args[1];
                        //TODO: make safer for debugging purposes
                        string val = args[2];
                        DataManager.SetString(key, val);

                    } else if (function == "$SETBOL") {
                        string key = args[1];
                        //TODO: make safer for debugging purposes
                        bool val = bool.Parse(args[2]);
                        DataManager.SetBool(key, val);
                    } else if (function == "$BATTLE") {
                        Debug.Log("starting battle...");
                        /*
                        int index = int.Parse(args[1]);
                        IOpponentGroup enemies = enemyParties[index];

                        BattleManager.StartBattle(enemies);*/
                        //TODO restore the above instead
                        BattleManager.StartBattle(null);
                        yield return new WaitUntil(() => !BattleManager.InBattle()); //wait for the battle to finish before continuing the script.
                        //BATTLE directives should be followed by a choice function, with an outcome for each BattleResult
                        List<Choice> paths = _inkStory.currentChoices;
                        string compare = BattleManager.BattleResultAsString(BattleManager.GetResultOfLastBattle());
                        Choice result = paths.Find(x => x.text == compare);
                        _inkStory.ChooseChoiceIndex(result.index);
                    } else if (function == "$PROMPT") {
                        promptBuffer = command.Remove(0,7).TrimStart();
                    } else {
                        //function not found!
                        Debug.LogWarning("Function " + function + " not found, script " + inkAsset.name);
                    }
                }

            } else if (_inkStory.currentChoices.Count > 0) {
                //we have a choice window
                //TODO: ths currently only works for string selection, as that is how Ink works normally. Figure out a syntax for other selections
                List<Choice> choices = _inkStory.currentChoices;
                List<string> choicesAsString = new List<string>();
                foreach (Choice c in choices) { choicesAsString.Add(c.text); }

                SelectionWindow s = WindowManager.CreateStringSelection(choicesAsString, null, promptBuffer); //TODO some way of getting a prompt from Ink
                yield return new WaitUntil(() => WindowManager.ContinuePlay());
                string selected = ((SelectableString)(s.selected)).data.GetLocalizedString();
                //TODO: sure this could be optimised
                int index = choices.Find(x => x.text == selected).index;
                _inkStory.ChooseChoiceIndex(index);

            } else {
                Debug.Log("Finished event " + inkAsset.name);
                //reached and of script, return to the beginning for the next time the player triggers it
                _inkStory.ChoosePathString("head");
                break;

            }


        }
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
            Debug.LogError("NPC object "+this.name+" has no NPCTraitSerialiser!");
        }
    }

    #endregion boilerplate

    //We do not save the shops or enemyParties variables, since they do not change.
 
    public JObject Save() {
        return JObject.Parse(_inkStory.state.ToJson());
    }

    public void Load(JObject saveString) {
        _inkStory.state.LoadJson(saveString.ToString());        
    }
}
