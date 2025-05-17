using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public bool spawning = true;
    public int activeEnemies = 0;
    int maxEnemies = 15;
    [SerializeField] List<GameObject> enemies;
    [SerializeField] Transform spawnPosUp;
    [SerializeField] Transform spawnPosDown;
    float spawnTime = 2f;
    float minSpawnTime = 0.5f;
    float maxSpawnTime = 3f;
    float spawnTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!spawning) return;

        if(spawnTimer < spawnTime)
        {
            spawnTimer += Time.deltaTime;
        }
        else
        {
            SpawnEnemy();
            spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
            spawnTimer = 0f;
        }
    }

    public void SpawnEnemy()
    {
        if (activeEnemies >= maxEnemies) return;

        GameObject enemy;
        if(Random.Range(0,2) == 0)
        {
            enemy = Instantiate(enemies[Random.Range(0, enemies.Count)], spawnPosUp.position, Quaternion.identity);
            enemy.GetComponent<Enemy>().moveDir = Vector2.down;
        }
        else
        {
            enemy = Instantiate(enemies[Random.Range(0, enemies.Count)], spawnPosDown.position, Quaternion.identity);
            enemy.GetComponent<Enemy>().moveDir = Vector2.up;
        }
        enemy.GetComponent<Enemy>().spawner = this;
        activeEnemies += 1;
    }
}
