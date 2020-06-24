using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public abstract class PawnMovementController : MonoBehaviour {

    public float moveSpeed; //how many tiles do you move in 1s?

    private Animator anim;
    new private Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start() {
        anim = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update() {

        Vector3 dir = GetInput();


        //TODO: snappng to grid option


        //TODO barriers I cant go up

        rigidbody.velocity = dir * moveSpeed;
        //animate
        //TODO: end on the last frame?
        anim.SetInteger("Horizontal", Mathf.RoundToInt(dir.x));
        anim.SetInteger("Vertical", Mathf.RoundToInt(dir.z));
        anim.speed = anim.GetInteger("Horizontal") == 0 && anim.GetInteger("Vertical") == 0 ? 0 : 1;


    }

    //TODO: maybe instead of abstract classes, use a delegate?
    internal abstract Vector3 GetInput();

}
