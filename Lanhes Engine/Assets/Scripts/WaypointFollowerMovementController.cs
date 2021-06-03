using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class WaypointFollowerMovementController : PawnMovementController
{

    public Queue<Vector3> waypoints = new Queue<Vector3>();//TODO show in editor

    private Vector3 overrideWaypoint;
    private bool overriden = false;

    private NavMeshAgent navMeshAgent = null;

    public void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        //TODO maybe just add the agent at runtime rather than editor time
        navMeshAgent.updatePosition = false;
        navMeshAgent.updateRotation = false;
        navMeshAgent.speed = this.moveSpeed ; 
        navMeshAgent.angularSpeed = 360000f;//Mathf.Infinity;
        navMeshAgent.autoRepath = true;
        navMeshAgent.autoBraking = true;
        navMeshAgent.acceleration = this.moveSpeed * 3;//this is the sensitivity of the players controls
        //base.Awake();
    }


    internal override Vector3 GetInput()
    {
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
            navMeshAgent.SetDestination(next);


        }


        return GetInputToPosition();

    }




    private Vector3 GetInputToPosition()
    {
        Vector3 diff = (navMeshAgent.nextPosition - transform.position);
        /*
        if (diff.sqrMagnitude <= moveSpeed * moveSpeed * Time.deltaTime)
        {
            if (navMeshAgent.remainingDistance <= Mathf.Epsilon)
            {
                //we have come to the end of the path
                //TODO this is not the cleanest of solutions, since we will not be animating for one frame, but it gets us to the desitination. Not a priority to fix, merely wrong.
                transform.position = navMeshAgent.nextPosition;
                return Vector3.zero;
            }
            else
            {  //TODO: In this case, we get some horrid jittering
                return diff.normalized;
            }
           
        }
        else
        {*/
            return diff.normalized;
        /*}*/

        //return navMeshAgent.desiredVelocity.normalized; turns out this doesnt work, the pawn overshoots the agent and wont correct
    }

    public void SetWaypoint(Vector3 waypoint)
    {
        overriden = true;
        overrideWaypoint = waypoint;
        navMeshAgent.SetDestination(waypoint);
    }

    public bool ReachedWaypoint(Vector3 waypoint)
    {
        Debug.Log((transform.position - waypoint).sqrMagnitude);
        return (transform.position - waypoint).sqrMagnitude <= Mathf.Epsilon;
    }


    //called by cutscene
    public bool ReachedWaypoint()
    {
        return ReachedWaypoint(overrideWaypoint);
    }
    public void FreeWaypoint()
    {
        overriden = false;
        navMeshAgent.SetDestination(waypoints.Peek());
    }

}
