using UnityEngine;

public class RoundPlayState : BaseState<GameState>
{
    private int player1Score;
    private int player2Score;

    private bool isRoundActive;

    public RoundPlayState() : base(GameState.RoundPlay)
    {
        player1Score = 0;
        player2Score = 0;
        isRoundActive = false;
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
        if (player1Score == 3 || player2Score == 3)
        {
            return GameState.EndGame;
        }

        return GameState.RoundPlay;
    }

    private void StartRound()
    {
        isRoundActive = true;
        Debug.Log("Round started");
    }

    private void EndRound(bool player1Won)
    {
        isRoundActive = false;

        if (player1Won)
        {
            player1Score++;
            Debug.Log("Player 1 wins the round. Score: " + player1Score);
        }
        else
        {
            player2Score++;
            Debug.Log("Player 2 wins the round. Score: " + player2Score);
        }

        if (player1Score < 3 && player2Score < 3)
        {
            StartRound();
        }
    }

    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
}
