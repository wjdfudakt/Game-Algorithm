using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class BfsGizmoVisualizer : MonoBehaviour
{
    [Header("grid")]
    [Tooltip("가로 칸 수")]
    [SerializeField] private int width = 3;

    [Tooltip("세로 칸 수")]
    [SerializeField] private int height = 3;

    [Tooltip("Scene 뷰에 그릴 칸의 크기")]
    [SerializeField] private float cellSize = 1f;

    [Header("Obstacle")]
    [Tooltip("이동할 수 없는 벽 좌표들")]
    [SerializeField] private List<Vector2Int> walls = new List<Vector2Int>();

    private readonly Queue<Vector2Int> frontier = new Queue<Vector2Int>();
    private readonly HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
    private readonly HashSet<Vector2Int> processed = new HashSet<Vector2Int>();
    private Vector2Int currentNode;

    private bool IsWall(Vector2Int node)
    {
        return walls.Contains(node);
    }

    private void Start()
    {
        ResetSearch();
    }

    private void Update()
    {
        if (Keyboard.current == null)
        {
            return;
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            StepSearch();
        }

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            ResetSearch();
        }
    }

    private void ResetSearch()
    {
        frontier.Clear();
        visited.Clear();
        processed.Clear();

        currentNode = new Vector2Int(0, 0);
        frontier.Enqueue(currentNode);
        visited.Add(currentNode);
    }

    private void StepSearch()
    {
        if (frontier.Count == 0)
        {
            return;
        }

        currentNode = frontier.Dequeue();

        processed.Add(currentNode);

        foreach (Vector2Int neighbor in GetNeighbors(currentNode))
        {
            if (IsWall(neighbor))
            {
                continue;
            }

            if (visited.Contains(neighbor))
            {
                continue;
            }

            visited.Add(neighbor);
            frontier.Enqueue(neighbor);
        }
    }

    private List<Vector2Int> GetNeighbors(Vector2Int node)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        Vector2Int[] directions =
        {
            Vector2Int.up,
            Vector2Int.right,
            Vector2Int.down,
            Vector2Int.left, 
        };

        foreach (Vector2Int direction in directions)
        {
            Vector2Int next = node + direction;

            if (next.x < 0 || next.x >= width || next.y < 0 || next.y >= height)
            {
                continue;
            }

            neighbors.Add(next);
        }

        return neighbors;
    }

    private void OnDrawGizmos()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector2Int node = new Vector2Int(x, y);
                Vector3 position = transform.position + new Vector3(x * cellSize, 0f, y * cellSize);

                Gizmos.color = GetNodeColor(node);
                Gizmos.DrawCube(position, Vector3.one * (cellSize * 0.8f));

                Gizmos.color = Color.black;
                Gizmos.DrawWireCube(position, Vector3.one * (cellSize * 0.8f));
            }
        }
    }

    private Color GetNodeColor(Vector2Int node)
    {
        // 현재 처리 중인 노드
        if (Application.isPlaying && node == currentNode)
        {
            return Color.yellow;
        }

        // 이미 지나간(처리 완료된) 노드
        if (Application.isPlaying && processed.Contains(node))
        {
            return Color.magenta;
        }

        // 발견되었지만 아직 처리되지 않은 노드
        if (Application.isPlaying && visited.Contains(node))
        {
            return Color.cyan;
        }

        if (IsWall(node))
        {
            return Color.black;
        }

        // 미방문 노드
        return Color.gray;
    }
}