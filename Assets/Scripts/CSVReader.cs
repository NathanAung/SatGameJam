using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVReader : MonoBehaviour {
    [System.Serializable]
    public class NoteData {
        public float time;
        public int pos;
    }

    public List<NoteData> noteList = new List<NoteData>();


    // Start is called before the first frame update
    void Start() {
        // handling CSV reloading
        GameObject[] objs = GameObject.FindGameObjectsWithTag("CSV");

        if (objs.Length > 1) {
            Debug.Log("CSV reader already exists.");
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

        noteList = ReadCSV("NoteData");
    }


    // Update is called once per frame
    void Update() {

    }


    public List<NoteData> ReadCSV(string path) {
        List<NoteData> notes = new List<NoteData>();
        
        try {
            var csv = Resources.Load(path) as TextAsset;

            string[] lines = csv.text.Split('\n');

            for (int i = 1; i < lines.Length; i++) {
                string line = lines[i].Trim();

                string[] parts = line.Split(',');

                if (float.TryParse(parts[0], out float time) && int.TryParse(parts[1], out int pos)) {
                    notes.Add(new NoteData { time = time, pos = pos });
                }
                else {
                    Debug.Log("cannot parse data");
                }
            }

        }
        catch {
            Debug.LogError("ERROR: File not found");
        }
        ;

        return notes;
    }
}
