using UnityEngine;
using System.Collections;
public class GameManager : MonoBehaviour/*, IGameContext*/
{
    private int player1Lives;
    private int player2Lives;

    private Vector3 spawn1;
    private Vector3 spawn2;

    private bool isRoundActive;

    private GameObject player1;
    private GameObject player2;

    private PlayerAttributes player1Attributes;
    private PlayerAttributes player2Attributes;


    private bool player1HasBomb;

    public float timerDuration = 10f;  // Initial duration of the timer
    private float currentTime;
    private bool timerRunning = true;
    public float roundDelay = 3f; // Time to wait between rounds

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");

        spawn1 = GameObject.Find("Spawn1").transform.position;
        spawn2 = GameObject.Find("Spawn2").transform.position;

        player1Attributes = GameObject.Find("Player1").GetComponent<PlayerAttributes>();
        player2Attributes = GameObject.Find("Player2").GetComponent<PlayerAttributes>();

        

        player1Lives = 2;
        player2Lives = 2;
        isRoundActive = false;


        int chance = UnityEngine.Random.Range(1, 2);
        if (chance == 1)
        {
            player1Attributes.HasBomb = true;
            player2Attributes.HasBomb = false;

            //player1.Attributes.HasBomb = true;
            //player2.Attributes.HasBomb = false;
        }
        else
        {
            player1Attributes.HasBomb = false;
            player2Attributes.HasBomb = true;

            //player1.Attributes.HasBomb = false;
            //player2.Attributes.HasBomb = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (isRoundActive)
        {
            
            //player1HasBomb = player1.HasBomb ? true : false;
            player1HasBomb = player1Attributes.HasBomb ? true : false;
            //player1HasBomb = player1.Attributes.HasBomb ? true : false;

            //Debug.Log("Round is being played");

            //if bomb timer not 0
            if (currentTime <= 0)
            {
                EndRound(); 
                Debug.Log("Boom! The bomb explodes!");
            }
        }
    }

    public void StartRound()
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
            Debug.Log("Player 2 wins the round. Score: " + player1Lives);
        }
        else
        {
            player2Lives--;
            Debug.Log("Player 1 wins the round. Score: " + player2Lives);
        }

        if (player1Lives == 0 || player2Lives == 0)
        {
            EndGame();
        } 
        else
        {
            //StartRound();
            StartCoroutine(DelayedStartRound());
        }
    }

    private void EndGame()
    {
        string winner = player1Lives == 0 ? "Player2" : "Player1";
        PlayerPrefs.SetString("Winner", winner);
        //MySceneManager.Instance.LoadEndScreen();
        Debug.Log($"Game Ended, Winner: {winner}");
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

    private IEnumerator DelayedStartRound()
    {
        Debug.Log($"Waiting {roundDelay} seconds before starting the next round...");
        yield return new WaitForSeconds(roundDelay);
        Debug.Log("Starting the next round!");

        StartRound();
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
        player1.transform.position = spawn1;
        player2.transform.position = spawn2;

        //Transform p1position = player1.gameobject.position;
        //Transform p2position = player2.gameobject.position;
    }



    float GetBombTimer() => currentTime;
    
    void SetBombTimer(float time)
    {
        currentTime = time;
    }

    //Player[] GetPlayers() => Player[player1, player2].ToArray();

    bool IsGameActive() => isRoundActive;
}
