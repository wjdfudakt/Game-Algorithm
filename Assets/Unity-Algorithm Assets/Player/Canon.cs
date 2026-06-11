using UnityEngine;
using UnityEngine.InputSystem;

public class Canon : MonoBehaviour
{
    [Header("Launch")]
    [Tooltip("Space 키를 눌렀을 때 실제로 생성할 투사체 프리팹. 비워 두면 예측선만 표시")]
    [SerializeField] private GameObject projectilePrefab;

    [Tooltip("투사체 발사 속도")]
    [SerializeField] private float launchSpeed = 12f;

    [Tooltip("투사체 상하 발사 각도")]
    [SerializeField] private float launchAngle = 35f;

    [Header("Prediction")]
    [Tooltip("궤적 예측 시 점의 개수")]
    [SerializeField] private int maxSteps = 40;

    [Tooltip("예측 점 사이의 시간 간격")]
    [SerializeField] private float timeStep = 0.08f;

    [Tooltip("탄이 발사되는 위치")]
    [SerializeField] private Transform firePoint;

    [Tooltip("Unity 6 Rigidbody의 Linear Damping에 대응하는 예측용 감쇠 값")]
    [SerializeField] private float linearDamping = 0f;

    private Vector3 hitPoint;
    private Vector3 lastPredictedPoint;
    private bool hasHit;

    private void Update()
    {
        if (Keyboard.current == null)
        {
            return;
        }

        if (Keyboard.current.downArrowKey.isPressed)
        {
            launchAngle -= 40f * Time.deltaTime;
        }

        if (Keyboard.current.upArrowKey.isPressed)
        {
            launchAngle += 40f * Time.deltaTime;
        }

        launchAngle = Mathf.Clamp(launchAngle, -10f, 20f);

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            FireProjectile();
        }
    }

    private Vector3 GetLaunchVelocity()
    {
        if (firePoint == null)
            return transform.forward * launchSpeed;

        Vector3 forward = firePoint.forward;

        Quaternion pitch = Quaternion.AngleAxis(-launchAngle, firePoint.right);

        Vector3 direction = pitch * forward;

        return direction.normalized * launchSpeed;
    }

    private void FireProjectile()
    {
        if (projectilePrefab == null || firePoint == null)
        {
            return;
        }

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Rigidbody body = projectile.GetComponent<Rigidbody>();

        if (body == null)
        {
            return;
        }

        body.linearDamping = linearDamping;
        body.useGravity = true;
        body.linearVelocity = GetLaunchVelocity();
    }

    private void OnDrawGizmos()
    {
        Vector3 position = transform.position;
        Vector3 velocity = GetLaunchVelocity();

        hasHit = false;
        lastPredictedPoint = position;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(position, 0.15f);

        for (int i = 0; i < maxSteps; i++)
        {
            Vector3 previousPosition = position;

            velocity *= 1f - linearDamping * timeStep;

            velocity += Physics.gravity * timeStep;
            position += velocity * timeStep;

            Vector3 move = position - previousPosition;
            float distance = move.magnitude;

            if (Physics.Raycast(previousPosition, move.normalized, out RaycastHit hit, distance))
            {
                hasHit = true;
                hitPoint = hit.point;

                Gizmos.color = Color.red;
                Gizmos.DrawLine(previousPosition, hit.point);
                Gizmos.DrawWireSphere(hit.point, 0.25f);
                break;
            }

            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(previousPosition, position);
            Gizmos.DrawWireSphere(position, 0.05f);
            lastPredictedPoint = position;
        }

        if (!hasHit)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(lastPredictedPoint, 0.25f);
            return;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(hitPoint, 0.08f);
    }
}
