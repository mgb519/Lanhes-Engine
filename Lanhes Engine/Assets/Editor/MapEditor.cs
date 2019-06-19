using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(Map))]
public class MapEditor : Editor {

    [Range(0, 5f)] private float distanceBetweenTiles;
    [Range(0, 9999999)] private int width;
    [Range(0, 9999999)] private int height;

    public override void OnInspectorGUI() {
        Map myTarget = (Map)target;

        //myTarget.experience = EditorGUILayout.IntField("Experience", myTarget.experience);
        //EditorGUILayout.LabelField("Level", myTarget.Level.ToString());

        distanceBetweenTiles = EditorGUILayout.FloatField("Tile Distance", distanceBetweenTiles);
        width = EditorGUILayout.IntField("Grid Width", width);
        height = EditorGUILayout.IntField("Grid Height", height);

        if (GUILayout.Button("Build Square Grid")) {
            myTarget.CreateSquareWaypointMesh(distanceBetweenTiles, width, height);
        }
    }

    [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
    static void DrawGizmosSelected(Map map, GizmoType gizmoType) {

        //Map map = ((Map)target);




        foreach (Waypoint t in map.transform.GetComponentsInChildren<Waypoint>()) {


            Vector2Int pos = new Vector2Int();
            pos.x = int.Parse((t.name.Split(','))[0]);
            pos.y = int.Parse((t.name.Split(','))[1]);
            foreach (Vector2Int dir in t.canExitWithMovement) {                
                if (map.InDirection(pos, dir)) {

                    Gizmos.DrawLine(map.coordinates[pos].transform.position, map.coordinates[pos + dir].transform.position);
                    //Handles.ArrowHandleCap(1, t.transform.position, t.transform.rotation, 1, EventType.Repaint);
                }
            }
        }
    }


}
