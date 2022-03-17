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
    private float jumpThreshold;

    private void Start()
    {
        playerRB = gameObject.GetComponent<Rigidbody2D>();
        collisions = gameObject.GetComponent<Collisions>();
    }

    private void FixedUpdate()
    {
       
        //if player is in midair, free-fall
        if (!collisions.onGround && !collisions.onLeftWall && !collisions.onRightWall){
            playerRB.velocity = new Vector2(inputDirection.x*playerSpeed, playerRB.velocity.y);
        }

        //if player is touching ground
        else if (collisions.onGround){
            playerRB.velocity = new Vector2(inputDirection.x*playerSpeed, 0);
            if (inputDirection.y > 0.2f){
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

   
   //fetch inputs from attached gamepad
    public void OnMove(InputAction.CallbackContext value)
    {
        inputDirection = value.ReadValue<Vector2>();
    }
    
    public void OnJump(InputAction.CallbackContext context)
    {
        
        
    }

}
