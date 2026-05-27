using UnityEngine;
using UnityEngine.InputSystem;

public class PracticeTransform : MonoBehaviour
{
    public float moveSpeed = 10f;//이동속도 10f로 설정

    void Start()
    {
        
    }

    void Update()
    {
        Vector2 inputVector = Vector2.zero;

        if (Keyboard.current is not null)//키보드가 연결되어 있는지 확인
        {
            float h = 0;//가로
            float v = 0;//세로

            if (Keyboard.current.aKey.isPressed) h = -1;
            if (Keyboard.current.dKey.isPressed) h = 1;
            if (Keyboard.current.wKey.isPressed) v = 1;
            if (Keyboard.current.sKey.isPressed) v = -1;

            inputVector = new Vector2(h, v);
        }

        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y).normalized;//가로 세로만 움직일 것이기 때문에 Y=0으로 설정

        if (moveDir.magnitude > 0)
        {
            transform.Translate(moveDir * moveSpeed * Time.deltaTime, Space.World);
        }
    }
}
