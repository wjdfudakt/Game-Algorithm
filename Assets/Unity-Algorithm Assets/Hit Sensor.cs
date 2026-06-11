using UnityEngine;

public class CollisionLog : MonoBehaviour
{
    public string currentState = "대기 중";
    void OnCollisionEnter(Collision collision)
    {
        currentState = "Collision" + collision.gameObject.name;
        Debug.Log("충돌 발생!");
    }
}