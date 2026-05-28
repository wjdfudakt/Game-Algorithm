using UnityEngine;
using UnityEngine.InputSystem;

public class Plane : MonoBehaviour
{
    [Header("Rotation")]
    public float rotationSpeed = 90f;

    [Header("Movement")]
    public float startSpeed = 5f;
    public float acceleration = 3f;
    public float maxSpeed = 50f;
    public float deceleration = 2f;

    float currentSpeed;

    void Start()
    {
        currentSpeed = startSpeed;
    }

    void Update()
    {
        Keyboard keyboard = Keyboard.current;

        if (keyboard == null)
            return;

        Vector3 input = Vector3.zero;

        //-------------------------------------------------
        // 회전 입력
        //-------------------------------------------------

        // Roll
        if (keyboard.rightArrowKey.isPressed)
        {
            input.z -= 1f;
        }

        if (keyboard.leftArrowKey.isPressed)
        {
            input.z += 1f;
        }

        // Pitch
        if (keyboard.downArrowKey.isPressed)
        {
            input.x -= 1f;
        }

        if (keyboard.upArrowKey.isPressed)
        {
            input.x += 1f;
        }

        // Yaw
        if (keyboard.qKey.isPressed)
        {
            input.y -= 1f;
        }

        if (keyboard.eKey.isPressed)
        {
            input.y += 1f;
        }

        //-------------------------------------------------
        // Quaternion 회전
        //-------------------------------------------------

        Quaternion deltaRotation =
            Quaternion.Euler(input * rotationSpeed * Time.deltaTime);

        transform.rotation =
            transform.rotation * deltaRotation;

        //-------------------------------------------------
        // 속도 처리
        //-------------------------------------------------

        // 가속
        if (keyboard.wKey.isPressed)
        {
            currentSpeed += acceleration * Time.deltaTime;
        }

        // 감속
        if (keyboard.sKey.isPressed)
        {
            currentSpeed -= deceleration * Time.deltaTime;
        }

        // 속도 제한
        currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);

        //-------------------------------------------------
        // 이동
        //-------------------------------------------------

        transform.position +=
            transform.forward * currentSpeed * Time.deltaTime;
    }
}