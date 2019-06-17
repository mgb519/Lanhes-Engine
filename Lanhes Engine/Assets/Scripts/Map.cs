using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

    public Dictionary<Vector2Int, Tile> coordinates;

    [Serializable]
    public struct tileCoordPair {
        public Vector2Int key;
        public Tile coord;
    }
    public tileCoordPair[] coords;


    public void Start() {
        coordinates = new Dictionary<Vector2Int, Tile>();
        foreach (tileCoordPair t in coords) {
            coordinates.Add(t.key, t.coord);
        }
    }

    
    public bool InDirection(Vector2Int from, Vector2Int direction) {
        //look up from in the coordinates, check the direction to see if we can move out this way, and into what tile it would eneter
        Tile tileFrom = coordinates[from];
        if (Array.Find(tileFrom.canExitWithMovement, i => i.x == direction.x && i.y == direction.y) != null) {
            //we can safely exit this tile
            if (coordinates.ContainsKey(from + direction)) {
                Tile tileTo = coordinates[from + direction];
                if (Array.Find(tileTo.canExitWithMovement, i => i.x == direction.x && i.y == direction.y) != null) {
                    return true;
                }
            }

        }
        //TODO: check for entity on destination tile that blocks movement


        return false;
    }
}