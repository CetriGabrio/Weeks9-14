using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour
{

    public float speed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        //Reset player position at the start of the game to the centre of the screen
        transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //Variables for the player horizontal and vertical movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //Functions to allow the player to move on both horizontal and vertical axis at a realistic speed
        //Since it's a shoot em up game, I do not want the player to be able to rotate and point in any direction other than upwards
        transform.Translate(Vector3.right * horizontalInput * speed * Time.deltaTime);
        transform.Translate(Vector3.up * verticalInput * speed * Time.deltaTime);
    }
}
