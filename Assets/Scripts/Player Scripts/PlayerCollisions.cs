using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCollisions : MonoBehaviour
{
     //bools for movement scripts to read
     public bool onGround;
     public bool onLeftWall;
     public bool onRightWall;

     //layer masks for types of collisions
     LayerMask groundLayer;
     LayerMask wallLayer;
     LayerMask enemyLayer;
     LayerMask playerLayer;

     //unchanging values for raycasting
     Vector2 rayMagnitude;

     //Unity Events for player health, respawn, and animation scripts
     UnityEvent hitWall;

     private void Start()
     {
         //assign layers for runtime raycasting
         groundLayer = LayerMask.GetMask("Ground");
         wallLayer = LayerMask.GetMask("Walls");
         enemyLayer = LayerMask.GetMask("Enemy");
         playerLayer = LayerMask.GetMask("Player");


        //set raycast magnitude so rays are slightly longer than player height/width
        rayMagnitude = gameObject.transform.localScale/2;
        rayMagnitude.x += gameObject.transform.localScale.x/5;
        rayMagnitude.y += gameObject.transform.localScale.y/5;
     } 
    
   
    private void FixedUpdate()
    {
        Vector2 playerPos = gameObject.transform.position;

        //raycast for walls/ground
        onLeftWall = Physics2D.Raycast(playerPos, Vector2.left, rayMagnitude.x, wallLayer);
        onRightWall = Physics2D.Raycast(playerPos, Vector2.right, rayMagnitude.x, wallLayer);
        onGround = Physics2D.Raycast(playerPos, Vector2.down, rayMagnitude.y, groundLayer);


        //Debugging rays - for Unity Editor use only
        Debug.DrawRay(playerPos, Vector2.left*rayMagnitude.x, Color.red);
        Debug.DrawRay(playerPos, Vector2.right*rayMagnitude.x, Color.red);
        Debug.DrawRay(playerPos, Vector2.down*rayMagnitude.y, Color.green);

        //raycast for enemy or player hits
        
        
    }

}
