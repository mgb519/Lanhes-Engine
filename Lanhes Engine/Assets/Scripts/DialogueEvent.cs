using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using System;


public class DialogueEvent : MonoBehaviour {

    //TODO: serialisation
    GameObject player;


    public List<ShopData> shops;

    // Set this file to your compiled json asset
    public TextAsset inkAsset;

    // The ink story that we're wrapping
    Story _inkStory;

    private List<WaypointFollowerMovementController> overridenNPCs = new List<WaypointFollowerMovementController>();


    //TODO: we need to reset the Ink story to its head node when the event is over generally
    //TODO: serialise state like "which branches should be removed since they've been traversed once"
    //TODO: at what point do I realise that what I should do is just block user interaction with an "event is occuring" flag
    void Awake() {
        player = GameObject.FindGameObjectWithTag("Player");
        _inkStory = new Story(inkAsset.text);

        //bind some getter functions
        _inkStory.BindExternalFunction("getInt", (string key) => DataManager.instance.GetInt(key));
        _inkStory.BindExternalFunction("getStr", (string key) => DataManager.instance.GetString(key));
        _inkStory.BindExternalFunction("getBol", (string key) => DataManager.instance.GetBool(key));

    }

    //TODO: this script is only ever caled once? This might be due to Ink not resetting
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

                if (!command.StartsWith("$")) {
                    //this is a dialogue
                    //TODO: get name and picture, etc
                    WindowManager.CreateStringWindow(command);
                    yield return new WaitUntil(() => WindowManager.instance.ContinuePlay());
                } else {
                    // parse command
                    //TODO: this won't really like strings with spaces as arguments..
                    string[] args = command.Split(' ');
                    string function = args[0];
                    if (function == "$SHOP") {
                        //TODO: make safer for debugging purposes
                        int index = int.Parse(args[1]);
                        WindowManager.CreateShopWindow(shops[index].buyPrices, shops[index].sellPrices, player.GetComponent<PlayerController>().inventory);
                        yield return new WaitUntil(() => WindowManager.instance.ContinuePlay());
                    } else if (function == "$NPCWALK") {
                        //NPC walks to positon
                        string npcName = args[1];
                        GameObject g = GameObject.Find(npcName);
                        if (g == null) { Debug.LogWarning("script " + inkAsset.name + ", did not find NPC " + npcName); break; }
                        WaypointFollowerMovementController controller = g.GetComponent<WaypointFollowerMovementController>();
                        //TODO: what about the player?
                        //TODO: I suppose the player has a different scripted movement fucntion, they are special after all
                        if (g == null) { Debug.LogWarning("script " + inkAsset.name + ", NPC " + npcName + " can't be directed"); break; }
                        Vector3 w = new Vector3(int.Parse(args[2]), int.Parse(args[3]), int.Parse(args[2]));
                        overridenNPCs.Add(controller);
                        controller.SetWaypoint(w);
                        player.GetComponent<PlayerPawnMovement>().blocked = true;
                        yield return new WaitUntil(() => controller.ReachedWaypoint());
                        player.GetComponent<PlayerPawnMovement>().blocked = false;              //TODO: do we want a movement to be run asynchronously?

                    } else if (function == "$NPCTELE") {
                        //NPC is teleported to location
                        string npcName = args[1];
                        GameObject g = GameObject.Find(npcName);
                        if (g == null) { Debug.LogWarning("script " + inkAsset.name + ", did not find NPC " + npcName); break; }
                        Vector3 w = new Vector3(int.Parse(args[2]), int.Parse(args[3]), int.Parse(args[2]));
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





}
