using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemy;
    public GameObject enemySpawner;

    public float spawnTime = 5f;

    private Coroutine spawnCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        spawnCoroutine = StartCoroutine(SpawnRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnEnemyAtTop();
            yield return new WaitForSeconds(spawnTime);
        }
    }

    public void SpawnEnemyAtTop()
    {
        Vector2 posToSpawn = new Vector3(Random.Range(-9.5f, 9.5f), 6.2f);
        GameObject newEnemy = Instantiate(enemy, posToSpawn, Quaternion.identity);
        newEnemy.transform.parent = enemySpawner.transform;
    }

    public void StopSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }
}
