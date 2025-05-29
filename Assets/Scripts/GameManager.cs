using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    [SerializeField] PlayerMovement player;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] GameObject startText;
    [SerializeField] GameObject gameOverText;
    [SerializeField] GameObject gameClearText;
    static TextMeshProUGUI _scoreText;
    public static int score = 0;
    private bool gameStarted = false;
    private bool gameOver = false;


    // Start is called before the first frame update
    void Start() {
        _scoreText = scoreText;
    }

    // Update is called once per frame
    void Update() {
        // start game
        if (!gameStarted && Input.GetKeyDown(KeyCode.Space)) {
            player.playerActive = true;
            startText.SetActive(false);
            gameStarted = true;
        }
        // restart game
        else if (gameOver && Input.GetKeyDown(KeyCode.Space)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public static void UpdateScore(int s) {
        score += s;
        _scoreText.text = "Score: " + score;
    }

    public void GameOver() {
        gameOverText.SetActive(true);
        gameOver = true;
    }

    public void GameClear() {
        gameClearText.SetActive(true);
        gameOver = true;
    }
}
