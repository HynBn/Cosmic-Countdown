using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private MonoBehaviour gameContextObject;
    [SerializeField] private Transform spawnPointsParent;
    [SerializeField] private List<BaseItem> itemPrefabs = new List<BaseItem>();

    private const int MAX_ITEMS = 5;
    private const float SPAWN_INTERVAL = 1f;
    private List<Transform> availableSpawnPoints;
    private List<BaseItem> activeItems = new List<BaseItem>();
    private int currentItemCount = 0;
    private IGameContext gameContext;
    private Player player1;
    private Player player2;
    private bool wasGameActive;

    private void Awake()
    {
        InitializeGameContext();
        InitializeSpawnPoints();
        StartCoroutine(SpawnRoutine());
    }

    private void Update()
    {
        bool isGameActive = gameContext.IsGameActive();
        if (wasGameActive && !isGameActive)
        {
            ResetItems();
        }
        wasGameActive = isGameActive;
    }

    private void ResetItems()
    {
        // Clean up all active items
        foreach (var item in activeItems)
        {
            if (item != null)
            {
                Destroy(item.gameObject);
            }
        }
        activeItems.Clear();
        currentItemCount = 0;

        // Reset spawn points
        availableSpawnPoints.Clear();
        foreach (Transform child in spawnPointsParent)
        {
            availableSpawnPoints.Add(child);
        }
    }

    private void InitializeGameContext()
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
    }

    private void InitializeSpawnPoints()
    {
        if (spawnPointsParent == null)
        {
            Debug.LogError("SpawnPoints parent not assigned to ItemManager!");
            return;
        }

        availableSpawnPoints = new List<Transform>();
        foreach (Transform child in spawnPointsParent)
        {
            availableSpawnPoints.Add(child);
        }

        if (availableSpawnPoints.Count == 0)
        {
            Debug.LogWarning("No spawn points found in SpawnPoints parent!");
        }
    }

    private IEnumerator SpawnRoutine()
    {
        float timer = SPAWN_INTERVAL;

        while (true)
        {
            if (gameContext.IsGameActive())
            {
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    if (currentItemCount < MAX_ITEMS && availableSpawnPoints.Count > 0)
                    {
                        SpawnRandomItem();
                    }
                    timer = SPAWN_INTERVAL;
                }
            }
            yield return null;
        }
    }

    private void SpawnRandomItem()
    {
        if (availableSpawnPoints.Count == 0 || itemPrefabs.Count == 0) return;

        int spawnIndex = Random.Range(0, availableSpawnPoints.Count);
        Transform spawnPoint = availableSpawnPoints[spawnIndex];
        availableSpawnPoints.RemoveAt(spawnIndex);

        BaseItem itemPrefab = itemPrefabs[Random.Range(0, itemPrefabs.Count)];
        BaseItem item = Instantiate(itemPrefab, spawnPoint.position, Quaternion.identity);
        item.Initialize(this, spawnPoint);
        activeItems.Add(item);
        currentItemCount++;
    }

    public void OnItemCollected(Transform spawnPoint)
    {
        availableSpawnPoints.Add(spawnPoint);
        currentItemCount--;
    }

    public void SpawnTrapAfterDelay(GameObject prefab, Vector3 position, float delay, Transform spawnPoint)
    {
        StartCoroutine(SpawnTrapAfterDelayCoroutine(prefab, position, delay, spawnPoint));
    }

    private IEnumerator SpawnTrapAfterDelayCoroutine(GameObject prefab, Vector3 position, float delay, Transform spawnPoint)
    {
        yield return new WaitForSeconds(delay); // Will respect TimeScale

        if (gameContext.IsGameActive())
        {
            GameObject trap = Instantiate(prefab, position, Quaternion.identity);
            BaseItem trapItem = trap.GetComponent<BaseItem>();
            trapItem.Initialize(this, spawnPoint);
            activeItems.Add(trapItem);
        }
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
}