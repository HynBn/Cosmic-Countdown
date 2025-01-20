using UnityEngine;


public enum GameState
{
    Idle,
    RoundPlay,
    EndGame
}

public class GameStateManager : StateManager<GameState>
{
    void Awake()
    {
        states[GameState.Idle] = new IdleState();
        states[GameState.RoundPlay] = new RoundPlayState();
        states[GameState.EndGame] = new EndGameState();

        currentState = states[GameState.Idle];
    }
}