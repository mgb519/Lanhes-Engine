using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerPawnMovement : MonoBehaviour {
    public float moveSpeed; //how many tiles do you move in 1s?

    private Animator anim;
    new private Rigidbody rigidbody;
    
    private void Start() {
        anim = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
    }
    

    // Update is called once per frame
    void Update() {
        //TODO: we have an issue where there is one frame where we can continue play between a dialogue being opened and the next, in which we can move...
        if (WindowManager.instance.ContinuePlay()) {
            //TODO: snappng to grid option
            int horizontalMove = Mathf.RoundToInt(Input.GetAxis("Horizontal"));
            int verticalMove = Mathf.RoundToInt(Input.GetAxis("Vertical"));

            //TODO barriers I cant go up

            rigidbody.velocity = new Vector3(horizontalMove, 0, verticalMove) * moveSpeed;

            //animate
            //TODO: end up on the last frame?
            anim.SetInteger("Horizontal", horizontalMove);
            anim.SetInteger("Vertical", verticalMove);
            anim.speed = 1;
        } else {
            //freeze
            //TODO: the movement issue could be simplified if we made movement a product of root motion
            rigidbody.velocity = Vector3.zero;
            anim.speed = 0;

        }
    }
}
