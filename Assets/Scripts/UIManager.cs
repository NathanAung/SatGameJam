using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject titleText;
    [SerializeField] GameObject gameOverText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] GameObject background;
    [SerializeField] Spawner spawner;
    bool gameStarted = false;
    bool gameOver = false;
    int currentScore = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!gameStarted && Input.GetKeyDown(KeyCode.Space))
        {
            titleText.SetActive(false);
            background.SetActive(true);
            spawner.SpawnBlock();
            gameStarted = true;
        }
        else if (gameOver && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void GameOver()
    {
        if (gameOver) return;

        gameOverText.SetActive(true);
        gameOver = true;
    }

    public void UpdateScore()
    {
        currentScore += 100;
        scoreText.text = "Score: " + currentScore;
    }
}
