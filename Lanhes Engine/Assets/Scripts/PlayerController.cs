using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    // Start is called before the first frame update

    // Update is called once per frame
    void Update() {
        if (WindowManager.instance.ContinuePlay()) {
            if(WindowManager.instance.ContinuePlay() && !GameSceneManager.IsLoading() && !DataManager.IsLoading() && !BattleManager.InBattle()) {
                if (Input.GetButtonDown("Pause"))
                {
                    WindowManager.CreatePauseWindow();
                }
            }
        }
    }


    public Inventory GetInventory() {
        return PartyManager.GetParty().inventory;
    }

}
