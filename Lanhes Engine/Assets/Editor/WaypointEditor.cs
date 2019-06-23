using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Waypoint))]
public class WaypointEditor : Editor {
    void OnSceneGUI() {
        Waypoint waypoint = target as Waypoint;
        Map map = GameObject.Find("Map").GetComponent<Map>();
        if (Event.current.type == EventType.Repaint || Event.current.type == EventType.Layout) {
            for (int i = 0; i < waypoint.canExitWithMovement.Count; i++) {
                if (map.InDirection(waypoint, waypoint.canExitWithMovement[i])) {

                    Waypoint pointTo = map.coordinates[waypoint.position + waypoint.canExitWithMovement[i]];

                    float angle = Vector2.Angle(Vector2.down, waypoint.transform.position - pointTo.transform.position);
                    Vector2 offset = waypoint.transform.position - pointTo.transform.position;

                    Handles.ArrowHandleCap(i, waypoint.transform.position, Quaternion.FromToRotation(Vector3.back, offset), offset.magnitude * 0.9f, Event.current.type);

                }
            }
        } else if (Event.current.type == EventType.MouseDown) {
            if (Event.current.button == 1) {
                //TODO: does not check if we are strictly *on* the control; i.e we want to also right lick to *create* new links
                int clicked = HandleUtility.nearestControl;
                if (clicked >= 0 && clicked < waypoint.canExitWithMovement.Count) {
                    waypoint.canExitWithMovement.RemoveAt(clicked);
                }
            } else if (Event.current.button == 0) {
                //select other waypoint   
                Vector2 mousePos = Event.current.mousePosition;
                mousePos.y = Screen.height - mousePos.y;

                Transform newSelection = map.transform.GetChild(0);
                Vector2 newSelectionPos = Camera.current.WorldToScreenPoint(newSelection.position);
                if (Camera.current != null) {
                    foreach (Waypoint t in map.GetComponentsInChildren<Waypoint>()) {
                        Vector2 screenPos = Camera.current.WorldToScreenPoint(t.transform.position);
                        //TODO: unusual behavioir; clicking a little up of a node selects the one above it, even if we are further away.
                        if ((screenPos - mousePos).sqrMagnitude < (newSelectionPos - mousePos).sqrMagnitude) {
                            newSelection = t.transform;
                            newSelectionPos = Camera.current.WorldToScreenPoint(newSelection.position);
                        }
                    }
                }

                if (Event.current.control) {
                    waypoint.canExitWithMovement.Add(newSelection.GetComponent<Waypoint>().position - waypoint.position);
                } else {

                    Selection.activeGameObject = newSelection.gameObject;
                }
            }

        }

    }
}