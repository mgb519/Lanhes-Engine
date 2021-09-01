using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO this class is unfinshed
public class SaveMenu : MenuWindow
{

    public SaveInstanceButton template;

    void Awake() {
        //Time.timeScale = 0f;

        //TODO populate the list
    }


    public void SaveSelected(string path) {
        //TODO save over the selected slot
    }


    public void SaveNew() { 
        //TODO save a new slot
        //TODO what if we want to name,say, slots? 
    }

}
