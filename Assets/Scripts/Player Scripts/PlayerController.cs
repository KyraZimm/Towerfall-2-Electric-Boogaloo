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
    private float gravity;

    //control inputs to read
    Vector2 inputDirection;

    //movement states
    PlayerCollisions collisions;

    [SerializeField]
    private float jumpThreshold;

    private void Start()
    {
        playerRB = gameObject.GetComponent<Rigidbody2D>();
        collisions = gameObject.GetComponent<PlayerCollisions>();

        gravity = playerRB.gravityScale;

        collisions.OnWallCollisionChanged += GravityControls;
    }

    private void FixedUpdate()
    {
        ////GAMEPAD INPUTS & MOVEMENT////

       //if player is in midair, free-fall
        if (!collisions.onGround && !collisions.onLeftWall && !collisions.onRightWall){
            playerRB.velocity = new Vector2(inputDirection.x*playerSpeed, playerRB.velocity.y);
        }

        //if player is touching ground
        else if (collisions.onGround){
            playerRB.velocity = new Vector2(inputDirection.x*playerSpeed, 0);
            if (inputDirection.y > 0.4f){
                playerRB.AddForce(jumpForce*Vector3.up, ForceMode2D.Impulse);
            }
        }

        //if player is on wall
        else if (collisions.onLeftWall){
            playerRB.velocity = new Vector2(0, inputDirection.y*playerSpeed);
            if (inputDirection.x > jumpThreshold){
                playerRB.AddForce(jumpForce*Vector3.right, ForceMode2D.Impulse);
            }
        }
        else if (collisions.onRightWall){
            playerRB.velocity = new Vector2(0, inputDirection.y*playerSpeed);
            if (inputDirection.x < -jumpThreshold){
                playerRB.AddForce(jumpForce*Vector3.left, ForceMode2D.Impulse);
            }
        }
    }

    //turn gravity on/off during/after wall cling
    private void GravityControls(bool hitWall){
        if (hitWall){
            playerRB.gravityScale = 0;
        }else{
            playerRB.gravityScale = gravity;
        }
    }

   
   //fetch inputs from attached gamepad
    public void OnMove(InputAction.CallbackContext value)
    {
        inputDirection = value.ReadValue<Vector2>();
    }

}
