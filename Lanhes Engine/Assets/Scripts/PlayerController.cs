using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    // Start is called before the first frame update
    
    // Update is called once per frame
    void Update() {
        if (WindowManager.instance.ContinuePlay()) {
            //TODO: this timeScale check is not a good idea, due to floating points. Is it necessary?
            if (Input.GetButtonDown("Pause") && Time.timeScale != 0 && WindowManager.instance.ContinuePlay()) {
                WindowManager.CreatePauseWindow();

            }
        }
    }


    public Inventory GetInventory() {
        return PartyManager.instance.GetParty().inventory;
    }

}
