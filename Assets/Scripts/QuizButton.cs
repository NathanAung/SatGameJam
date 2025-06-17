using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizButton : MonoBehaviour {
    Quiz quiz;
    public bool correct = false;

    // Start is called before the first frame update
    void Start() {
        quiz = transform.parent.GetComponent<Quiz>();
    }

    // Update is called once per frame
    void Update() {

    }

    public void SetButton(string answer, bool c) {
        transform.GetChild(0).GetComponent<Text>().text = answer;
        correct = c;
    }

    public void OnButtonPress() {
        if (correct)
            quiz.CorrectAnswer();
        else
            quiz.IncorrectAnswer();
    }
}
