using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Trigger : MonoBehaviour
{
    [SerializeField,SerializeReference]
    private MapScript scriptToCall;

    [SerializeField]
    private bool haltsPlayerMovement=true;


    public void Action() {
        if (haltsPlayerMovement) {
            PartyManager.playerInThisScene.GetComponent<PlayerPawnMovementController>().HaltMovement();
        }
        scriptToCall.Action();
    }
}
