using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//TODO finish this
//This should be overriden by the game, depending on how a save slot looks.
public class SaveInstanceButton : MonoBehaviour
{

    private string path;
    public void Clicked() {
        SendMessageUpwards("SaveSelected", path);
    }

    public void SetPath(string newPath) {
        path = newPath;
        //TODO populate the panel
    }

}
