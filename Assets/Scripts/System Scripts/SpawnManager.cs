using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemy;
    public GameObject enemySpawner;

    public float spawnTime = 5f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            Vector2 posToSpawn = new Vector3(Random.Range(-9.5f, 9.5f), 6.2f);
            GameObject newEnemy = Instantiate(enemy, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = enemySpawner.transform;
            yield return new WaitForSeconds(spawnTime);
        }
    }
}
