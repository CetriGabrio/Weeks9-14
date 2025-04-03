using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Spaceship : MonoBehaviour
{

    public float speed = 1f;

    public GameObject laserPrefab;

    public float firerate = 1f;
    private float canFire = -1f;
    // Start is called before the first frame update
    void Start()
    {
        //Reset player position at the start of the game to the centre of the screen
        transform.position = new Vector2(0, -2);
    }

    // Update is called once per frame
    void Update()
    {
        SpaceshipMovement();

        ShootLaser();
    }

    //Created a new function for all the spaceship movement features
    void SpaceshipMovement()
    {
        //Variables for the player horizontal and vertical movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //Functions to allow the player to move on both horizontal and vertical axis at a realistic speed
        //Since it's a shoot em up game, I do not want the player to be able to rotate and point in any direction other than upwards
        transform.Translate(Vector3.right * horizontalInput * speed * Time.deltaTime);
        transform.Translate(Vector3.up * verticalInput * speed * Time.deltaTime);

        //Creating player bouds so that the spaceship is enclosed in the screen

        //This part is handling the vertical movement, so the player can only stay in the bottom side of the screen
        if (transform.position.y >= -1f)
        {
            transform.position = new Vector3(transform.position.x, -1f, 0);
        }
        else if (transform.position.y <= -4f)
        {
            transform.position = new Vector3(transform.position.x, -4f, 0);
        }

        //This part is handling the horizontal movement, implementing a pac man effect that allows the player to teleport on the other border of the screen
        if (transform.position.x > 9.5f)
        {
            transform.position = new Vector3(-9.5f, transform.position.y, 0);
        }
        else if (transform.position.x < -9.5f)
        {
            transform.position = new Vector3(9.5f, transform.position.y, 0);
        }

        /////////////////
        ///Use camera to wolrd point to handle different aspect ratios
        ////////////////
    }

    //Created a function t handle all the shooting mechanics to organize the code
    void ShootLaser()
    {
        //Shooting happens only when space bar is pressed
        //I also added a cooldown so that the player can not spam shoot
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > canFire)
        {
            canFire = Time.time + firerate;
            Instantiate(laserPrefab, transform.position + new Vector3(0, 1f, 0), Quaternion.identity); //The vector3 is for the offset, allowing the laser to be spawned in the correct position
        }
    }
}
