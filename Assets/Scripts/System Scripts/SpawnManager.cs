using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script manages the spawning mechanics of all the various powerups and enemies
//I used coroutines for two reasons
//The first one is because I want enemy to spawn on a cycle every few seconds, with a possibility to stop this coroutine when the player dies
//The second reason is to cover the coroutine requirements from the assignment brief (both having at least one coroutine and stopping it)

public class SpawnManager : MonoBehaviour
{
    //variables to reference the enemy spawwner gameobject and the powerups
    public GameObject enemySpawner;
    public GameObject enemy;
    public GameObject shieldPowerUp;
    public GameObject speedPowerUp;
    public GameObject fireRatePowerUp;

    //Setting up the coroutine to spawn enemies
    private Coroutine spawnCoroutine;
    private Coroutine powerUpSpawnCoroutine;

    //Variable to handle the time in between spawning of enemies
    public float spawnTime = 5f;

    void Start()
    {
        //Starting the coroutine right at the beginning of the game so that the first enemy spawns instantly
        spawnCoroutine = StartCoroutine(SpawnRoutine());

        //For the powerups, the spawntimer also starts at the game but it will take a few seconds to see the first powerup
        powerUpSpawnCoroutine = StartCoroutine(SpawnPowerUpRoutine());
    }

    //Update is called once per frame
    void Update()
    {
        
    }

    //What is happening in this coroutine?
    IEnumerator SpawnRoutine()
    {
        while (true) //while the coroutine is taking action...
        {
            //Spawn an enemy at the top and then wait the previosuly announced amount of time to spawn a new enemy
            SpawnEnemyAtTop(); //(Refer below for details)
            yield return new WaitForSeconds(spawnTime);
        }
    }

    //What is happening in this coroutine instead?
    IEnumerator SpawnPowerUpRoutine()
    {
        while (true) //while the coroutine is taking action...
        {
            //Establish a random value in a range to spawn the powerups
            yield return new WaitForSeconds(Random.Range(2f, 5f));

            //Establish a random position on the X-Axis, while the Y valuse is fixed at the top of the screen
            Vector2 posToSpawn = new Vector2(Random.Range(-9.5f, 9.5f), 6.2f);

            //Establish a random powerup to spawn among the three
            int randomChoice = Random.Range(0, 3);

            //The first option is the shield powerup
            if (randomChoice == 0)
            {
                Instantiate(shieldPowerUp, posToSpawn, Quaternion.identity);
            }
            //Otherwise, option 2 is the speed boost
            else if (randomChoice == 1)
            {
                Instantiate(speedPowerUp, posToSpawn, Quaternion.identity);
            }
            //Otherwise option 3 is the fire rate boost
            else if (randomChoice == 2)
            {
                Instantiate(fireRatePowerUp, posToSpawn, Quaternion.identity);
            }
        }
    }

    //This part of the code deals instead with the spawning mechanic of the enemy 
    //In particular it's dealing with the position of the enemies
    public void SpawnEnemyAtTop()
    {
        //Once again, establish a random value on the X-Axis while the Y value is fixed
        Vector2 posToSpawn = new Vector3(Random.Range(-9.5f, 9.5f), 6.2f);

        //Actually instantiate the enemy at the random position
        GameObject newEnemy = Instantiate(enemy, posToSpawn, Quaternion.identity);

        //This line is to count the spawned enemy a child of the part, which is the enemy spawner itself
        newEnemy.transform.parent = enemySpawner.transform;
    }

    //This function is what allows the coroutine to be stopped
    public void StopSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }

        if (powerUpSpawnCoroutine != null)
        {
            StopCoroutine(powerUpSpawnCoroutine);
            powerUpSpawnCoroutine = null;
        }
    }
}
