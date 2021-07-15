using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BattleManager : MonoBehaviour, ISaveable
{



    static BattleManager instance = null;

    public string battleScene; //TODO: for now, we only assume one kind of battle. If we wish to add more (i.e minigames) then we'd need to expand this

    private bool inBattle = false;

    private BattleResult lastResult; //TODO: maybe this should be restored? It *shouldn't need to*, I think, but maybe just for safety?

    private GameObject playerCam; //FIXME this is cached to get around how Unity's getComponent works; we need to reactivate a deactivated object when returning to the scene (the camera), but we cannot do this, because GetComponent does not get deactivated gameObjects
                                  // see-> 
                                  /*
                                     //FIXME: THIS DOESN'T WORK! becuase GetComponent only gets ACTIVE objects, but we just disabled the object with the camera. I guess we need to cache the gameObject or something.
                                      Camera camera = player.GetComponentInChildren<Camera>();
                                      camera.gameObject.SetActive(true);
                                  */
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


    public static BattleResult GetResultOfLastBattle()
    {
        Debug.Log("Got result of last battle...(" + instance.lastResult + ")");
        return instance.lastResult;
    }


    public static void StartBattle(IOpponentGroup enemies)
    {
        //pause the game world
        instance.inBattle = true;

        //disable the player's camera, since the battle scene will have a camera of its own
        GameObject player = PartyManager.playerInThisScene.gameObject;
        Camera camera = player.GetComponentInChildren<Camera>();
        instance.playerCam = camera.gameObject;
        instance.playerCam.SetActive(false);

        //load in the new scene
        SceneManager.LoadSceneAsync(instance.battleScene, LoadSceneMode.Additive);
        //TODO put a loading screen up

    }

    /// <summary>
    /// Called by the battle scene when the battle ends. Disposes of the battle scen and restores control to the overworld.
    /// </summary>
    public static void EndBattle(BattleResult result)
    {
        //TODO      

        //restore the player camera
        GameObject player = PartyManager.playerInThisScene.gameObject;

        /*
        //FIXME: THIS DOESN'T WORK! becuase GetComponent only gets ACTIVE objects, but we just disabled the object with the camera. I guess we need to cache the gameObject or something.
        Camera camera = player.GetComponentInChildren<Camera>();
        camera.gameObject.SetActive(true);
        */
        instance.playerCam.SetActive(true);
        
        //communicate state of last battle (i.e for puposes of ink)
        instance.lastResult = result;

        //dispose of the battle scene
        SceneManager.UnloadSceneAsync(instance.battleScene);
        //TODO put loading screen up until the scene fully unloads



        instance.inBattle = false;


    }

    internal static bool InBattle()
    {
        return instance.inBattle;
    }

    public XmlNode SaveToFile(XmlDocument doc)
    {
        XmlElement ret = doc.CreateElement("battle");


        return ret;
    }

    public void LoadFromFile(XmlNode node)
    {
        //nothing is saved, so nothing is loaded!
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

