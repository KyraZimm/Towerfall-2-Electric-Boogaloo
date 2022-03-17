using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
   //This script largely controls player movement and accesses gamepad/keyboard inputs.

    Rigidbody2D playerRB;

    //vars to playtest in editor
    [SerializeField]
    private float playerSpeed;

    [SerializeField]
    private float jumpForce;

    //control inputs to read
    Vector2 inputDirection;

    //movement states
    Collisions collisions;

     [SerializeField]
    private bool jumped;

    private void Start()
    {
        playerRB = gameObject.GetComponent<Rigidbody2D>();
        collisions = gameObject.GetComponent<Collisions>();
    }

    private void FixedUpdate()
    {
       Vector2 playerVelocity;

        //player movement in x direction
        playerVelocity.x = inputDirection.x*playerSpeed;
        if (!jumped && (collisions.onLeftWall || collisions.onRightWall)){ //if player is on wall and has not jumped, cling to wall
            playerVelocity.x = 0;
        }

        //player movement in y direction
        if (collisions.onLeftWall || collisions.onRightWall){ //if player is on wall, climb up/down wall
            playerVelocity.y = inputDirection.y*playerSpeed;
        }
        else{ //if player is not on wall, gravity applies
            playerVelocity.y = playerRB.velocity.y;
        }

        //pass custom velocity to player's rigidbody
        playerRB.velocity = playerVelocity;

        //check if player is done jumping
        if (jumped && (collisions.onGround || collisions.onLeftWall || collisions.onRightWall)){
            jumped = false;
        }

    }
   
   //fetch inputs from attached gamepad
    public void OnMove(InputAction.CallbackContext value)
    {
        inputDirection = value.ReadValue<Vector2>();
    }
    
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed){ //activate jump right at button push, not at button hold or lift

            if (collisions.onGround){ //force applied up when player jumps from ground
                playerRB.AddForce(jumpForce*Vector3.up, ForceMode2D.Impulse);
            }
            else if (collisions.onLeftWall) { //force applied to the right when player jumps from left wall
                playerRB.AddForce(jumpForce*Vector3.right, ForceMode2D.Impulse);
            }
            else if (collisions.onRightWall){ //force applied to the left when player jumps from right wall
                playerRB.AddForce(jumpForce*Vector3.left, ForceMode2D.Impulse);
            }

            jumped = true;

        }
    }

}
