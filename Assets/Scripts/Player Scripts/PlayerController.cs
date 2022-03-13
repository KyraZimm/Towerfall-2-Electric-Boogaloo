using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
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
    [SerializeField]
    Collisions collisions;
    private bool jumping;
    private bool dashing;


    private void Start()
    {
        playerRB = gameObject.GetComponent<Rigidbody2D>();
        collisions = gameObject.GetComponent<Collisions>();
    }

    private void FixedUpdate()
    {
        
        playerRB.velocity = inputDirection * playerSpeed;
        
    }
   
   //fetch inputs from attached gamepad
    public void OnMove(InputAction.CallbackContext value)
    {
        inputDirection = value.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log("jump!");

        if(collisions.onGround && context.performed)
        {
            playerRB.AddForce(jumpForce*Vector3.up, ForceMode2D.Impulse);
        }
    }

}
