using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using System;


public class DialogueEvent : MonoBehaviour {


    GameObject player;


    public List<ShopData> shops;

    // Set this file to your compiled json asset
    public TextAsset inkAsset;

    // The ink story that we're wrapping
    Story _inkStory;



    //TODO: we need to reset the Ink story to its head node when the event is over generally
    //TODO: serialise state like "which branches should be removed since they've been traversed once"
    //TODO: at what point do I realise that what I should do is just block user interaction with an "event is occuring" flag
    void Awake() {
        player = GameObject.FindGameObjectWithTag("Player");
        _inkStory = new Story(inkAsset.text);

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
                    } else {
                        //function not found!
                        Debug.LogWarning("Function " + function + " not found, script " + inkAsset.name);
                    }
                }

            } else if (_inkStory.currentChoices.Count > 0) {
                //we have a choice window
                //TODO: ths currently only works for string selection, figure out a syntax for other selections
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
                Debug.Log("Finished event");
                //I suppose this means we reached the end of ths script
                //TODO: reset script?
                break;

            }


        }
        yield break;

    }





}
