using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] Vector3 rotationPoint;
    private float fallTime = 0.8f;
    private float previousTime = 0f;
    static int height = 20;
    static int width = 10;
    private static Transform[,] grid = new Transform[width, height];
    bool movedDown = false;
    bool blockDisabled = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (blockDisabled && transform.childCount == 0)
        {
            Debug.Log("block destroyed");
            Destroy(this.gameObject);
        }
        else if (!blockDisabled)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                transform.position += new Vector3(-1, 0, 0);
                if (!ValidMove())
                    transform.position += new Vector3(1, 0, 0);

            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                transform.position += new Vector3(1, 0, 0);
                if (!ValidMove())
                    transform.position += new Vector3(-1, 0, 0);
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
                if (!ValidMove())
                    transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);

            }

            if (Time.time - previousTime > (Input.GetKey(KeyCode.S) ? fallTime / 10 : fallTime))
            {
                transform.position += new Vector3(0, -1, 0);
                if (!ValidMove())
                {
                    transform.position += new Vector3(0, 1, 0);
                    if (!movedDown)
                    {
                        FindObjectOfType<Spawner>().spawning = false;
                        FindObjectOfType<UIManager>().GameOver();
                        Debug.Log("Game Over");
                    }
                    else
                    {
                        AddToGrid();
                        CheckForLines();
                        FindObjectOfType<Spawner>().SpawnBlock();
                    }
                    //this.enabled = false;
                    blockDisabled = true;
                }
                movedDown = true;
                previousTime = Time.time;
            }
        }
    }

    private void AddToGrid()
    {
        foreach (Transform child in transform)
        {
            int roundedX = Mathf.RoundToInt(child.position.x);
            int roundedY = Mathf.RoundToInt(child.position.y);

            grid[roundedX, roundedY] = child;
        }
    }

    private bool ValidMove()
    {
        foreach (Transform child in transform)
        {
            int roundedX = Mathf.RoundToInt(child.position.x);
            int roundedY = Mathf.RoundToInt(child.position.y);

            if(roundedX < 0 || roundedX >= width || roundedY < 0 || roundedY >= height)
            {
                return false;
            }

            if(grid[roundedX, roundedY] != null)
                return false;
        }
        return true;
    }

    private void CheckForLines()
    {
        for (int i = height - 1; i >= 0; i--)
        {
            if (HasLine(i))
            {
                DestroyLine(i);
                RowDown(i);
            }
        }
    }
    private bool HasLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            if(grid[j, i] == null)
                return false;
        }
        return true;
    }

    private void DestroyLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            Destroy(grid[j, i].gameObject);
            grid[j, i] = null;
        }
        FindObjectOfType<UIManager>().UpdateScore();
    }

    private void RowDown(int i)
    {
        for(int y = i; y < height; y++)
        {
            for (int j = 0; j < width; j++)
            {
                if (grid[j, y] != null)
                {
                    grid[j, y - 1] = grid[j,y];
                    grid[j, y] = null;
                    grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);
                }

            }
        }
        
    }
}
