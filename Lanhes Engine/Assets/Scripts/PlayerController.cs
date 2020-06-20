using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Waypoint position;
    private Waypoint target;
    public Vector2Int facing;

    private Map map;

  

    public float moveSpeed; //how many tiles do you move in 1s?

    public Inventory inventory;

    //TODO: for game testing remove this later
    public InertItem currency;

    // Start is called before the first frame update
    void Start() {
        map = GameObject.Find("Map").GetComponent<Map>();

        position = map.coordinates[new Vector2Int(0, 0)];
        target = position;

        for (int i = 0; i < 1000; i++) {
            inventory.AddItem(currency);
        }
    }

    // Update is called once per frame
    void Update() {
        if (WindowManager.instance.ContinuePlay()) {


            //TODO: we're assuming that we are moving on a square grid here, factor this assumption out into a different input module to allow hex movement. How do we do hex movement on a keyboard? WEADZX?
            int horizontalMove = Mathf.RoundToInt(Input.GetAxis("Horizontal"));
            int verticalMove = Mathf.RoundToInt(Input.GetAxis("Vertical"));

            if (target.position.x == position.position.x && target.position.y == position.position.y) {
                //find the tile coords we're going to and set that as target, if we're moving
                Vector2Int motion = new Vector2Int(horizontalMove, verticalMove);
                if (map.InDirection(position, motion)) {
                    target = map.coordinates[position.position + motion];
                }
                facing = motion;
            }

            //drift towards target, snap and set position to it when we're close enough
            Waypoint targetTile = map.coordinates[target.position];
            float moveDistance = moveSpeed * Time.deltaTime;
            Vector3 distance = (targetTile.gameObject.transform.position - gameObject.transform.position);
            Vector3 movementThisFrame = distance.normalized * moveDistance;


            if (distance.sqrMagnitude <= movementThisFrame.sqrMagnitude) {
                position = target;
                gameObject.transform.Translate(distance);
            } else {
                gameObject.transform.Translate(movementThisFrame);
            }

            if (Input.GetButtonDown("Pause") && Time.timeScale != 0 && WindowManager.instance.ContinuePlay()) {
                WindowManager.CreatePauseWindow();

            }
        }

    }
}
