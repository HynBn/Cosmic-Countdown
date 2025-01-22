using TMPro;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    [Header("Player Lives")]
    [SerializeField] private TextMeshProUGUI player1LivesText;
    [SerializeField] private TextMeshProUGUI player2LivesText;

    [Header("Timer")]
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Round Start")]
    [SerializeField] private TextMeshProUGUI roundStartText;
    [SerializeField] private CanvasGroup roundStartGroup;

    [Header("Pause")]
    [SerializeField] private TextMeshProUGUI pauseText;

    [SerializeField]
    private GameManager gameManager;

    private void Start()
    {
        // Initialize pause text as hidden
        pauseText.enabled = false;
        pauseText.text = "PAUSED";
    }

    private void Update()
    {
        UpdateLivesDisplay();
        UpdateTimer();
        UpdatePauseDisplay();
    }

    private void UpdateLivesDisplay()
    {
        player1LivesText.text = $":{gameManager.Player1Lives}";
        player2LivesText.text = $"{gameManager.Player2Lives}:";
    }

    private void UpdatePauseDisplay()
    {
        if (gameManager != null)
        {
            pauseText.enabled = gameManager.IsPaused;
        }
    }

    private void UpdateTimer()
    {
        if (gameManager.IsGameActive())
        {
            float time = gameManager.GetBombTimer();
            //timerText.text = $"Time: {Mathf.Ceil(time)}";
            timerText.text = $"{Mathf.Ceil(time)}";

        }
    }

    public void ShowRoundStartCountdown(float duration)
    {
        StartCoroutine(RoundStartCountdownRoutine(duration));
    }

    private System.Collections.IEnumerator RoundStartCountdownRoutine(float duration)
    {
        roundStartGroup.alpha = 1;
        float startTime = duration;

        while (startTime > 0)
        {
            // Only countdown if game is not paused
            if (!gameManager.IsPaused)
            {
                roundStartText.text = Mathf.Ceil(startTime).ToString();
                startTime -= Time.deltaTime;
            }
            // Hide the countdown text while paused
            roundStartGroup.alpha = gameManager.IsPaused ? 0 : 1;

            yield return null;
        }

        // Only show "GO!" and continue if not paused
        if (!gameManager.IsPaused)
        {
            roundStartText.text = "GO!";
            yield return new WaitForSeconds(1f);
        }

        roundStartGroup.alpha = 0;
    }
}
