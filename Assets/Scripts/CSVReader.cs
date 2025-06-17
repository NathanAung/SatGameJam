using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVReader : MonoBehaviour
{
    [System.Serializable]
    public class WordsPull
    {
        public string mainWord;
        public string correctWord;
        public string[] dummyWords;
    }

    [System.Serializable]
    public class WordList
    {
        public List<WordsPull> wordsPull;
    }

    public WordList quizzes = new WordList();


    void Awake()
    {
        // handling CSV reloading
        GameObject[] objs = GameObject.FindGameObjectsWithTag("CSV");

        if (objs.Length > 1)
        {
            Debug.Log("CSV reader already exists.");
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

        ReadCSV(quizzes, "quizzes");
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void ReadCSV(WordList selectedList, string path)
    {
        try
        {
            var csv = Resources.Load(path) as TextAsset;

            int dummyLength = 2;
            int b = 4;

            string results = csv.text;

            string[] data = results.Split(new string[] { ",", "\n" }, System.StringSplitOptions.None);

            int tableSize = data.Length / b - 1;

            selectedList.wordsPull = new List<WordsPull>();

            for (int i = 0; i < tableSize; i++)
            {
                selectedList.wordsPull.Add(new WordsPull());
                selectedList.wordsPull[i].mainWord = data[b * (i + 1)];
                selectedList.wordsPull[i].correctWord = data[b * (i + 1) + 1];
                selectedList.wordsPull[i].dummyWords = new string[dummyLength];
                for (int j = 0; j < dummyLength; j++)
                {
                    selectedList.wordsPull[i].dummyWords[j] = data[b * (i + 1) + (j + 2)];
                }
            }

        }
        catch
        {
            Debug.LogError("ERROR: File not found");
        }
        ;
    }
}
