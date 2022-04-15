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

        ////PHYSICS FINE-TUNING//// - delete this once events are implemented

        //if player detaches from wall, correct gravity scale
        if (playerRB.gravityScale == 0 && !collisions.onLeftWall && !collisions.onRightWall){
            playerRB.gravityScale = gravity;
        }

        //if player attaches to wall, correct gravity scale
        else if (playerRB.gravityScale == gravity && (collisions.onLeftWall || collisions.onRightWall)){
            playerRB.gravityScale = 0;
        }

    }

   
   //fetch inputs from attached gamepad
    public void OnMove(InputAction.CallbackContext value)
    {
        inputDirection = value.ReadValue<Vector2>();
    }

}
