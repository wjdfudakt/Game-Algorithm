using UnityEngine;
using UnityEngine.InputSystem;

public class AimTest : MonoBehaviour
{
    [Header("Launch")]
    [Tooltip("Space 키를 눌렀을 때 실제로 생성할 투사체 프리팹입니다. 비워 두면 예측선만 표시합니다.")]
    [SerializeField] private GameObject projectilePrefab;

    [Tooltip("투사체가 발사되는 속도입니다. 값이 클수록 더 멀리 날아갑니다.")]
    [SerializeField] private float launchSpeed = 12f;

    [Tooltip("투사체를 위로 들어 올리는 발사 각도입니다.")]
    [SerializeField] private float launchAngle = 35f;

    [Tooltip("투사체가 좌우로 향하는 방향 각도입니다.")]
    [SerializeField] private float yawAngle = 0f;

    [Header("Prediction")]
    [Tooltip("궤적을 예측할 때 찍을 점의 개수입니다.")]
    [SerializeField] private int maxSteps = 40;

    [Tooltip("예측 점 사이의 시간 간격입니다. 작을수록 더 촘촘하지만 계산이 늘어납니다.")]
    [SerializeField] private float timeStep = 0.08f;

    [Tooltip("Unity 6 Rigidbody의 Linear Damping에 대응하는 예측용 감쇠 값입니다.")]
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

        if (Keyboard.current.leftArrowKey.isPressed)
        {
            yawAngle -= 60f * Time.deltaTime;
        }

        if (Keyboard.current.rightArrowKey.isPressed)
        {
            yawAngle += 60f * Time.deltaTime;
        }

        if (Keyboard.current.downArrowKey.isPressed)
        {
            launchAngle -= 40f * Time.deltaTime;
        }

        if (Keyboard.current.upArrowKey.isPressed)
        {
            launchAngle += 40f * Time.deltaTime;
        }

        launchAngle = Mathf.Clamp(launchAngle, 5f, 80f);

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            FireProjectile();
        }
    }

    private Vector3 GetLaunchVelocity()
    {
        Quaternion rotation = Quaternion.Euler(-launchAngle, yawAngle, 0f);//위 화살표를 눌렀을 때 상승해야 하므로 x는 - 추가

        return rotation * Vector3.forward * launchSpeed;
    }

    private void FireProjectile()
    {
        if (projectilePrefab == null)
        {
            return;
        }

        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
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