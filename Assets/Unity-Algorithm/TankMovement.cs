using UnityEngine;
using UnityEngine.InputSystem;

public class Tank : MonoBehaviour
{
    public float rotationSpeed = 90f;

    public float startSpeed = 5f;
    public float acceleration = 3f;
    public float maxSpeed = 10f;
    public float deceleration = 4f;

    float currentSpeed;

    void Start()
    {
        currentSpeed = startSpeed;
    }

    void FixedUpdate()
    {
        Keyboard keyboard = Keyboard.current;

        if (keyboard == null)
            return;

        float turnInput = 0f;

        if (keyboard.aKey.isPressed)
            turnInput -= 1f;

        if (keyboard.dKey.isPressed)
            turnInput += 1f;

        transform.Rotate(0f, turnInput * rotationSpeed * Time.deltaTime, 0f);

        if (keyboard.wKey.isPressed)
        {
            currentSpeed += acceleration * Time.deltaTime;
        }
        else if (keyboard.sKey.isPressed)
        {
            currentSpeed -= deceleration * Time.deltaTime;
        }
        else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0f, 2f * Time.deltaTime);
        }

        currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);

        transform.position += transform.forward * currentSpeed * Time.deltaTime;
    }
}