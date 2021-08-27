using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(Collider))]
public class EnterZoneTrigger : MonoBehaviour
{

    public MapScript scriptToCall;


    public void OnTriggerEnter(Collider collision) {
        if (collision.gameObject == PartyManager.playerInThisScene.gameObject) {
            scriptToCall.Action();
        }
    }
}
