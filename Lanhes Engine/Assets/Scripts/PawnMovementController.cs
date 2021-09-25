using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public abstract class PawnMovementController : MonoBehaviour {

    public float moveSpeed; //how many tiles do you move in 1s?

    private Animator anim;
    new private Rigidbody rigidbody;
    private Vector3 lastDirection;



    // Start is called before the first frame update
    void Start() {
        anim = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();

    }



    // Update is called once per frame
    public void Update() {
        if (WindowManager.instance.ContinuePlay() && !GameSceneManager.IsLoading() && !DataManager.IsLoading() && !BattleManager.InBattle())
        {
            //TODO: perhaps this should be got as a vector2? I *presume* we won't have Y axis movement, but the navmesh seems to assume we *can*.
            Vector3 inp = GetInput();
            inp.y = 0;
            Vector3 dir = inp.normalized;
            //TODO: snappng to grid option
            if (dir.magnitude > 0) {
                lastDirection = dir;
            }

            //TODO set an angle for slopes I can't go up

            rigidbody.velocity = dir * moveSpeed;
            //Debug.Log(rigidbody.velocity);
            //animate
            //TODO: make sure to end on the last frame?
            anim.SetInteger("Horizontal", Mathf.RoundToInt(dir.x));
            anim.SetInteger("Vertical", Mathf.RoundToInt(dir.z));
            anim.speed = anim.GetInteger("Horizontal") == 0 && anim.GetInteger("Vertical") == 0 ? 0 : 1;
                        
        }
        else {
            rigidbody.velocity = Vector3.zero;
            anim.speed = 0;
        }

    }

    //TODO: maybe instead of abstract classes, use a delegate?
    /// <summary>
    /// Gets the input being sent to the direct the pawn, as appropriate for the pawn
    /// </summary>
    /// <returns> Vector representing the direction the pawn moves in.</returns>
    internal abstract Vector3 GetInput();

}
