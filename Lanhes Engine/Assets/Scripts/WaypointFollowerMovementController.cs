using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollowerMovementController : PawnMovementController {

    public Queue<Vector3> waypoints;

    private Vector3 overrideWaypoint;
    private bool overriden = false;

    internal override Vector3 GetInput() {
        if (overrideWaypoint!=null) {
            //TODO: patrol along path
            throw new System.NotImplementedException();
        } else {
            //go to the cutscene position
            throw new System.NotImplementedException();
        }
    }


    public void SetWaypoint(Vector3 waypoint) {
        overriden = true;
        overrideWaypoint = waypoint;
    }

    public bool ReachedWaypoint(Vector3 waypoint) {
        return (transform.position - waypoint).sqrMagnitude < Mathf.Epsilon;
    }


    //called by cutscene
    public bool ReachedWaypoint() {
        return ReachedWaypoint(overrideWaypoint);
    }
    public void FreeWaypoint() {
        overriden = false;
    }

}
