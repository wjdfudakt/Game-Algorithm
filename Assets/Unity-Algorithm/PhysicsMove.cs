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
        // GetComponent<T>()는 같은 게임 오브젝트에 붙은 T 타입 컴포넌트를 가져오는 메서드입니다.
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {

        // Physics.Raycast는 시작점에서 방향으로 보이지 않는 선을 쏴 충돌 여부를 검사하는 메서드입니다.
        // Vector3.down은 월드 기준 아래 방향인 (0, -1, 0) 벡터입니다.
        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, checkDistance);

        // Debug.DrawRay는 씬 뷰에 디버그용 광선을 그려 Raycast 방향을 눈으로 확인하게 해 줍니다.
        Debug.DrawRay(transform.position, Vector3.down * checkDistance, isGrounded ? Color.green : Color.red);

        // Input System: 스페이스바를 누른 순간 확인
        // wasPressedThisFrame은 해당 키가 이번 프레임에 막 눌렸을 때만 true가 되는 프로퍼티입니다.
        if (isGrounded && Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            // Vector3.up은 월드 기준 위 방향인 (0, 1, 0) 벡터입니다.
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    // FixedUpdate는 물리 계산 주기에 맞춰 자동 호출되므로 Rigidbody 이동에 적합합니다.
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