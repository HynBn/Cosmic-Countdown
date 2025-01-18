using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController Controller { get; private set; }
    public PlayerAttributes Attributes { get; private set; }
    public PlayerModifier Modifier { get; private set; }
    public BombInteraction BombInteraction { get; private set; }

    private void Awake()
    {
        // Cache all components
        Controller = GetComponent<PlayerController>();
        Attributes = GetComponent<PlayerAttributes>();
        Modifier = GetComponent<PlayerModifier>();
        BombInteraction = GetComponentInChildren<BombInteraction>();

        // Validate required components
        if (Controller == null) Debug.LogError($"Missing PlayerController on {gameObject.name}");
        if (Attributes == null) Debug.LogError($"Missing PlayerAttributes on {gameObject.name}");
        if (Modifier == null) Debug.LogError($"Missing PlayerModifier on {gameObject.name}");
        if (BombInteraction == null) Debug.LogError($"Missing BombInteraction in children of {gameObject.name}");
    }
}