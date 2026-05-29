using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private float distance = 5f;
    [SerializeField] private float height = 2f;

    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private float rotationSmooth = 5f;

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        // 타겟 뒤 방향
        Vector3 backDir = -target.forward;

        // 목표 위치
        Vector3 desiredPos =
            target.position +
            backDir * distance +
            Vector3.up * height;

        // 부드럽게 따라가기
        transform.position =
            Vector3.SmoothDamp(
                transform.position,
                desiredPos,
                ref velocity,
                smoothTime);

        // 바라볼 위치
        Vector3 lookTarget =
            target.position + Vector3.up * 1.2f;

        // 목표 회전
        Quaternion targetRot =
            Quaternion.LookRotation(
                lookTarget - transform.position);

        // 부드러운 회전
        transform.rotation =
            Quaternion.Slerp(
                transform.rotation,
                targetRot,
                rotationSmooth * Time.deltaTime);
    }
}