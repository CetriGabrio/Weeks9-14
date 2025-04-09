using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//This script deals with the main behaviour of the enemy
//Obviously, I made this element a Prefab since it is going to be instantiated multiple times during the game

public class Enemy : MonoBehaviour
{
    //Variable for the vertical falling speed of the enemy
    public float speed = 1f;

    //Reference to the spawnmanagere, which is storing the enemies
    private SpawnManager spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        //Since I cannot attach a gaeobject to this prefab, I had to call the SpawnManager directly through code
        spawnManager = FindObjectOfType<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        EnemyMovement();
    }

    //Dealing with the enemy movement
    void EnemyMovement()
    {
        //Function to move the enemy only downwards at a constant speed
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        //I started by making a Pac-Man effect also here, where if the enemy reaches the bottom it teleports to the top
        //However, this caused a bug with the hitbox system not being transported, therefore I changed the system
        //Now, when the enemy reaches the bottom of the screen, it gets destroyed and a copy of it is instantly instantiated at the top
        if (transform.position.y < -6.2f)
        {
            Destroy(gameObject);

            //Since I wanted to keep the unpredictability of the spawn on a random X position (and because I had already written the code in the other script)
            //I am simply referencing the spawning function in the spawnManager 
            if (spawnManager != null)
            {
                spawnManager.SpawnEnemyAtTop();
            }
        }
    }
}
