using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour, IGameContext
{
    [SerializeField] private Transform spawn1;
    [SerializeField] private Transform spawn2;
    [SerializeField] private Player player1;
    [SerializeField] private Player player2;
    [SerializeField] private float timerDuration = 15f;
    [SerializeField] private float roundDelay = 3f;
    [SerializeField] private InGameUI gameUI;

    public float CurrentTime { get; private set; }
    public int Player1Lives { get; private set; }
    public int Player2Lives { get; private set; }

    private bool isRoundActive;
    private bool isPaused;

    public bool IsPaused
    {
        get => isPaused;
        private set
        {
            isPaused = value;
            Time.timeScale = value ? 0f : 1f;
            if (!value && isRoundActive) 
            {
                SetPlayerControlsEnabled(true);
            }
            else if (value) 
            {
                SetPlayerControlsEnabled(false);
            }
        }
    }

    private void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        Player1Lives = 2;
        Player2Lives = 2;
        StartRound();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            IsPaused = !IsPaused;
        }
    }

    private void StartRound()
    {
        StartCoroutine(StartRoundRoutine());
    }

    private IEnumerator StartRoundRoutine()
    {
        // Start round transition
        SetPlayerControlsEnabled(false);
        PlacePlayersAtSpawns();

        if (gameUI != null)
        {
            gameUI.ShowRoundStartCountdown(roundDelay);
        }

        yield return new WaitForSeconds(roundDelay);


        // End round transition and start round
        isRoundActive = true;
        SetPlayerControlsEnabled(true);
        AssignBomb();
        ResetTimer();
        StartCoroutine(StartTimer());
    }

    private void SetPlayerControlsEnabled(bool isEnabled)
    {
        player1.Controller.enabled = isEnabled;
        player2.Controller.enabled = isEnabled;
    }

    private void PlacePlayersAtSpawns()
    {
        player1.transform.position = spawn1.position;
        player1.transform.rotation = spawn1.rotation;
        player2.transform.position = spawn2.position;
        player2.transform.rotation = spawn2.rotation;
    }

    private void AssignBomb()
    {
        bool player1HasBomb = Random.Range(0, 2) == 0;
        player1.Attributes.HasBomb = player1HasBomb;
        player2.Attributes.HasBomb = !player1HasBomb;
    }

    private void ResetTimer()
    {
        CurrentTime = timerDuration;
    }

    private void EndRound()
    {
        isRoundActive = false;
        SetPlayerControlsEnabled(false);
        UpdateLives();

        if (Player1Lives == 0 || Player2Lives == 0)
        {
            EndGame();
        }
        else
        {
            StartRound();
        }
    }

    private void UpdateLives()
    {
        if (player1.Attributes.HasBomb)
        {
            Player1Lives--;
        }
        else
        {
            Player2Lives--;
        }
    }

    private void EndGame()
    {
        PlacePlayersAtSpawns();
        string winner = Player1Lives == 0 ? "Player2" : "Player1";
        PlayerPrefs.SetString("Winner", winner);

    }

    private IEnumerator StartTimer()
    {
        while (CurrentTime > 0 && isRoundActive)
        {
            if (!IsPaused)
            {
                yield return new WaitForSeconds(1f);
                CurrentTime--;

                if (CurrentTime <= 0)
                {
                    EndRound();
                }
            }
            else
            {
                yield return null;
            }
        }
    }

    // IGameContext implementation
    public float GetBombTimer() => CurrentTime;
    public void SetBombTimer(float time) => CurrentTime = time;
    public Player[] GetPlayers() => new Player[] { player1, player2 };
    public bool IsGameActive() => isRoundActive;
}