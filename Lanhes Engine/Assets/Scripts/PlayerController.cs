using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Vector2Int position;
    private Vector2Int target;
    public Vector2Int facing;

    private Map map;

    public float moveSpeed; //how many tiles do you move in 1s?


    // Start is called before the first frame update
    void Start() {
        map = GameObject.Find("Map").GetComponent<Map>();
        target = position;
    }

    // Update is called once per frame
    void Update() {
        //TODO: we're assuming that we are moving on a square grid here, factor this assumption out into a different input module to allow hex movement. How do we do hex movement on a keyboard?
        int horizontalMove = Mathf.RoundToInt(Input.GetAxis("Horizontal"));
        int verticalMove = Mathf.RoundToInt(Input.GetAxis("Vertical"));

        if (target.x == position.x && target.y == position.y) {
            //find the tile coords we're going to and set that as target, if we're moving
            Vector2Int motion = new Vector2Int(horizontalMove, verticalMove);            
            if (map.InDirection(position, motion)) {
                target = position + motion;
            }
            facing = motion;
        }
        
        //drift towards target, snap and set position to it when we're close enough
        Waypoint targetTile = map.coordinates[target];
        float moveDistance = moveSpeed * Time.deltaTime;
        Vector3 distance = (targetTile.gameObject.transform.position - gameObject.transform.position);
        Vector3 movementThisFrame = distance.normalized * moveDistance;


        if (distance.sqrMagnitude <= movementThisFrame.sqrMagnitude) {
            position = target;
            gameObject.transform.Translate(distance);
        } else {
            gameObject.transform.Translate(movementThisFrame);
        }



    }
}
