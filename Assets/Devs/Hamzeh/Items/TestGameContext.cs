using UnityEngine;

public class TestGameContext : MonoBehaviour, IGameContext
{
    [SerializeField] private Player player1;
    [SerializeField] private Player player2;

    public Player[] GetPlayers()
    {
        return new Player[] {player1, player2 };
    }

    public float GetBombTimer()
    {
        return 10.0f;
    }

    public void SetBombTimer(float time)
    {
        Debug.Log($"Setting bomb timer to {time} seconds.");
    }

    public bool IsGameActive()
    {
        return true;
    }
}

