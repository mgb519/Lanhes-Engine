using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Waypoint : MonoBehaviour {


    /// <summary>
    /// for clear square grids, this is (+1,+1),(+1,0),(+1,-1),(0,+1),(0,0),(0,-1),(-1,+1),(-1,0),(-1,-1)
    /// for clear square grids with no diagonals, this is (+1,0),(0,+1),(0,0),(0,-1),(-1,0)
    /// for clear hex grids, this is (0,-1),(+1,-1),(+1,0),(0,+1),(-1,+1),(-1,0)
    /// </summary>
    /// 
    public Vector2Int position;
    public List<Vector2Int> canExitWithMovement = new List<Vector2Int>();

}
