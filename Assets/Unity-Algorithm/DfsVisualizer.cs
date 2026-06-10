using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework.Internal.Commands;
using UnityEngine;
using UnityEngine.InputSystem;

public class DfsGizmoVisualizer : MonoBehaviour
{
    [Header("Grid")]
    [Tooltip("가로 칸 수입니다.")]
    [SerializeField] private int width = 3;

    [Tooltip("세로 칸 수입니다.")]
    [SerializeField] private int height = 3;

    [Tooltip("Scene 뷰에 그릴 칸 크기입니다.")]
    [SerializeField] private float cellSize = 1f;

    [Header("Path")]
    [Tooltip("경로 탐색을 시작할 칸입니다.")]
    [SerializeField] private Vector2Int start = new Vector2Int(0, 0);

    [Tooltip("경로 탐색의 목표 칸입니다.")]
    [SerializeField] private Vector2Int goal = new Vector2Int(5, 3);

    [Tooltip("이동할 수 없는 벽 칸 목록입니다.")]
    [SerializeField]
    private Vector2Int[] walls =
    {
        new Vector2Int(2, 0),
        new Vector2Int(2, 1),
        new Vector2Int(2, 2)
    };

    private readonly Stack<Vector2Int> frontier = new Stack<Vector2Int>();
    private readonly HashSet<Vector2Int> visited = new HashSet<Vector2Int>();
    private readonly HashSet<Vector2Int> processed = new HashSet<Vector2Int>();
    private Vector2Int currentNode;

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
        frontier.Push(currentNode);
        visited.Add(currentNode);
    }

    private void StepSearch()
    {
        if (frontier.Count == 0)
        {
            return;
        }

        currentNode = frontier.Pop();

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

            frontier.Push(neighbor);
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
            Vector2Int.left
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

    private bool IsWall(Vector2Int node)
    {
        foreach (Vector2Int wall in walls)
        {
            if (wall == node)
            {
                return true;
            }
        }

        return false;
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
        if (Application.isPlaying && node == currentNode)
        {
            return Color.yellow;
        }

        if (Application.isPlaying && processed.Contains(node))
        {
            return Color.magenta;
        }

        if (Application.isPlaying && visited.Contains(node))
        {
            return Color.cyan;
        }        

        if (IsWall(node))
        {
            return Color.black;
        }

        return Color.gray;
    }
}