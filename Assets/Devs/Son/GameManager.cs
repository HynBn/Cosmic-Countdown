using UnityEngine;
using System.Collections;
public class GameManager : MonoBehaviour
{
    private int player1Lives;
    private int player2Lives;

    private bool isRoundActive;

    /////////private Player player1;
    /////////private Player player2;

    private bool player1HasBomb;

    public float timerDuration = 10f;  // Initial duration of the timer
    private float currentTime;
    private bool timerRunning = true;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        /////////player1 = GameObject.Find("Player1").GetComponent<Player>();
        /////////player2 = GameObject.Find("Player2").GetComponent<Player>();

        player1Lives = 3;
        player2Lives = 3;
        isRoundActive = false;
        ////collider2D = GameObject.Find("Player1").GetComponent<Collider2D>();

        /////////int chance = UnityEngine.Random.Range(1, 2);
        /////////if (chance == 1) Player1.updateBombStatus;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRoundActive)
        {
            /////////player1HasBomb = Player1.HasBomb ? true : false;
            Debug.Log("Round is being played");

            //if bomb timer not 0 {}
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
        currentTime = timerDuration;
        StartCoroutine(StartTimer());
        //Instantiate and set fields to the player
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
}
