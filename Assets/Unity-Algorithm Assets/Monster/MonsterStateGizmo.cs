using UnityEngine;

public class MonsterStateGizmo : MonoBehaviour
{
    private enum MonsterState
    {
        Idle,//대기
        detect,//발견
        Chase,//추적
        Attack//공격
    }

    [Header("Target")]
    [SerializeField] private Transform player;

    [Header("Distance")]
    [SerializeField] private float idleDistance = 0f;
    [SerializeField] private float detectDistance = 10f;
    [SerializeField] private float chaseDistance = 6f;
    [SerializeField] private float attackDistance = 1.8f;

    [SerializeField] private float detectStayTime = 5f;

    private float detectTimer = 0f;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f; // 이동 속도

    private MonsterState currentState = MonsterState.Idle;

    private void Update()
    {
        if (player == null)
        {
            currentState = MonsterState.Idle;
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        bool canSeePlayer = CanSeePlayer();

        if (!canSeePlayer)
        {
            currentState = MonsterState.Idle;
            return;
        }

        if (distance <= attackDistance)
        {
            currentState = MonsterState.Attack;
        }
        else if (distance <= chaseDistance)
        {
            currentState = MonsterState.Chase;

            transform.position = Vector3.MoveTowards(
                transform.position,
                player.position,
                moveSpeed * Time.deltaTime
            );
        }
        else if (distance <= detectDistance)
        {
            detectTimer += Time.deltaTime;

            if (detectTimer >= detectStayTime)
            {
                currentState = MonsterState.Chase;

                transform.position = Vector3.MoveTowards(
                    transform.position,
                    player.position,
                    moveSpeed * Time.deltaTime
                );
            }
            else
            {
                currentState = MonsterState.detect;
            }
        }
        else
        {
            detectTimer = 0f;
            currentState = MonsterState.Idle;
        }
    }

    private bool CanSeePlayer()
    {
        if (player == null)
            return false;

        Vector3 origin = transform.position + Vector3.up * 0.5f;
        Vector3 target = player.position + Vector3.up * 0.5f;

        Vector3 direction = (target - origin).normalized;
        float distance = Vector3.Distance(origin, target);

        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance))
        {
            return hit.transform == player;
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = GetStateColor();
        Gizmos.DrawSphere(transform.position, 0.35f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectDistance);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackDistance);

        if (player == null)
            return;

        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, player.position);
    }

    private Color GetStateColor()
    {
        switch (currentState)
        {
            case MonsterState.detect:
                return Color.green;
            case MonsterState.Chase:
                return Color.yellow;
            case MonsterState.Attack:
                return Color.red;
            default:
                return Color.gray;
        }
    }
}