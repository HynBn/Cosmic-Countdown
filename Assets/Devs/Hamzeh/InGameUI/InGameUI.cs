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

    [SerializeField]
    private GameManager gameManager;


    private void Update()
    {
        UpdateLivesDisplay();
        UpdateTimer();
    }

    private void UpdateLivesDisplay()
    {
        player1LivesText.text = $"P1: {gameManager.Player1Lives}";
        player2LivesText.text = $"P2: {gameManager.Player2Lives}";
    }

    private void UpdateTimer()
    {
        if (gameManager.IsGameActive())
        {
            float time = gameManager.GetBombTimer();
            timerText.text = $"Time: {Mathf.Ceil(time)}";
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
            roundStartText.text = Mathf.Ceil(startTime).ToString();
            startTime -= Time.deltaTime;
            yield return null;
        }

        roundStartText.text = "GO!";
        yield return new WaitForSeconds(1f);

        roundStartGroup.alpha = 0;
    }
}
