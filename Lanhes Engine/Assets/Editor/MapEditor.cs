using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(Map))]
public class MapEditor : Editor {

    [SerializeField]
    [Range(0, 5f)] private float distanceBetweenTiles;
    [SerializeField]
    [Range(0, 9999999)] private int width;
    [SerializeField]
    [Range(0, 9999999)] private int height;

    [SerializeField]
    private SerializableDictionary<Vector2Int, Waypoint> coords;

    public override void OnInspectorGUI() {
        Map myTarget = (Map)target;

        //myTarget.experience = EditorGUILayout.IntField("Experience", myTarget.experience);
        //EditorGUILayout.LabelField("Level", myTarget.Level.ToString());

        distanceBetweenTiles = EditorGUILayout.FloatField("Tile Distance", distanceBetweenTiles);
        width = EditorGUILayout.IntField("Grid Width", width);
        height = EditorGUILayout.IntField("Grid Height", height);

        if (GUILayout.Button("Build Square Grid")) {
            CreateSquareWaypointMesh(distanceBetweenTiles, width, height);
        }
    }

    [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
    static void DrawGizmosSelected(Map map, GizmoType gizmoType) {


        foreach (Waypoint t in map.transform.GetComponentsInChildren<Waypoint>()) {
            Handles.DrawWireDisc(t.transform.position, Vector3.forward, HandleUtility.GetHandleSize(t.transform.position) * 0.1f);
            Vector2Int pos = t.position;
            foreach (Vector2Int dir in t.canExitWithMovement) {
                if (map.InDirection(t, dir)) {

                    Gizmos.DrawLine(t.transform.position, map.coordinates[pos + dir].transform.position);

                }
            }
        }
    }



    void OnSceneGUI() {
        Map map = target as Map;
        if (Event.current.type == EventType.Repaint || Event.current.type == EventType.Layout) {
            foreach (Waypoint t in map.GetComponentsInChildren<Waypoint>()) {
                Handles.DrawWireDisc(t.transform.position, Vector3.forward, HandleUtility.GetHandleSize(t.transform.position)*0.1f);
            }
        } else if (Event.current.type == EventType.MouseDown) {
            

        }
    }

    public void CreateSquareWaypointMesh(float distanceBetweenTiles, int width, int hieght) {
        //delete existing grid
        Map map = (Map)target;

        while (map.gameObject.transform.childCount > 0) {
            GameObject.DestroyImmediate(map.gameObject.transform.GetChild(0).gameObject);
        }
        map.coordinates = new Map.CoordDict();


        for (int i = 0; i < width; i++) {
            for (int j = 0; j < hieght; j++) {
                GameObject w = new GameObject("Waypoint at " + i + "," + j);
                w.transform.parent = map.gameObject.transform;
                Waypoint waypoint = w.AddComponent<Waypoint>();
                waypoint.position = new Vector2Int(i, j);
                //waypoint.canEnterWithMovement = new Vector2Int[4] { new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1) };
                waypoint.canExitWithMovement = new List<Vector2Int> { new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1) };
                w.transform.position = new Vector3(i * distanceBetweenTiles, j * distanceBetweenTiles);
                map.coordinates.Add(new Vector2Int(i, j), waypoint);
            }
        }
    }


}
