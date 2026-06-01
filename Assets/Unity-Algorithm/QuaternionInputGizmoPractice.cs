using UnityEngine;
using UnityEngine.InputSystem;

public class QuaternionInputGizmoPractice : MonoBehaviour
{
    public float rotationSpeed = 4f;
    public float targetMoveSpeed = 3f;
    public float targetDistance = 4f;
    public float targetRange = 4f;

    Vector3 targetOffset = new Vector3(0f, 0f, 4f);

    void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null)
        {
            return;
        }

        Vector3 input = Vector3.zero;

        if (keyboard.leftArrowKey.isPressed)
        {
            input.x -= 1f;
        }

        if (keyboard.rightArrowKey.isPressed)
        {
            input.x += 1f;
        }

        if (keyboard.shiftKey.isPressed)
        {
            input.y -= 1f;
        }

        if (keyboard.ctrlKey.isPressed)
        {
            input.y += 1f;
        }

        if (keyboard.downArrowKey.isPressed)
        {
            input.z -= 1f;
        }

        if (keyboard.upArrowKey.isPressed)
        {
            input.z += 1f;
        }

        if (keyboard.spaceKey.wasPressedThisFrame)//wasPressedThisFrame - 이번 프레임에 눌렸을 때만 true가 되는 프로퍼티.
        {
            targetOffset = new Vector3(0f, 0f, 0f);
        }

        targetOffset += new Vector3(input.x, input.y, input.z) * targetMoveSpeed * Time.deltaTime;// Time.deltaTime - 이전 프레임에서 현재 프레임까지 걸린 시간. 이동 속도를 일정하게 맞출 때 사용.
        targetOffset.x = Mathf.Clamp(targetOffset.x, -targetRange, targetRange);//Mathf.Clamp - 값을 최대값과 최소값 사이로 제한하는 매서드.
        targetOffset.y = Mathf.Clamp(targetOffset.y, -targetRange, targetRange);
        //targetOffset.z = targetDistance;
        //targetOffset.y = targetDistance;
        targetOffset.z = Mathf.Clamp(targetOffset.z, -targetRange, targetRange);

        Vector3 targetDirection = targetOffset.normalized;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);//Quaternion.LookRotation - 지정한 방향을 바라보도록 회전시키는 매서드. Vector3.up - (0, 1, 0).

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);//transform.rotation - 오브젝트의 회전값. Quaternion.Slerp - 곡선을 그리며 부드럽게 회전.
    }

    private void OnDrawGizmos()
    {
        Vector3 origin = transform.position;//transform.position - 현재 오브젝트의 월드 위치
        Vector3 targetPosition = origin + targetOffset;
        Vector3 targetDirection = targetOffset.normalized;

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(targetPosition, 0.15f);//Gizmos.DrawSphere - Scene(씬)뷰에 구체를 그려 특정 위치를 표시하는 매서드(target).

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(origin, origin + transform.forward * targetDistance);//Gizmos.DrawLine - Scene(씬)뷰에 두 점을 잇는 선을 그리는 매서드. transform.forward - 현재 오브젝트가 바라보는 방향.

        Gizmos.color = Color.red;
        Gizmos.DrawLine(origin, origin + targetDirection * targetDistance);
    }


}
