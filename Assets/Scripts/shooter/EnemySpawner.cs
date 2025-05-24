using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public bool spawning = false;
    public int activeEnemies = 0;
    int maxEnemies = 15;
    [SerializeField] List<GameObject> enemies;
    [SerializeField] Transform spawnPosUp;
    [SerializeField] Transform spawnPosDown;
    [SerializeField] Transform spawnPosRightUp;
    [SerializeField] Transform spawnPosRightDown;
    float spawnTime = 2f;
    float minSpawnTime = 0.1f;
    float maxSpawnTime = 2f;
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
        int e = Random.Range(0, enemies.Count);

        switch (e)
        {
            case 0:
                if (Random.Range(0, 2) == 0)
                {
                    enemy = Instantiate(enemies[e], spawnPosUp.position, Quaternion.identity);
                    enemy.GetComponent<Enemy>().moveDir = Vector2.down;
                }
                else
                {
                    enemy = Instantiate(enemies[e], spawnPosDown.position, Quaternion.identity);
                    enemy.GetComponent<Enemy>().moveDir = Vector2.up;
                }
                enemy.GetComponent<Enemy>().spawner = this;

                break;
            case 1:
                if (Random.Range(0, 2) == 0)
                {
                    enemy = Instantiate(enemies[e], spawnPosRightUp.position, Quaternion.identity);
                    
                }
                else
                {
                    enemy = Instantiate(enemies[e], spawnPosRightDown.position, Quaternion.identity);
                }
                enemy.GetComponent<Enemy>().moveDir = Vector2.left;
                enemy.GetComponent<Enemy>().spawner = this;
                break;
        }
        activeEnemies += 1;
    }
}
