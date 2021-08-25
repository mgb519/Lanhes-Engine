using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using System;


public class DialogueEvent : MonoBehaviour {

    //TODO: serialisation
    GameObject player { get {
           return PartyManager.playerInThisScene.gameObject;
    } }


    public ShopData[] shops;
    public IOpponentGroup[] enemyParties;

    // Set this file to your compiled json asset
    public TextAsset inkAsset;

    // The ink story that we're wrapping
    Story _inkStory;

    //FIXME: what does this do again? Do we save and restore it or something?
    private List<WaypointFollowerMovementController> overridenNPCs = new List<WaypointFollowerMovementController>();


    //TODO: we need to reset the Ink story to its head node when the event is over generally
    //TODO: serialise state like "which branches should be removed since they've been traversed once"
    //TODO: at what point do I realise that what I should do is just block user interaction with an "event is occuring" flag
    void Awake() {
        _inkStory = new Story(inkAsset.text);

        //bind some getter functions
        _inkStory.BindExternalFunction("getInt", (string key) => DataManager.instance.GetInt(key));
        _inkStory.BindExternalFunction("getStr", (string key) => DataManager.instance.GetString(key));
        _inkStory.BindExternalFunction("getBol", (string key) => DataManager.instance.GetBool(key));
        _inkStory.BindExternalFunction("getBattleResult", () => (int)BattleManager.GetResultOfLastBattle());
    }


    public void OnTriggerEnter(Collider collision) {
    if (collision.gameObject == player) {
        //TODO: have script triggers and scripts be seperate
        StartCoroutine(HandleScript());
        }
    }

    IEnumerator HandleScript() {
        while (true) {
            if (_inkStory.canContinue) {
                //fetch the next window
                string command = _inkStory.Continue();
                //Debug.Log(command);
                if (!command.StartsWith("$")) {
                    Debug.Log("Showing dialogue"+command);
                    //this is a dialogue
                    //TODO: get name and picture, etc
                    WindowManager.CreateStringWindow(command);
                    yield return new WaitUntil(() => WindowManager.instance.ContinuePlay());
                } else {
                    // parse command
                    //TODO: this won't really like strings with spaces as arguments..
                    //TODO: maybe split by comma?
                    string[] args = command.Split(' ');
                    string function = args[0];
                    if (function == "$SHOP") {
                        //TODO: make safer for debugging purposes; this will fail if int.Parse fails. Although that may be a good thing, it's a clear indicator of a malformed script.
                        int index = int.Parse(args[1]);
                        WindowManager.CreateShopWindow(shops[index].buyPrices, shops[index].sellPrices, PartyManager.instance.GetParty().inventory);
                        yield return new WaitUntil(() => WindowManager.instance.ContinuePlay());
                    } else if (function == "$NPCWALK") {
                        //NPC walks to positon
                        string npcName = args[1];   
                        GameObject g = GameObject.Find(npcName);
                        if (g == null) { Debug.LogWarning("script " + inkAsset.name + ", did not find NPC " + npcName); break; }
                        WaypointFollowerMovementController controller = g.GetComponent<WaypointFollowerMovementController>();
                        //TODO: what about the player?
                        //TODO: I suppose the player has a different scripted movement fucntion, they are special after all
                        //TODO: or maybe waypoint following is a behavoir that other pawn movement controllers should implement; i.e an interface
                        if (g == null) { Debug.LogWarning("script " + inkAsset.name + ", NPC " + npcName + " can't be directed"); break; }
                        Vector3 w = new Vector3(float.Parse(args[2]), float.Parse(args[3]), float.Parse(args[4])); 
                        overridenNPCs.Add(controller);
                        controller.SetWaypoint(w);
                        player.GetComponent<PlayerPawnMovement>().blocked = true;
                        //TODO: getting the Y ever so slightly wrong can result in this never being triggered, as the agent cannot actually move freely on Y.
                        yield return new WaitUntil(() => controller.ReachedWaypoint());
                        player.GetComponent<PlayerPawnMovement>().blocked = false;  //TODO: do we want a movement to be run asynchronously? i.e we would move to the next line as the NPC moves
                        controller.FreeWaypoint();  //TODO presumably, we may want the NPC to stay in pace. maybe then we shouldn't free the waypoint? In this case, the waypoint needs to be freed up at *some* point. Except for cases where we alter patrol paths?
                        overridenNPCs.Remove(controller); //TODO is this correct? unsure what overrdien NPCs variable was for
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
                        DataManager.instance.SetInt(key, val);

                    } else if (function == "$SETSTR") {
                        string key = args[1];
                        //TODO: make safer for debugging purposes
                        string val = args[2];
                        DataManager.instance.SetString(key, val);

                    } else if (function == "$SETBOL") {
                        string key = args[1];
                        //TODO: make safer for debugging purposes
                        bool val = bool.Parse(args[2]);
                        DataManager.instance.SetBool(key, val);
                    }
                    else if (function == "$BATTLE")
                    {
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
                    }
                    else {
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

                SelectionWindow s = WindowManager.CreateStringSelection(choicesAsString);
                yield return new WaitUntil(() => WindowManager.instance.ContinuePlay());
                string selected = ((SelectableString)(s.selected)).data;
                //TODO: sure this could be optimised
                int index = choices.Find(x => x.text == selected).index;
                _inkStory.ChooseChoiceIndex(index);

            } else {
                Debug.Log("Finished event "+inkAsset.name);
                //reached and of script, return to the beginning for the next time the player triggers it
                _inkStory.ChoosePathString("head");
                break;

            }


        }
        yield break;

    }

    public string Save()
    {
        return _inkStory.state.ToJson();
    }

    public void Load(string json) {
        _inkStory.state.LoadJson(json);
    }
}
