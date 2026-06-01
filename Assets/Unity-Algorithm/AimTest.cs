using UnityEngine;
using UnityEngine.InputSystem;

public class AimRay : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float aimDistance = 10f;
    [SerializeField] private float moveSpeed = 5f;

    private Vector2 screenOffset; // 화면 기준 이동

    public Vector3 AimPoint { get; private set; }

    void Update()
    {
        Vector2 input = Vector2.zero;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed) input.y += 1;
            if (Keyboard.current.sKey.isPressed) input.y -= 1;
            if (Keyboard.current.aKey.isPressed) input.x -= 1;
            if (Keyboard.current.dKey.isPressed) input.x += 1;            
        }

        screenOffset += input * moveSpeed * Time.deltaTime;

        screenOffset = Vector2.ClampMagnitude(screenOffset, 3f);

        Vector3 screenCenter = new Vector3(
            Screen.width / 2f + screenOffset.x * 50f,
            Screen.height / 2f + screenOffset.y * 50f,
            0f
        );

        Ray ray = cam.ScreenPointToRay(screenCenter);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, aimDistance))
        {
            AimPoint = hit.point;
        }
        else
        {
            AimPoint = ray.origin + ray.direction * aimDistance;
        }

        Debug.DrawRay(ray.origin, ray.direction * aimDistance, Color.red);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(AimPoint, 0.2f);
    }
}