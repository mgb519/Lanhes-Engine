﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json.Linq;


public class BattleManager : MonoBehaviour, ISaveable
{



    static BattleManager instance = null;

    public string battleScene; //TODO: for now, we only assume one kind of battle. If we wish to add more (i.e minigames) then we'd need to expand this

    private bool inBattle = false;

    private BattleResult lastResult; //TODO: maybe this should be restored? It *shouldn't need to*, I think, but maybe just for safety?

    [SerializeField]
    private LoadingScreen battleLoadingScreen;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            battleLoadingScreen.gameObject.SetActive(false); //We don't need a loading screen on startup
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }


    public static BattleResult GetResultOfLastBattle()
    {
        Debug.Log("Got result of last battle...(" + instance.lastResult + ")");
        return instance.lastResult;
    }


    public static void StartBattle(IOpponentGroup enemies)
    {
        //pause the game world
        instance.inBattle = true;

       
        //load in the new scene
        instance.StartCoroutine(instance.LoadInBattle(enemies));
    }


    private IEnumerator LoadInBattle(IOpponentGroup enemies) {
        //put a loading screen up
        instance.battleLoadingScreen.gameObject.SetActive(true);
        AsyncOperation op = SceneManager.LoadSceneAsync(instance.battleScene, LoadSceneMode.Additive);
        while (!op.isDone) {
            battleLoadingScreen.UpdateProgress(op.progress);
            yield return null;
        }

        instance.battleLoadingScreen.gameObject.SetActive(false);

        //TODO we can have some frames with no camera, it seems, probably since we deactivate the player cam first.

    }

    /// <summary>
    /// Called by the battle scene when the battle ends. Disposes of the battle scen and restores control to the overworld.
    /// </summary>
    public static void EndBattle(BattleResult result)
    {             
        //communicate state of last battle (i.e for puposes of ink)
        instance.lastResult = result;

        //dispose of the battle scene
        instance.StartCoroutine(instance.UnloadBattle());
        instance.inBattle = false;
    }



    private IEnumerator UnloadBattle() {
        //put a loading screen up
        instance.battleLoadingScreen.gameObject.SetActive(true);
        AsyncOperation op = SceneManager.UnloadSceneAsync(instance.battleScene);
        while (!op.isDone) {
            battleLoadingScreen.UpdateProgress(op.progress);
            yield return null;
        }

        instance.battleLoadingScreen.gameObject.SetActive(false);

    }

    internal static bool InBattle()
    {
        return instance.inBattle;
    }

    public JObject SaveToFile()
    {
        JObject ret = new JObject();
        return ret;
    }

    public void LoadFromFile(JObject node)
    {
        //nothing is saved, so nothing is loaded!
    }


    /// <summary>
    /// Converts the given BattleResult into its string representation for use in Ink scripts. Must be defined on a per-game basis.
    /// </summary>
    /// <returns>The battleResults string representation.</returns>
    /// <param name="battleResult">Battle result to convert</param>
    internal static string BattleResultAsString(BattleResult battleResult)
    {
        switch (battleResult)
        {
            case BattleResult.Victory:
                return "victory";
            case BattleResult.Loss:
                return "loss";
            default:
                return null; //This should never happen. And so we can safely return null, thus breaking the game, clearly. (I mean, yeah, it alerts the dev to a problem at least.)
        }
    }
}

//TODO move these to thier own file

//TODO This would be game-specific thing so should be overriden
public interface IOpponentGroup
{

}

public enum BattleResult : int
{
    //TODO maybe this should be a ScriptableObject or something? either way every game must override this.
    Victory = 1,
    Loss = 2
    //TODO We explicitly specify values for use in Ink scripts. This is unfortunate, can we fix this?
}

