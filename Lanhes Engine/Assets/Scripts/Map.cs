using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Map : MonoBehaviour {

    [SerializeField]
    public coordDict coordinates;


    //this makes the above serializable, becuase a <T> class cannot be serialized by unity, but this can
    //for some reason
    //idunno but that's what the internet says
    [Serializable]
    public class coordDict : SerializableDictionary<Vector2Int, Waypoint> { };



    public bool InDirection(Waypoint from, Vector2Int direction) {
        //look up from in the coordinates, check the direction to see if we can move out this way, and into what tile it would eneter
        if (from.canExitWithMovement.Contains(direction)) {
            //we can safely exit this tile
            if (coordinates.ContainsKey(from.position + direction)) {
                Waypoint tileTo = coordinates[from.position + direction];
                //if (Array.Find(tileTo.canEnterWithMovement, i => i.x == direction.x && i.y == direction.y) != null) {
                    return true;
                //}
            }

        }
        //TODO: check for entity on destination tile that blocks movement


        return false;
    }


}