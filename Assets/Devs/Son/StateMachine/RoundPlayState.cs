using UnityEditor.Overlays;
using UnityEngine;

public class RoundPlayState : BaseState<GameState>
{
    private int player1Lives;
    private int player2Lives;

    private bool isRoundActive;
    private Collider2D collider2D;

    //private Player player1;
    //private Player player2;

    public RoundPlayState() : base(GameState.RoundPlay)
    {
        //player1 = GameObject.Find("Player1").GetComponent<Player>();
        //player2 = GameObject.Find("Player2").GetComponent<Player>();

        player1Lives = 3;
        player2Lives = 3;
        isRoundActive = false;
        collider2D = GameObject.Find("Player1").GetComponent<Collider2D>();
    }

    public override void Enterstate()
    {
        Debug.Log("Entering RoundPlay State");
        StartRound();
    }

    public override void ExitState()
    {
        Debug.Log("Exiting RoundPlay State");
    }

    public override void UpdateState()
    {
        

        if (isRoundActive)
        {
            Debug.Log("Round is being played");

            //if bomb timer not 0 {}

            

            // Simulate round end condition for example purposes
            if (Input.GetKeyDown(KeyCode.A))
            {
                EndRound(true);
            }
            else if (Input.GetKeyDown(KeyCode.B))
            {
                EndRound(false);
            }
        }
    }

    public override GameState GetNextState()
    {
        if (player1Lives == 0 || player2Lives == 0)
        {
            string winner = player1Lives == 0 ? "Player1" : "Player2";
            PlayerPrefs.SetString("Winner", winner);
            return GameState.EndGame;
        }

        return GameState.RoundPlay;
    }

    private void StartRound()
    {
        isRoundActive = true;
        Debug.Log("Round started");

        //Instantiate and set fields to the player
    }

    private void EndRound(bool player1Won)
    {
        isRoundActive = false;

        if (player1Won)
        {
            player1Lives--;
            Debug.Log("Player 1 wins the round. Score: " + player1Lives);
        }
        else
        {
            player2Lives--;
            Debug.Log("Player 2 wins the round. Score: " + player2Lives);
        }

        if (player1Lives > 0 && player2Lives > 0)
        {
            StartRound();
        }
    }

    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
}
