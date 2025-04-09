using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


//This code contains everything related to the lasers being shot by the player's spaceship.
public class Laser : MonoBehaviour
{
    //Variable to store the main game components
    public GameObject enemy;
    private Spaceship spaceship;

    //Dimensions for lasers
    //Being lasers rectangles as well, I simply needed their width and height to set up a fairly appropriate collision
    public float laserWidth = 1f;
    public float laserHeight = 2f;

    //Speed of the bullets flying upwards
    public float speed = 1f;

    //I added these variables to adjust the hitbox of the enemies in relation to the bullets
    //At first they were incorrect, and lasers would destroy the enemies withouth hitting them directly
    //Since I didn't want to modify the rectangle's hitbox, I simply added some offsets to adjust them first handed
    float enemyHitboxOffsetX = -1f;
    float enemyHitboxOffsetY = +0.5f;
    float enemyHitboxTrimRight = -1.5f;

    private CollisionDetection collisionDetection;

    private void Start()
    {
        //Calling the collision detection script which contains the ractangular hitbox
        collisionDetection = GetComponent<CollisionDetection>();

        //I am using tags so that if the laser detects the enemy tag, it process the intended code
        //Tags have been extremely helpful in this project as they allowded me to easily identify the various game components by simply calling them out with their name
        //At the same time, not being allowed to attach game objects from the hierarchy onto a prefab, I had to call them and find them with the GetComponent
        spaceship = GameObject.FindWithTag("Player").GetComponent<Spaceship>();
        enemy = GameObject.FindWithTag("Enemy");

    }

    void Update()
    {
        LaserCollision();

        //Move the laser upwards at a defined speed
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        //Destroy the laser if it leaves the screen on the Y-Axis
        if (transform.position.y > 6.1f)
        {
            Destroy(this.gameObject);
        }
    }

    void LaserCollision()
    {
        //Previously I had a simple enemy = GameObject.FindGameObjectsWithTag("Enemy");
        //However, this meethod only counted as the first enemy spawned, meaning that the others where "Tagless"
        //This created issues with the hitboxes not being attached to the enemy
        //Therefore, I updated it to a list method so that every enemy spawned is detected as enemy using the assigned Tag
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        //Once again, since I am dealing with multiple enemies, I want the following to apply to every single one of them
        //So I am basically calling for the single enemy inside the list of enemies multiple times 
        foreach (GameObject enemy in enemies)
        {
            //Get enemy's dimensions - both height and width
            //Localscale gives more precision for the hitbox
            float enemyWidth = enemy.transform.localScale.x;
            float enemyHeight = enemy.transform.localScale.y;

            //Check if the laser collides with the enemy using the rectangle-based collision detection
            //Since I can not use colliders, rectangles are the easiest shape to code through logic
            if (collisionDetection.CheckCollision(
                    transform.position.x, transform.position.y, laserWidth, laserHeight,
                    enemy.transform.position.x + enemyHitboxOffsetX, enemy.transform.position.y + enemyHitboxOffsetY, enemyWidth + enemyHitboxTrimRight, enemyHeight))
            {
                //Debug.Log("Player Laser hit Enemy!");

                //Destroy the laser after it hits the enemy
                Destroy(gameObject);

                //Destroy the enemy after collision with the laser
                Destroy(enemy);

                //If the enemy has been successfuly destroyed by the player's laser, then increase the score by 1 point
                if (spaceship != null)
                {
                    spaceship.IncreaseScore(1);  
                }
                break;
            }
        }
    }
}