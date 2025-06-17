using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    [SerializeField] Quiz quiz;
    [SerializeField] GameObject titleText;
    [SerializeField] GameObject gameOverText;
    [SerializeField] GameObject gameClearText;
    [SerializeField] Text scoreText;
    int score = 0;
    private bool gameStarted = false;
    public bool gameEnded = false;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (!gameStarted && Input.GetKeyDown(KeyCode.Space)) {
            quiz.GetQuiz();
            titleText.SetActive(false);
            gameStarted = true;
        }
        else if (gameEnded && Input.GetKeyDown(KeyCode.Space)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void GameOver(bool win) {
        gameEnded = true;
        if (win)
            gameClearText.SetActive(true);
        else
            gameOverText.SetActive(true);
    }

    public void UpdateScore() {
        score++;
        scoreText.text = "Correct: " + score + "/5";
    }
}
