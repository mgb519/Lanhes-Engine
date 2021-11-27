using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
//TODO should this be ISaveable to save the overriden status? On the other hand, currently overrides only take place within dialogues; and you can't save mid-dialogue.
public class WaypointFollowerMovementController : PawnMovementController
{
    //TODO actually TEST the waypoint behaviour.
    public Queue<Vector3> waypoints = new Queue<Vector3>();//TODO show in editor

    private Vector3 overrideWaypoint;
    private bool overriden = false;


    [SerializeField]
    private float closeEnoughDist;

    internal override Vector3 GetInput() {

        if (!overriden) {
            if (waypoints.Count == 0) { return Vector3.zero; }
            //patrol along path
            Vector3 next = waypoints.Peek();
            while (ReachedWaypoint(next)) {
                next = waypoints.Dequeue();
                waypoints.Enqueue(next);
                next = waypoints.Peek();
            }


        }


        return GetInputToPosition();

    }




    private Vector3 GetInputToPosition() {
        //TODO this snap can be pretty ugly
        //if ((transform.position - GetCurrentWaypoint()).sqrMagnitude <= closeEnoughDist * closeEnoughDist) {
        //    //if we can reach in one frame, snap to it, this should remove jitter
        //    //TODO this is not the cleanest of solutions, since we will not be animating for one frame, but it gets us to the desitination. Not a priority to fix, merely wrong.
        //    //TODO the snap is visible, fix that
        //    transform.position = GetCurrentWaypoint();
        //    //navMeshAgent.nextPosition = transform.position;
        //    return Vector3.zero;
        //}

        return ( GetCurrentWaypoint()- transform.position); //don't need to normalise since that is done later anyway

    }

    public void SetWaypoint(Vector3 waypoint) {
        overriden = true;
        overrideWaypoint = waypoint;
    }

    public bool ReachedWaypoint(Vector3 waypoint) {
        return (transform.position - GetCurrentWaypoint()).sqrMagnitude <= closeEnoughDist * closeEnoughDist;
    }


    //called by cutscene
    public bool ReachedWaypoint() {
        return ReachedWaypoint(GetCurrentWaypoint());
    }

    public void FreeWaypoint() { 
        overriden = false; 
    }


    public Vector3 GetCurrentWaypoint() {
        return overriden ? overrideWaypoint : waypoints.Peek();
    }
}
