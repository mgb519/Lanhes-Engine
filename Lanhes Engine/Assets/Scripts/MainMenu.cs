using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class MainMenu : MonoBehaviour
{


    public int partyID;
    public string firstScene;
    public string firstPawnPoint;
    public void NewGame()
    {
        PartyManager.SpawnPlayer(partyID);
        //FIXME: the way this works, we get a warning about having multiple listeners in the scene. This is becuase the main menu scene has a camera with a listener in it too, and the player coexists with it. This goes away after reaching the next scene, and the only consequence will be main menu sound being screwed up for a few frames.
        GameSceneManager.StartLoadScene(firstScene, firstPawnPoint);

    }


    public void LoadGame()
    {

        WindowManager.CreateLoadWindow(true);
    }
}
