using UnityEngine;

public class MatrixTest : MonoBehaviour
{
    public void OnDrawGizmos()//자동 호출 메서드
    {
        Matrix4x4 worldMatrix = transform.localToWorldMatrix;//로컬 매트릭스를 월드 매트릭스로 변환

        Vector3 localPos = new Vector3(2f, 0, 0);//로컬 상의 위치

        Vector3 worldPos = worldMatrix.MultiplyPoint3x4(localPos);//행렬 곱을 통한 월드 좌표로 변환

        //시각화
        Gizmos.color = Color.yellow;
        
        Gizmos.DrawLine(transform.position, worldPos);//원점에서 변환된 점까지 선 그리기

        Gizmos.DrawSphere(worldPos, 0.1f);//변환된 최종 위치를 구체로 표시
    }
}
