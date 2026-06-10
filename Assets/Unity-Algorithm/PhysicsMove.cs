using UnityEngine;
using UnityEngine.InputSystem; // 최신 인풋 시스템

[RequireComponent(typeof(Rigidbody))]
public class PhysicsMove : MonoBehaviour
{
    private Rigidbody rb;
    public float pushForce = 10f;
    public float jumpForce = 5f;
    public float checkDistance = 0.6f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, checkDistance);

        Debug.DrawRay(transform.position, Vector3.down * checkDistance, isGrounded ? Color.green : Color.red);

        if (isGrounded && Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void FixedUpdate()
    {
        float h = 0;
        float v = 0;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) h = -1;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) h = 1;
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) v = 1;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) v = -1;
        }

        Vector3 forceDir = new Vector3(h, 0, v).normalized;

        // Rigidbody.AddForce는 Rigidbody에 힘을 가해 물리 엔진이 속도를 바꾸게 하는 메서드입니다.
        rb.AddForce(forceDir * pushForce, ForceMode.Force);
    }
}