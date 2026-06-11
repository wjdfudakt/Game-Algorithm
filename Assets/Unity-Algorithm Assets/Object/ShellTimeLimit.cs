using UnityEngine;

public class ShellTimeLimit : MonoBehaviour
{
    [Header("time")]
    [SerializeField] private float Time = 5f;

    private void Start()
    {
        //일정 시간 후 삭제
        Destroy(gameObject, Time);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Trigger 사용 시도 대응
        Destroy(gameObject);
    }
}