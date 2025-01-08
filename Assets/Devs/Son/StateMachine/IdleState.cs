using UnityEngine;

public class IdleState : BaseState<GameState>
{
    public IdleState() : base(GameState.Idle) { }

    public override void Enterstate()
    {
        Debug.Log("Entering Idle State");
    }

    public override void ExitState()
    {
        Debug.Log("Exiting Idle State");
    }

    public override void UpdateState()
    {
        // Wait for user input to start the game
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Starting Game");
        }
    }

    public override GameState GetNextState()
    {
        // Transition to RoundPlay when the game starts
        if (Input.GetKeyDown(KeyCode.Space))
        {
            return GameState.RoundPlay;
        }

        return GameState.Idle;
    }

    public override void OnTriggerEnter(Collider other) { }
    public override void OnTriggerStay(Collider other) { }
    public override void OnTriggerExit(Collider other) { }
}
