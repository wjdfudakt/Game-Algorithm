using UnityEngine;
using UnityEngine.InputSystem;

public class Plane : MonoBehaviour
{
    public float rotationSpeed = 90f;//회전 속도

    public float startSpeed = 5f;//시작 속도
    public float acceleration = 3f;//가속도
    public float maxSpeed = 50f;//최대 속도
    public float deceleration = 2f;//감속도

    float currentSpeed;//현재 속도

    void Start()
    {
        currentSpeed = startSpeed;//현재 속도를 시작 속도로 설정
    }

    void FixedUpdate()
    {
        Keyboard keyboard = Keyboard.current;

        if (keyboard == null)
            return;

        Vector3 input = Vector3.zero;

        // 회전 입력

        // Roll(롤, 오브젝트 Z축 기준 회전)
        if (keyboard.rightArrowKey.isPressed)
        {
            input.z -= 1f;
        }

        if (keyboard.leftArrowKey.isPressed)
        {
            input.z += 1f;
        }

        // Pitch(피치,오브젝트 X축 기준 회전, 상하 회전)
        if (keyboard.downArrowKey.isPressed)
        {
            input.x -= 1f;
        }

        if (keyboard.upArrowKey.isPressed)
        {
            input.x += 1f;
        }

        // Yaw(요잉, 오브젝트 Y축 기준 회전, 좌우 회전)
        if (keyboard.aKey.isPressed)
        {
            input.y -= 1f;
        }

        if (keyboard.dKey.isPressed)
        {
            input.y += 1f;
        }

        //쿼터니언 회전

        Quaternion deltaRotation = Quaternion.Euler(input * rotationSpeed * Time.deltaTime);

        transform.rotation = transform.rotation * deltaRotation;

        // 가속
        if (keyboard.wKey.isPressed)
        {
            currentSpeed += acceleration * Time.deltaTime;//현재 속도를 가속도 * 시간에 따라 적용. 누르는 만큼 증가
        }

        // 감속
        if (keyboard.sKey.isPressed)
        {
            currentSpeed -= deceleration * Time.deltaTime;//가속과 같이 적용. 단, 감속으로.
        }

        // 속도 제한
        currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);

        transform.position += transform.forward * currentSpeed * Time.deltaTime;//위치를 바라보는 방향 * 현재 속도 * 시간에 따라 이동
    }
}