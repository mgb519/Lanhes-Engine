using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(PlayerController))]
public abstract class PlayerPawnMovementController : PawnMovementController //allows the player to follow waypoitns and thus be directed by events
{

    public bool blocked = false;


    [SerializeField]
    internal float useDistance;
    internal override Vector3 GetInput() {
        //TODO: we have an issue where there is one frame where we can continue play between a dialogue being opened and the next, in which we can move...
        //Cannot reproduce the above. If no one can, then I assume that this was solved during work on the dialogue system.
        if (!blocked && !overriden) {
            Vector3 input = GetPlayerInput();
            return input;
        } else if (overrideWaypoints.Count > 0) {
            while (overrideWaypoints.Count > 0 && ReachedWaypoint(GetCurrentWaypoint())) {
                overrideWaypoints.Dequeue();
            }

            if (overrideWaypoints.Count == 0) { return Vector3.zero; } else { return GetCurrentWaypoint() - transform.position; }
        } else {
            //freeze
            //TODO: the movement issue could be simplified if we made movement a product of root motion
            //fuck root motion though
            return Vector3.zero;

        }

    }


    abstract public void HaltMovement();

    internal abstract Vector3 GetPlayerInput();
}
