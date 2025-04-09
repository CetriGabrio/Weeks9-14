using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script manages the spawning mechanics of enemies
//I used coroutines for two reasons
//The first one is because I want enemy to spawn on aa cycle every few seconds, with a possibility to stop this coroutine when the player dies
//The second reason is to cover the coroutine requirements from the assignment brief (both having at least one coroutine and stopping it)

public class SpawnManager : MonoBehaviour
{
    //variables to reference the enemy spawwner gameobject
    public GameObject enemySpawner;
    public GameObject enemy; //and the object being spawned (the enemy)
    public GameObject shieldPowerUp;

    //Variable to handle the time in between spawning of enemies
    public float spawnTime = 5f;

    //Setting up the coroutine to spawn enemies
    private Coroutine spawnCoroutine;
    private Coroutine powerUpSpawnCoroutine;

    //Start is called before the first frame update
    void Start()
    {
        //Starting the coroutine right at the beginning of the game
        spawnCoroutine = StartCoroutine(SpawnRoutine());
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

    IEnumerator SpawnPowerUpRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3f, 8f));

            Vector2 posToSpawn = new Vector2(Random.Range(-9.5f, 9.5f), 6.2f);
            Instantiate(shieldPowerUp, posToSpawn, Quaternion.identity);
        }
    }

    //Fuction that handles the spawning mechanic of the enemy 
    //In particular it's dealing with the position of the enemies
    public void SpawnEnemyAtTop()
    {
        Vector2 posToSpawn = new Vector3(Random.Range(-9.5f, 9.5f), 6.2f);
        GameObject newEnemy = Instantiate(enemy, posToSpawn, Quaternion.identity);
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
