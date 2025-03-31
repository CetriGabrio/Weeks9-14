using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    //Static method for rectangle-to-rectangle collision detection
    public bool CheckCollision(float r1x, float r1y, float r1w, float r1h,
                                      float r2x, float r2y, float r2w, float r2h)
    {
        //Debug.Log("Checking");
        //Check if the rectangles overlap
        return r1x < r2x + r2w &&    //r1 left edge is before r2 right edge
               r1x + r1w > r2x &&    //r1 right edge is after r2 left edge
               r1y < r2y + r2h &&    //r1 top edge is above r2 bottom edge
               r1y + r1h > r2y;      //r1 bottom edge is below r2 top edge
    }
}
