using UnityEngine;
using TMPro;

public class EndScreenManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro winnerText;
    [SerializeField]
    private GameObject player1;
    [SerializeField]
    private GameObject player2;
    private Vector3 spawnpoint;

    void Start()
    {
        spawnpoint = GameObject.Find("winnerSpawner").transform.position;
        var winner = PlayerPrefs.GetString("Winner") == "Player1" ? player1 : player2;
        winnerText.SetText(winner.name.ToString());
        winner.transform.position = spawnpoint;

        // Get and reset the animator directly
        Animator animator = winner.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("IsMoving", false);
            animator.SetBool("IsJumping", false);
        }
    }
}
