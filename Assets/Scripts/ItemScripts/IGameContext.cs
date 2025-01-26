public interface IGameContext
{
    float GetBombTimer();
    void SetBombTimer(float time);
    Player[] GetPlayers();
    bool IsGameActive();
}