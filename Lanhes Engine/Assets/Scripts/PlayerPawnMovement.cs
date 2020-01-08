using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(PlayerController))]
[RequireComponent (typeof(Rigidbody))]
public class PlayerPawnMovement : MonoBehaviour
{

    public float moveSpeed; //how many tiles do you move in 1s?

    
    // Update is called once per frame
    void Update()
    {
        //TODO: snappng to grip option
        int horizontalMove = Mathf.RoundToInt(Input.GetAxis("Horizontal"));
        int verticalMove = Mathf.RoundToInt(Input.GetAxis("Vertical"));

        //TODO barriers I cant go up

        //TODO animation
        GetComponent<Rigidbody>().velocity = new Vector3(horizontalMove,0,verticalMove)*moveSpeed;
    }
}
