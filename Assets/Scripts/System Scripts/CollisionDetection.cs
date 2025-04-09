using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This was by far one of the most important part of the whole project, making collisions withouth using colliders2D.
//More details in the implementation Log, but on a brief review, I decided to use rectangles into rectangles collisions
//This made the athematical process much simpler to both understand and develop

//For this part, shoutout to Jeffrey Thompson for the notes
//https://jeffreythompson.org/collision-detection/rect-rect.php


public class CollisionDetection : MonoBehaviour
{
    //Static method for rectangle-to-rectangle collision detection
    //These method includes detecting the X and Y coordinates, as well as the width and height of both rectangles
    public bool CheckCollision(float r1x, float r1y, float r1w, float r1h,
                                      float r2x, float r2y, float r2w, float r2h)
    {
        //Debug.Log("Checking");

        //Here I am checking if the rectangles overlap with eachother
        return r1x < r2x + r2w &&    //r1 left edge is before r2 right edge
               r1x + r1w > r2x &&    //r1 right edge is after r2 left edge
               r1y < r2y + r2h &&    //r1 top edge is above r2 bottom edge
               r1y + r1h > r2y;      //r1 bottom edge is below r2 top edge
    }
}
