using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Variable for the falling down speed of the enemy
    public float speed = 1f;

    private SpawnManager spawnManager;

    //public GameObject enemyLaserPrefab;

    // Start is called before the first frame update
    void Start()
    {
        spawnManager = FindObjectOfType<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        EnemyMovement();
    }

    void EnemyMovement()
    {
        //Function to move the enemy only downwards at a constant speed
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        //Pac-Man effect once again, this time vetically to ensure that if the enemy reaches the bottom of the screen, it teleports to the top
        if (transform.position.y < -6.2f)
        {
            Destroy(gameObject);

            if (spawnManager != null)
            {
                spawnManager.SpawnEnemyAtTop();
            }
        }
    }
}
