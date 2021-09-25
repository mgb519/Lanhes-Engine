﻿using System.Collections;
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

   

    internal override Vector3 GetInput()
    {
        //TODO
        if (!overriden)
        {
            if (waypoints.Count == 0) { return Vector3.zero; }
            //patrol along path
            Vector3 next = waypoints.Peek();
            while (ReachedWaypoint(next))
            {
                next = waypoints.Dequeue();
                waypoints.Enqueue(next);
                next = waypoints.Peek();
            }
            //navMeshAgent.SetDestination(next);


        }


        return GetInputToPosition();

    }




    private Vector3 GetInputToPosition()
    {

        /*
        navMeshAgent.nextPosition = transform.position;
        Vector3 d = navMeshAgent.desiredVelocity;
        if ((transform.position-navMeshAgent.destination).sqrMagnitude <= moveSpeed * moveSpeed * Time.deltaTime) {
            //if we can reach in one frame, snap to it, this should remove jitter
            //TODO this is not the cleanest of solutions, since we will not be animating for one frame, but it gets us to the desitination. Not a priority to fix, merely wrong.
            //TODO the snap is visible, fix that
            transform.position = navMeshAgent.destination;
            navMeshAgent.nextPosition = transform.position;
            return Vector3.zero;
        }


        return d.normalized;*/
        //TODO
        return Vector3.zero;

    }

    public void SetWaypoint(Vector3 waypoint)
    {
        //TODO
        overriden = true;
        overrideWaypoint = waypoint;
        //navMeshAgent.SetDestination(waypoint);
    }

    public bool ReachedWaypoint(Vector3 waypoint)
    {     
        //Debug.Log((transform.position - waypoint).sqrMagnitude);
        return (transform.position - waypoint).sqrMagnitude <= Mathf.Epsilon;
    }


    //called by cutscene
    public bool ReachedWaypoint()
    {

        //TODO
        //return ReachedWaypoint(navMeshAgent.destination);
        return true;
    }

    public void FreeWaypoint()
    {/*
        overriden = false;
        if (waypoints.Count >= 1)
        {
            navMeshAgent.SetDestination(waypoints.Peek());
        }
        else {
            navMeshAgent.ResetPath();
        }
        *?
        *///TODO
        overriden = false;


    }

}
