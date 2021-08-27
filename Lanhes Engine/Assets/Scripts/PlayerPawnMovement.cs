﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerController))]
public class PlayerPawnMovement : PawnMovementController {

    public bool blocked = false;

    internal override Vector3 GetInput() {
        //TODO: we have an issue where there is one frame where we can continue play between a dialogue being opened and the next, in which we can move...
        //Cannot reproduce the above. If no one can, then I assume that this was solved during work on the dialogue system.
        if (!blocked) {
            int horizontalMove = Mathf.RoundToInt(Input.GetAxis("Horizontal"));
            int verticalMove = Mathf.RoundToInt(Input.GetAxis("Vertical"));
            return new Vector3(horizontalMove, 0, verticalMove);

    } else {
            //freeze
            //TODO: the movement issue could be simplified if we made movement a product of root motion
            //fuck root motion though
            return Vector3.zero;

        }

    }

}
