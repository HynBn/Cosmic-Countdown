using System.Collections;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private Player player1;
    [SerializeField] private Player player2;
    [SerializeField] private MonoBehaviour gameContextObject;  // Reference in inspector to object implementing IGameContext

    private IGameContext gameContext;

    private void Awake()
    {
        gameContext = gameContextObject as IGameContext;
        if (gameContext == null)
        {
            Debug.LogError($"The assigned component {gameContextObject.GetType()} does not implement IGameContext!");
            return;
        }

        Player[] players = gameContext.GetPlayers();

        if (players == null)
        {
            Debug.LogError("GetPlayers() returned null!");
            return;
        }

        if (players.Length < 2)
        {
            Debug.LogError($"Insufficient players retrieved. Expected 2, but got {players.Length}.");
            return;
        }

        player1 = players[0];
        player2 = players[1];

        if (player1 == null || player2 == null)
        {
            Debug.LogError("One or more players are null after retrieval!");
        }
    }

    public BaseItem SpawnItem(BaseItem itemPrefab, Vector3 position)
    {
        BaseItem item = Instantiate(itemPrefab, position, Quaternion.identity);
        item.Initialize(this);
        return item;
    }

    public Player GetOtherPlayer(Player currentPlayer)
    {
        return currentPlayer == player1 ? player2 : player1;
    }

    public float GetBombTimer()
    {
        return gameContext.GetBombTimer();
    }

    public void SetBombTimer(float time)
    {
        gameContext.SetBombTimer(time);
    }

    public bool IsGameActive()
    {
        return gameContext.IsGameActive();
    }

    public void SpawnTrapAfterDelay(GameObject prefab, Vector3 position, float delay)
    { 
        StartCoroutine(SpawnTrapAfterDelayCoroutine(prefab, position, delay));
    }
    private IEnumerator SpawnTrapAfterDelayCoroutine(GameObject prefab, Vector3 position, float delay)
    {
        yield return new WaitForSeconds(delay);
        GameObject trap = Instantiate(prefab, position, Quaternion.identity);
        trap.GetComponent<BaseItem>().Initialize(this);
    }
}