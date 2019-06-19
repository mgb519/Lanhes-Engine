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
    public Vector2Int[] canExitWithMovement;
    public Vector2Int[] canEnterWithMovement;
    

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}
