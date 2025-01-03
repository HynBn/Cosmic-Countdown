using UnityEngine;

public class EndGameState : BaseState<GameState>
{
    public EndGameState() : base(GameState.EndGame) { }

    public override void Enterstate()
    {
        Debug.Log("Entering EndGame State");
        Debug.Log("Game Over!");
    }

    public override void ExitState()
    {
        Debug.Log("Exiting EndGame State");
    }

    public override void UpdateState()
    {
        // Wait for restart or exit input
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Restarting Game");
        }
    }

    public override GameState GetNextState()
    {
        // Restart game by going to Idle state
        if (Input.GetKeyDown(KeyCode.R))
        {
            return GameState.Idle;
        }

        return GameState.EndGame;
    }

    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
}
