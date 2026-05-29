using UnityEngine;
using UnityEngine.InputSystem;

public class MonsterMove : MonoBehaviour
{
    public float MonsterMoveSpeed = 10f;

    void Start()
    {

    }

    void Update()
    {
        Vector3 inputVector = Vector3.zero;

        if (Keyboard.current is not null)
        {
            float x = 0;
            float y = 0;
            float z = 0;

            if (Keyboard.current.aKey.isPressed) x = -1;
            if (Keyboard.current.dKey.isPressed) x = 1;
            if (Keyboard.current.qKey.isPressed) y = -1;
            if (Keyboard.current.eKey.isPressed) y = 1;
            if (Keyboard.current.wKey.isPressed) z = 1;
            if (Keyboard.current.sKey.isPressed) z = -1;

            inputVector = new Vector3(x, y, z);
        }

        Vector3 moveDir = new Vector3(inputVector.x, inputVector.y, inputVector.z).normalized;

        if (moveDir.magnitude > 0)
        {
            transform.Translate(moveDir * MonsterMoveSpeed * Time.deltaTime, Space.World);
        }
    }
}