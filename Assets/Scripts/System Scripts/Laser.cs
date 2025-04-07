using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Laser : MonoBehaviour
{
    //List of enemies in the scene
    public GameObject enemy;

    //Player position and size
    //public GameObject player;

    //Dimensions for bullets
    public float laserWidth = 1f;
    public float laserHeight = 2f;

    public float speed = 1f;

    private CollisionDetection collisionDetection;

    private void Start()
    {
        collisionDetection = GetComponent<CollisionDetection>();

        //I am using tags so that if the laser detects the enemy tag, it process the intended code
        //In this case it would deal damage and destroy the enemy witouth impacting other elements
        enemy = GameObject.FindWithTag("Enemy");

    }

    void Update()
    {
        LaserCollision();

        transform.Translate(Vector3.up * speed * Time.deltaTime);

        //Destroy the laser if it leaves the screen
        if (transform.position.y > 6.1f)
        {
            Destroy(this.gameObject);
        }
    }

    void LaserCollision()
    {
        if (enemy == null) return;  //If no enemy is found, return early

        //Get enemy's dimensions - both height and width
        float enemyWidth = enemy.transform.localScale.x;
        float enemyHeight = enemy.transform.localScale.y;

        //Check if the laser collides with the enemy using the rectangle-based collision detection
        //Since I can not use colliders, rectangles are the easiest shape to code through logic
        if (collisionDetection.CheckCollision(
                transform.position.x, transform.position.y, laserWidth, laserHeight,
                enemy.transform.position.x, enemy.transform.position.y, enemyWidth, enemyHeight))
        {
            //Debug.Log("Player Laser hit Enemy!");

            //Destroy the laser after it hits the enemy
            Destroy(gameObject);

            //Destroy the enemy after collision with the laser
            Destroy(enemy);
        }
    }
}