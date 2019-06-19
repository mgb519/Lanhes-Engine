using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

    public Dictionary<Vector2Int, Waypoint> coordinates;




    public void Start() {
        //coordinates = new Dictionary<Vector2Int, Waypoint>();
        //TODO: hardcoded for square tiles
        //foreach (Waypoint child in gameObject.GetComponentsInChildren<Waypoint>()) {

        //}
    }


    public bool InDirection(Vector2Int from, Vector2Int direction) {
        //look up from in the coordinates, check the direction to see if we can move out this way, and into what tile it would eneter
        Waypoint tileFrom = coordinates[from];
        if (Array.Find(tileFrom.canExitWithMovement, i => i.x == direction.x && i.y == direction.y) != null) {
            //we can safely exit this tile
            if (coordinates.ContainsKey(from + direction)) {
                Waypoint tileTo = coordinates[from + direction];
                if (Array.Find(tileTo.canExitWithMovement, i => i.x == direction.x && i.y == direction.y) != null) {
                    return true;
                }
            }

        }
        //TODO: check for entity on destination tile that blocks movement


        return false;
    }



    public void CreateSquareWaypointMesh(float distanceBetweenTiles, int width, int hieght) {
        //delete existing grid
        while (gameObject.transform.childCount > 0) {
            GameObject.DestroyImmediate(gameObject.transform.GetChild(0).gameObject);
        }
        coordinates = new Dictionary<Vector2Int, Waypoint>();


        for (int i = 0; i < width; i++) {
            for (int j = 0; j < hieght; j++) {
                GameObject w = new GameObject(i + "," + j);
                w.transform.parent = gameObject.transform;
                Waypoint waypoint = w.AddComponent<Waypoint>();
                waypoint.canEnterWithMovement = new Vector2Int[4] { new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1) };
                waypoint.canExitWithMovement = new Vector2Int[4] { new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1) };
                w.transform.position = new Vector3(i * distanceBetweenTiles, j * distanceBetweenTiles);
                coordinates.Add(new Vector2Int(i, j), waypoint);
            }
        }




    }
}