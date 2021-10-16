using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovementPlayerController : PlayerPawnMovementController
{

    private Vector3 target;
    private UseTrigger useTarget;

    [SerializeField]
    private float snapDistance;
    internal override Vector3 GetPlayerInput() {
        UpdateWaypoint();


        //walk towards target
        Vector3 offset = target - transform.position;
        float distance = offset.magnitude;


        if (useTarget != null) {
            if (distance <= useDistance) {
                useTarget.Used();
                useTarget = null;
                return Vector3.zero;
            } else {
                return offset;
            }
        } else {
            if (distance <= snapDistance) {
                transform.position = target;
                return Vector3.zero;
            } else {
                return offset;
            }
        }

    }

    private void UpdateWaypoint() {
        if (Input.GetMouseButtonDown(0)) {
            Camera currentCamera = transform.GetComponentInChildren<Camera>();
            Ray ray = currentCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                //Debug.Log(hit.point);
                //TODO show player where they clicked
                UseTrigger useTrigger = hit.transform.GetComponent<UseTrigger>();
                if (useTrigger != null) {
                    useTarget = useTrigger;
                    target = hit.transform.position;
                } else {
                    useTarget = null;
                    target = hit.point;
                }
            }
        }
    }
}
