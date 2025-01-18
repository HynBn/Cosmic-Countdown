using UnityEngine;
using TMPro;

public class EndScreenManager : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI winnerText;

    [SerializeField]
    private GameObject player1;
    [SerializeField]
    private GameObject player2;

    private Vector3 spawnpoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnpoint = GameObject.Find("winnerSpawner").transform.position;

        var winner = PlayerPrefs.GetString("Winner") == "Player1" ? player1 : player2;
        winnerText.SetText(winner.name.ToString());


        winner.transform.position = spawnpoint;

    }
}
