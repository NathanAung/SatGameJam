using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Quiz : MonoBehaviour {
    [SerializeField] GameManager gameManager;
    [SerializeField] int quizCount = 5;
    [SerializeField] CSVReader csvReader;
    public int rightAnsNo = 0;
    [SerializeField] Text question;
    [SerializeField] QuizButton[] quizButtons;
    [SerializeField] private List<int> askedQuestions = new List<int> { };


    // Start is called before the first frame update
    void Start() {

    }


    // Update is called once per frame
    void Update() {

    }

    public void GetQuiz() {
        int r = Random.Range(0, 11);
        if (askedQuestions.Contains(r)) {
            while (askedQuestions.Contains(r))
                r = Random.Range(0, 11);
        }
        askedQuestions.Add(r);

        SetButtons(true);
        question.text = csvReader.quizzes.wordsPull[r].mainWord;

        int correctPos = Random.Range(0, 3);
        int[] wrongPos = new int[] { 0, 1 };
        if (Random.Range(0, 2) == 0)
            wrongPos = new int[] { 1, 0 };
        int w = 0;


        for (int i = 0; i < quizButtons.Length; i++) {
            if (i == correctPos) {
                quizButtons[i].SetButton(csvReader.quizzes.wordsPull[r].correctWord, true);
            }
            else {
                quizButtons[i].SetButton(csvReader.quizzes.wordsPull[r].dummyWords[wrongPos[w]], false);
                w++;
            }
        }

    }

    public void CorrectAnswer() {
        if (quizCount <= 0) return;

        Debug.Log("correct");
        gameManager.UpdateScore();
        quizCount--;

        if (quizCount > 0) {
            GetQuiz();
        }
        else {
            SetButtons(false);
            gameManager.GameOver(true);
        }
    }

    public void IncorrectAnswer() {
        Debug.Log("incorrect");
        SetButtons(false);
        gameManager.GameOver(false);
    }

    private void SetButtons(bool active) {
        question.gameObject.SetActive(active);
        for (int i = 0; i < quizButtons.Length; i++) {
            quizButtons[i].gameObject.SetActive(active);
        }
    }
}
