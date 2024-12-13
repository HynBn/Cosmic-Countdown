using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement : MonoBehaviour
{
    //[SerializeField]
    //private float speed = 5f;

    InputAction left;
    InputAction right;
    InputAction jump;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        left = InputSystem.actions.FindAction("Left");
        right = InputSystem.actions.FindAction("Right");
        jump = InputSystem.actions.FindAction("Jump");
    }

    // Update is called once per frame
    void Update()
    {
        if (left.IsPressed())
        {
            transform.position += Vector3.left * Time.deltaTime;
        }
        if (right.IsPressed())
        {
            transform.position += Vector3.right * Time.deltaTime;
        }
        if (jump.IsPressed())
        {
            transform.position += Vector3.up * Time.deltaTime;
        }
    }
}
