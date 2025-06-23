using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RacerPlayer : MonoBehaviour
{
    Rigidbody rb;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 0f;
    [SerializeField] float moveSpeedDefault = 10f;
    [SerializeField] float moveSpeedDash = 100f;
    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;
    Transform orientation;
    private Quaternion targetModelRotation;
    private Vector3 rotDirection;
    public WaypointManager waypointManager;
    private int currentWaypoint = 0;
    RacerPlacement racerPlacement;
    public bool racerActive = false;
    public RaceManager raceManager;
    [SerializeField] TextMeshProUGUI lapText;
    [SerializeField] TextMeshProUGUI posText;
    [SerializeField] TextMeshProUGUI finalPosText;
    [SerializeField] GameObject gameOverText;
    private bool gameOver = false;



    // Start is called before the first frame update
    void Start()
    {
        racerPlacement = GetComponent<RacerPlacement>();
        moveSpeed = moveSpeedDefault;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        orientation = gameObject.transform;
        finalPosText = gameOverText.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (racerActive)
        {
            PlayerInput();
            Transform target = waypointManager.waypoints[currentWaypoint];
            // Check if close enough to switch to next waypoint
            if (Vector3.Distance(transform.position, target.position) < 20f)
            {
                currentWaypoint = (currentWaypoint + 1) % waypointManager.waypoints.Count;
                if (currentWaypoint == 0 && racerPlacement.currentLap >= racerPlacement.maxLaps)
                {
                    moveSpeed = 0;
                    racerActive = false;
                    GameOver();
                }
                else
                {
                    racerPlacement.PassedWaypoint();
                }

            }
            posText.text = "Position: " + raceManager.GetPlayerPos();
            lapText.text = "Lap: " + racerPlacement.currentLap;
        }
        else if (gameOver && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void FixedUpdate()
    {
        Movement();
        SpeedControl();
        if (horizontalInput != 0)
        {
            targetModelRotation = Quaternion.LookRotation(rotDirection.normalized, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetModelRotation, Time.deltaTime * 1f);

        }

    }

    private void PlayerInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        moveSpeed = (verticalInput != 0) ? moveSpeedDefault : moveSpeedDefault / 2;
    }

    private void Movement()
    {
        rotDirection = orientation.forward * 0 + orientation.right * horizontalInput;

        moveDirection = orientation.forward * verticalInput + rotDirection;
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        //string pos = transform.position.ToString();
        //Debug.Log(pos);
    }

    private void SpeedControl()
    {
        Vector3 flatVet = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVet.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVet.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void GameOver()
    {
        posText.gameObject.SetActive(false);
        lapText.gameObject.SetActive(false);
        gameOverText.SetActive(true);
        finalPosText.text = "Final Position: " + raceManager.GetPlayerPos();
        gameOver = true;
    }
}
