using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JumpWithSpace : MonoBehaviour
{
    Rigidbody2D playerRB;

    //vars to playtest in editor
    [SerializeField]
    private float playerSpeed;

    [SerializeField]
    private float jumpForce;

    //control inputs to read
    Vector2 inputDirection;

    //movement states
    PlayerCollisions collisions;

    [SerializeField]
    private bool canJump;
    private float gravity;

    private void Start()
    {
        playerRB = gameObject.GetComponent<Rigidbody2D>();
        collisions = gameObject.GetComponent<PlayerCollisions>();

        gravity = playerRB.gravityScale;

        collisions.OnGroundCollisionChanged += TrackJumping;
        collisions.OnWallCollisionChanged += TrackJumping;
        collisions.OnWallCollisionChanged += GravityControls;
    }

    private void FixedUpdate()
    {
       Vector2 playerVelocity;

        //player movement in x direction
        playerVelocity.x = inputDirection.x*playerSpeed;
        if (canJump && (collisions.onLeftWall || collisions.onRightWall)){ //if player is on wall and has not jumped, cling to wall
            playerVelocity.x = 0;
        }
        else if (!canJump && (collisions.onLeftWall || collisions.onRightWall)){ //allow player to jump if button was pressed
            playerVelocity.x = playerRB.velocity.x;
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

    }
   
   //fetch inputs from attached gamepad
    public void OnMove(InputAction.CallbackContext value)
    {
        inputDirection = value.ReadValue<Vector2>();
    }

    private void TrackJumping(bool hitWall){
       canJump = hitWall;
    }

    private void GravityControls(bool hitWall){
        if (hitWall){
            playerRB.gravityScale = 0;
        }else{
            playerRB.gravityScale = gravity;
        }
    }
    
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && canJump){ //activate jump right at button push, not at button hold or lift
            canJump = false;

            if (collisions.onGround){ //force applied up when player jumps from ground
                playerRB.AddForce(jumpForce*Vector2.up, ForceMode2D.Impulse);
            }
            else if (collisions.onLeftWall) { //force applied to the right when player jumps from left wall
                playerRB.AddForce(jumpForce*Vector2.right, ForceMode2D.Impulse);
            }
            else if (collisions.onRightWall){ //force applied to the left when player jumps from right wall
                playerRB.AddForce(jumpForce*Vector2.left, ForceMode2D.Impulse);
            }

        }
    }
}
