using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerController))]
public abstract class PlayerPawnMovementController : PawnMovementController
{

    public bool blocked = false;


    [SerializeField]
    internal float useDistance;
    internal override  Vector3 GetInput() {
        //TODO: we have an issue where there is one frame where we can continue play between a dialogue being opened and the next, in which we can move...
        //Cannot reproduce the above. If no one can, then I assume that this was solved during work on the dialogue system.
        if (!blocked) {
            Vector3 input = GetPlayerInput();
            return input;
        } else {
            //freeze
            //TODO: the movement issue could be simplified if we made movement a product of root motion
            //fuck root motion though
            return Vector3.zero;

        }

    }


    internal abstract Vector3 GetPlayerInput();
}
