using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class NoteSpawner : MonoBehaviour {
    [SerializeField] GameObject NotePrefab;
    [SerializeField] AudioSource music;
    [SerializeField] CSVReader csvReader;
    [SerializeField] List<CSVReader.NoteData> csvList;
    [SerializeField] List<GameObject> NoteList;
    [SerializeField] TextMeshProUGUI pfmText;   // performance text
    private float perfectScaleMin = 0.52f;
    private float perfectScaleMax = 0.56f;
    private float goodScaleMin = 0.46f;
    private float goodScaleMax = 0.6f;
    [SerializeField] private float noteTravelTime = 0.94f;
    private int noteCount = 0;
    private float pfmTextTime = 2f;
    private float pfmTextTimer = 0f;


    // Start is called before the first frame update
    void Start() {
        csvList = csvReader.noteList;
        music.Play();
    }

    // Update is called once per frame
    void Update() {
        float musicTime = music.time;

        while (noteCount < csvList.Count && musicTime >= csvList[noteCount].time - noteTravelTime) {
            SpawnNote();
            noteCount++;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            if (NoteList.Count > 0) {
                if (NoteList[0] != null) {
                    if (NoteList[0].transform.localScale.x >= perfectScaleMin && NoteList[0].transform.localScale.x <= perfectScaleMax) {
                        pfmText.text = "PERFECT";
                        Debug.Log("PERFECT");
                    }
                    else if (NoteList[0].transform.localScale.x >= goodScaleMin && NoteList[0].transform.localScale.x <= goodScaleMax) {
                        pfmText.text = "GOOD";
                        Debug.Log("GOOD");
                    }
                    else {
                        pfmText.text = "BAD";
                        Debug.Log("BAD");
                    }
                    Destroy(NoteList[0].gameObject);
                    pfmTextTimer = 0;
                }
            }
        }

        // remove empty
        NoteList.RemoveAll(s => s == null);

        if (pfmTextTimer < pfmTextTime) {
            pfmTextTimer += Time.deltaTime;
        }
        else {
            pfmText.text = "";
            pfmTextTimer = 0;
        }
    }

    private void SpawnNote() {
        GameObject n = Instantiate(NotePrefab, transform.position, quaternion.identity);
        NoteList.Add(n);
    }
}
