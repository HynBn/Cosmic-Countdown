using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private Player player1;
    [SerializeField] private Player player2;
    [SerializeField] private MonoBehaviour gameContextObject;
    [SerializeField] private Transform spawnPointsParent;
    [SerializeField] private List<BaseItem> itemPrefabs = new List<BaseItem>();

    private const int MAX_ITEMS = 3;
    private const float SPAWN_INTERVAL = 5f;
    private List<Transform> availableSpawnPoints;
    private int currentItemCount = 0;
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
        player1 = players[0];
        player2 = players[1];

        // Validate spawn points parent
        if (spawnPointsParent == null)
        {
            Debug.LogError("SpawnPoints parent not assigned to ItemManager!");
            return;
        }

        // Initialize available spawn points with all children of the SpawnPoints object
        availableSpawnPoints = new List<Transform>();
        foreach (Transform child in spawnPointsParent)
        {
            availableSpawnPoints.Add(child);
        }

        if (availableSpawnPoints.Count == 0)
        {
            Debug.LogWarning("No spawn points found in SpawnPoints parent!");
        }

        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(SPAWN_INTERVAL);

            if (gameContext.IsGameActive() && currentItemCount < MAX_ITEMS && availableSpawnPoints.Count > 0)
            {
                SpawnRandomItem();
            }
        }
    }

    private void SpawnRandomItem()
    {
        if (availableSpawnPoints.Count == 0 || itemPrefabs.Count == 0) return;

        // Get random spawn point and remove it from available points
        int spawnIndex = Random.Range(0, availableSpawnPoints.Count);
        Transform spawnPoint = availableSpawnPoints[spawnIndex];
        availableSpawnPoints.RemoveAt(spawnIndex);

        // Spawn random item
        BaseItem itemPrefab = itemPrefabs[Random.Range(0, itemPrefabs.Count)];
        BaseItem item = Instantiate(itemPrefab, spawnPoint.position, Quaternion.identity);
        item.Initialize(this, spawnPoint);
        currentItemCount++;
    }

    public void OnItemCollected(Transform spawnPoint)
    {
        availableSpawnPoints.Add(spawnPoint);
        currentItemCount--;
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

    public void SpawnTrapAfterDelay(GameObject prefab, Vector3 position, float delay, Transform spawnPoint)
    {
        StartCoroutine(SpawnTrapAfterDelayCoroutine(prefab, position, delay, spawnPoint));
    }

    private IEnumerator SpawnTrapAfterDelayCoroutine(GameObject prefab, Vector3 position, float delay, Transform spawnPoint)
    {
        yield return new WaitForSeconds(delay);
        GameObject trap = Instantiate(prefab, position, Quaternion.identity);
        trap.GetComponent<BaseItem>().Initialize(this, spawnPoint);
    }
}