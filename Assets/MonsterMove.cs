using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class MonsterMove : MonoBehaviour
{
    public float MonsterMoveSpeed = 10f;

    void Start()
    {

    }

    void Update()
    {
        Vector2 inputVector = Vector2.zero;

        if (Keyboard.current is not null)
        {
            float h = 0;
            float v = 0;

            if (Keyboard.current.leftArrowKey.isPressed) h = -1;
            if (Keyboard.current.rightArrowKey.isPressed) h = 1;
            if (Keyboard.current.upArrowKey.isPressed) v = 1;
            if (Keyboard.current.downArrowKey.isPressed) v = -1;

            inputVector = new Vector2(h, v);
        }

        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y).normalized;

        if (moveDir.magnitude > 0)
        {
            transform.Translate(moveDir * MonsterMoveSpeed * Time.deltaTime, Space.World);

            transform.rotation = Quaternion.LookRotation(moveDir);//회전 기능 추가
        }
    }        
}