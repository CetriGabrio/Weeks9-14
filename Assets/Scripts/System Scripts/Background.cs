using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//I made this script solely to make a scrolling background, adding depth to the game
public class ScrollingBackground : MonoBehaviour
{
    //Variable for the speed of the scrolling 
    public float scrollSpeed = 0.1f;

    //I defined a start position so that every times it reset it always goes to the starting point
    private Vector2 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        //As mentioned previously, the background image is going to scroll upwards and reset to its initial position
        //I used Mathf.Repeat to simplify this process as it allows for the event to be repeated over and over again
        float newPosition = Mathf.Repeat(Time.time * scrollSpeed, 2);
        transform.position = startPosition + Vector2.down * newPosition;
    }
}
