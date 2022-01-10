using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class MapScript :MonoBehaviour
{
    public abstract IEnumerator Action(); 

    public IEnumerator Call() {
        PartyManager.playerInThisScene.SetEventRunning();
        yield return Action(); //wait for Action to finish
        PartyManager.playerInThisScene.UnsetEventRunning();      

    }

    //These are functions that are called by DialogueEvent. They are placed here so that you can create your own events in C# using those building blocks.

     

}
