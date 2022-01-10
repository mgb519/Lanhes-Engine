using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


    private bool eventRunning=false;

    // Update is called once per frame
    // TODO do not allow opening of pause menu when event is running
    void Update() {
        if (WindowManager.ContinuePlay() && !eventRunning) {
            if (WindowManager.ContinuePlay() && !GameSceneManager.IsLoading() && !DataManager.IsLoading() && !BattleManager.InBattle()) {
                if (Input.GetButtonDown("Pause") || Input.GetMouseButtonDown(1)) {
                    WindowManager.CreatePauseWindow();
                }
            }
        }
    }


    public Inventory GetInventory() {
        return PartyManager.GetParty().inventory;
    }
    public void SetEventRunning() {
        gameObject.GetComponent<PlayerPawnMovementController>().blocked = true; //FIXME cache the movement controller
        eventRunning = true;
    }
    public void UnsetEventRunning() {
        gameObject.GetComponent<PlayerPawnMovementController>().blocked = false;
        eventRunning = false;
    }

    public bool IsEventRunning() { return eventRunning; }
}
