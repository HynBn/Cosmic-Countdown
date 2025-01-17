using UnityEngine;
using System.Collections;
public class GameManager : MonoBehaviour
{
    private int player1Lives;
    private int player2Lives;

    private Vector3 Spawn1;
    private Vector3 Spawn2;

    private bool isRoundActive;

    private GameObject player1;
    private GameObject player2;

    private PlayerAttributes player1Attributes;
    private PlayerAttributes player2Attributes;


    private bool player1HasBomb;

    public float timerDuration = 10f;  // Initial duration of the timer
    private float currentTime;
    private bool timerRunning = true;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player1");

        Spawn1 = GameObject.Find("SpawnLocation1").transform.position;
        Spawn2 = GameObject.Find("SpawnLocation2").transform.position;

        player1Attributes = GameObject.Find("Player1").GetComponent<PlayerAttributes>();
        player2Attributes = GameObject.Find("Player2").GetComponent<PlayerAttributes>();

        player1Lives = 3;
        player2Lives = 3;
        isRoundActive = false;

        int chance = UnityEngine.Random.Range(1, 2);
        if (chance == 1)
        {
            player1Attributes.HasBomb = true;
            player2Attributes.HasBomb = false;
        }
        else
        {
            player1Attributes.HasBomb = false;
            player2Attributes.HasBomb = true;
        }
        StartRound();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRoundActive)
        {
            //player1HasBomb = player1.HasBomb ? true : false;
            player1HasBomb = GameObject.Find("Player1").GetComponent<PlayerAttributes>().HasBomb ? true : false;


            Debug.Log("Round is being played");

            //if bomb timer not 0
            if (currentTime <= 0)
            {
                EndRound(); 
                Debug.Log("Boom! The bomb explodes!");
            }
        }
    }

    private void StartRound()
    {
        isRoundActive = true;
        placePlayersToSpawn();

        currentTime = timerDuration;
        StartCoroutine(StartTimer());

        
    }

    private void EndRound()
    {
        isRoundActive = false;


        if (player1HasBomb)
        {
            player1Lives--;
            Debug.Log("Player 1 wins the round. Score: " + player1Lives);
        }
        else
        {
            player2Lives--;
            Debug.Log("Player 2 wins the round. Score: " + player2Lives);
        }

        if (player1Lives == 0 || player2Lives == 0)
        {
            EndGame();
        } 
        else
        {
            StartRound();
        }
    }

    private void EndGame()
    {
        string winner = player1Lives == 0 ? "Player1" : "Player2";
        PlayerPrefs.SetString("Winner", winner);
        /////////SceneManager switch to End Screen
    }

    private IEnumerator StartTimer()
    {
        while (currentTime > 0 && timerRunning)
        {
            Debug.Log($"Time left: {currentTime} seconds");
            yield return new WaitForSeconds(1f);  // Wait for 1 second
            currentTime--;
        }

        
    }

    // Public method to add time to the timer
    public void AddTime(float seconds)
    {
        currentTime += seconds;
        Debug.Log($"Time increased! New time left: {currentTime} seconds");
    }

    // Example: Call this function to stop the timer
    public void StopTimer()
    {
        timerRunning = false;
    }

    private void placePlayersToSpawn()
    {
        player1.transform.position = Spawn1;
        player2.transform.position = Spawn2;
    }
}
