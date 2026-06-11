using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Rigidbody targetRb;

    [SerializeField] private float distance = 20f;
    [SerializeField] private float height = 10f;
    [SerializeField] private float rotationSmooth = 1000000f;

    private Vector3 lastMoveDir = Vector3.forward;

    void LateUpdate()
    {
        Vector3 moveDir = targetRb.linearVelocity;

        // 거의 멈췄을 때는 마지막 이동 방향 유지
        if (moveDir.sqrMagnitude > 0.01f)
        {
            moveDir.Normalize();
            lastMoveDir = moveDir;
        }

        Vector3 desiredPos = target.position - lastMoveDir * distance + Vector3.up * height;

        transform.position = desiredPos;

        Vector3 lookTarget = target.position + Vector3.up * 1.2f;

        Quaternion targetRot = Quaternion.LookRotation(lookTarget - transform.position);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSmooth * Time.deltaTime);
    }
}