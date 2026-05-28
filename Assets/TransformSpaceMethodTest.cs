using UnityEngine;

public class TransformSpaceMethodTest : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        if (target == null)
        {
            return;
        }

        //Point: 위치 좌표 변환. 이동, 회전, 크기의 영향을 모두 받음
        Vector3 localSpawnPoint = new Vector3(2f, 0f, 0f);
        Vector3 worldSpawnPoint = transform.TransformPoint(localSpawnPoint);
        Vector3 targetLocalPoint = transform.InverseTransformPoint(target.position);

        //Direction: 방향만 변환. 위치, 크기는 무시, 회전만 반영
        Vector3 localForwardDirection = Vector3.forward;
        Vector3 worldForwardDirection = transform.TransformDirection(localForwardDirection);
        Vector3 targetDirectionInWorld = (target.position - transform.position).normalized;
        Vector3 targetDirectionInLocal = transform.InverseTransformDirection(targetDirectionInWorld);

        //Vector: 이동량, 힘의 크기 변환. 위치 이동 무시, 회전, 크기 반영
        Vector3 localKnockbackVector = new Vector3(0f, 0f, 3f);
        Vector3 worldKnockbackVector = transform.TransformVector(localKnockbackVector);
        Vector3 localVelocityVector = transform.InverseTransformVector(worldKnockbackVector);

        string forwardState = targetLocalPoint.z >= 0f ? "앞" : "뒤";
        string sideState = targetLocalPoint.x >= 0f ? "오른쪽" : "왼쪽";

        Debug.Log($"TransformPoint: 로컬 {localSpawnPoint} -> 월드 {worldSpawnPoint}");
        Debug.Log($"InverseTransformPoint: 대상은 내 기준 {forwardState}, {sideState} / 로컬 좌표 {targetLocalPoint}");
        Debug.Log($"TransformDirection: 내 앞 방향 -> 월드 방향 {worldForwardDirection}");
        Debug.Log($"InverseTransformDirection: 대상 방향 -> 내 기준 방향 {targetDirectionInLocal}");
        Debug.Log($"TransformVector: 로컬 넉백량 {localKnockbackVector} -> 월드 넉백량 {worldKnockbackVector}");
        Debug.Log($"InverseTransformVector: 월드 넉백량 -> 로컬 이동량 {localVelocityVector}");
    }

    void OnDrawGizmos()
    {
        Vector3 worldSpawnPoint = transform.TransformPoint(2f, 0f, 0f);//월드 좌표 변환
        Vector3 worldForwardDirection = transform.TransformDirection(Vector3.forward);
        Vector3 worldKnockbackVector = transform.TransformVector(0f, 0f, 3f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(worldSpawnPoint, 0.12f);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + worldForwardDirection * 4f);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + worldKnockbackVector);
    }
}
