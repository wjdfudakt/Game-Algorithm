using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Plane : MonoBehaviour
{
    public float rotationSpeed = 90f;//회전 속도

    public float startSpeed = 5f;//시작 속도
    public float acceleration = 3f;//가속도
    public float maxSpeed = 50f;//최대 속도
    public float deceleration = 2f;//감속도

    //public float gravity = -9.81f;//중력가속도
    //[SerializeField] private float currentVelocityY = 0f;//중력으로 인한 y값 감속 속도

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

        // Roll(롤, 오브젝트 Z축 기준 회전
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
        if (keyboard.qKey.isPressed)
        {
            input.y -= 1f;
        }

        if (keyboard.eKey.isPressed)
        {
            input.y += 1f;
        }

        // Quaternion 회전(쿼터니언 회전 적용)

        Quaternion deltaRotation = Quaternion.Euler(input * rotationSpeed * Time.deltaTime);

        transform.rotation = transform.rotation * deltaRotation;

        // 속도

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

        //중력 구현
        //bool gravityActive = true;

        //Vector3 pos = transform.position;

        //if (currentSpeed < 1f)//현재 속도가 1보다 낮을 경우
        //{
        //    gravityActive = true;//중력 작동
        //    if (gravityActive = true)//중력이 작동중일 때
        //    {
        //        currentVelocityY += gravity + Time.deltaTime;//중력 가속도 * 시간에 따라 Y값 속도 증가

        //        pos.y += currentVelocityY * Time.deltaTime;//Y값 속도 * 시간에 따라 실제 Y값 변환

        //        if (pos.y <= 0f)//현재 오브젝트의 위치가 
        //        {
        //            pos.y = 0;
        //            currentSpeed = 0;
        //        }

        //        transform.position = pos;
        //    }
        //}

        //if (currentSpeed >= 1f)
        //{
        //    gravityActive = false;
        //    currentVelocityY = 0;
        //}
    }    
}