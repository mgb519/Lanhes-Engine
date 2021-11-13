using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovementPlayerController : PlayerPawnMovementController
{
    //TODO reimplment the stuff getting shortest distance between objects


    private Vector3 target;
    private UseTrigger useTarget;

    bool reached = true;

    [SerializeField]
    private float snapDistance;
    internal override Vector3 GetPlayerInput() {
        UpdateWaypoint();

        if (reached) { return Vector3.zero; }


        Vector3 offset = target - transform.position;


        if (useTarget != null) {
            Vector3 closestPoint = useTarget.GetComponent<Collider>().ClosestPoint(transform.position);
            float distance = (closestPoint - transform.position).sqrMagnitude;
            if (distance <= useDistance*useDistance) {
                Debug.Log("Using " + useTarget.name);
                useTarget.Used();
                useTarget = null;
                reached = true;
                return Vector3.zero;
            } else {
                return offset;
            }
        } else {
            //walk towards target
            float distance = offset.magnitude;
            if (distance <= snapDistance) {
                transform.position = target;
                reached = true;
                return Vector3.zero;
            } else {
                return offset;
            }
        }

    }

    private void UpdateWaypoint() {
        Camera currentCamera = transform.GetComponentInChildren<Camera>();
        Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            //Debug.Log(hit.point);
            UseTrigger useTrigger = hit.transform.GetComponent<UseTrigger>();

            if (useTrigger != null) {
                if (Input.GetMouseButtonDown(0)) {
                    //TODO show player where they clicked
                    useTarget = useTrigger;
                    target = hit.transform.position;
                    reached = false;
                }

                useTrigger.RequestLabel();
            } else {
                if (Input.GetMouseButtonDown(0)) {
                    print("click");
                    //TODO show player where they clicked
                    useTarget = null;
                    target = hit.point;
                    Debug.Log(target);
                    reached = false;
                }
            }

            
        }
    }
}
