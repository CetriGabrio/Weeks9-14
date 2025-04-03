using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Laser : MonoBehaviour
{
    //List of enemies in the scene
    public GameObject enemy;

    //Player position and size
    public GameObject player;

    //Dimensions for bullets
    public float laserWidth = 1f;
    public float LaserHeight = 2f;

    public float speed = 1f;

    void Update()
    {
        LaserCollision();

        transform.Translate(Vector3.up * speed * Time.deltaTime);
        Destroy(gameObject, 3f);
    }

    void LaserCollision()
    {
        float playerWidth = 1.0f;  //Define player's width
        float playerHeight = 2.0f; //Define player's height

        //Check collision for Player Bullets
        if (gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            float enemyWidth = enemy.transform.localScale.x;
            float enemyHeight = enemy.transform.localScale.y;

            if (GetComponent<CollisionDetection>().CheckCollision(
                    transform.position.x, transform.position.y, laserWidth, LaserHeight,
                    enemy.transform.position.x, enemy.transform.position.y, enemyWidth, enemyHeight))
            {
                Debug.Log("Player Bullet hit Enemy!");
            }
        }

        //Check collision for Enemy Bullets
        if (gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (GetComponent<CollisionDetection>().CheckCollision(
                    transform.position.x, transform.position.y, laserWidth, LaserHeight,
                    player.transform.position.x, player.transform.position.y, playerWidth, playerHeight))
            {
                Debug.Log("Enemy Bullet hit Player!");
            }
        }
    }
}
