using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardMovementPlayerController : PlayerPawnMovementController
{
    internal override Vector3 GetPlayerInput() {
        int horizontalMove = Mathf.RoundToInt(Input.GetAxis("Horizontal"));
        int verticalMove = Mathf.RoundToInt(Input.GetAxis("Vertical"));
        Vector3 dir = new Vector3(horizontalMove, 0, verticalMove);
        //raycast for use
        //On the other hand, why is this here? This function is supposed to get input, not carry out actions...
        if (Input.GetButtonDown("Use")) {
            RaycastHit hitInfo;
            //TODO: what if we are standing still? Get ray will be in direction 0.
            if (Physics.Raycast(transform.position, dir, out hitInfo, useDistance)) {
                UseTrigger trigger = hitInfo.collider.gameObject.GetComponent<UseTrigger>();
                if (trigger != null) {
                    trigger.Used();
                }
            }
        }

        return dir;
    }
}
