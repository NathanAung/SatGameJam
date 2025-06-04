using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] GameObject enemyParent;
    [SerializeField] GameManager gameManager;
    Transform modelTransform;
    public int difficulty = 0;
    private int maxDifficulty = 3;
    [SerializeField] private float[] spawnTimes;
    private float spawnTime = 2f;
    private float spawnTimer = 0f;
    private float difficultyTime = 10f;
    private float difficultyTimer = 0f;
    public bool spawnerActive = false;


    // Start is called before the first frame update
    void Start() {
        modelTransform = transform.GetChild(0).transform;
    }


    // Update is called once per frame
    void Update() {
        if (spawnerActive) {
            if (difficultyTimer < difficultyTime) {
                difficultyTimer += Time.deltaTime;
            }
            else {
                IncreaseDifficulty();
                difficultyTimer = 0f;
            }

            if (spawnTimer < spawnTime) {
                spawnTimer += Time.deltaTime;
            }
            else {
                SpawnEnemy();
                spawnTimer = 0f;
            }

            modelTransform.Rotate(0, 0.1f, 0, Space.Self);
        }
        else if (difficulty > maxDifficulty && enemyParent.transform.childCount == 0) {
            gameManager.GameClear();
            DestroySpawner();
        }
    }


    private void SpawnEnemy() {
        int enemyNo = Random.Range(0, difficulty + 1);
        GameObject e = Instantiate(enemyPrefabs[enemyNo], transform.position, Quaternion.identity, enemyParent.transform);
        e.GetComponent<Enemy>().gameManager = gameManager;
    }


    private void IncreaseDifficulty() {
        difficulty += 1;
        if (difficulty > maxDifficulty) {
            spawnerActive = false;
        }
        else {
            spawnTime = spawnTimes[difficulty];
            Debug.Log("Difficulty increased");
        }
    }


    private void DestroySpawner() {
        Destroy(this.gameObject);
    }
}
