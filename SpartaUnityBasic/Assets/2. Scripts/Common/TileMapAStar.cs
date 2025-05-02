using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class TileMapAStar
{
    public class Node
    {
        public Vector2Int Position;
        public Node Parent;
        public int G;
        public int H;
        public int F => G + H; // 최종 비용

        public Node(Vector2Int pos, Node parnet, int g, int h)
        {
            Position = pos;
            Parent = parnet;
            G = g;
            H = h;
        }
    }

    private static HashSet<Vector2Int> walls = new HashSet<Vector2Int>();

    public static void SetWallData(Tilemap wallsMap)
    {
        walls.Clear();
        foreach (var pos in wallsMap.cellBounds.allPositionsWithin)
        {
            if (wallsMap.HasTile(pos))
                walls.Add(new Vector2Int(pos.x, pos.y));
        }
    }

    public static List<Vector2Int> FindPath(Vector2Int start, Vector2Int goal)
    {
        List<Node>          openList  = new List<Node>();          // 탐색 후보 노드
        HashSet<Vector2Int> closedSet = new HashSet<Vector2Int>(); //이미 탐색한 노드

        Node startNode = new Node(start, null, 0, GetHeuristic(start, goal));
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            // F값이 가장 낮은 노드를 찾기
            Node currentNode = GetLowestFNode(openList);

            if (currentNode.Position == goal)
                return ReconstructPath(currentNode);

            openList.Remove(currentNode);
            closedSet.Add(currentNode.Position);

            foreach (var dir in directions)
            {
                Vector2Int neighborPos = currentNode.Position + dir;

                if (IsDiagonalMoveBlocked(currentNode.Position, neighborPos))
                    continue;
                if (walls.Contains(neighborPos) || closedSet.Contains(neighborPos))
                    continue;
                int gCost = currentNode.G + 1;
                int hCost = GetHeuristic(neighborPos, goal);

                Node existingNode = openList.Find(n => n.Position == neighborPos);
                if (existingNode == null)
                {
                    openList.Add(new Node(neighborPos, currentNode, gCost, hCost));
                }
                else if (gCost < existingNode.G)
                {
                    existingNode.G = gCost;
                    existingNode.Parent = currentNode;
                }
            }
        }

        return null; // 경로 없음
    }

    private static Node GetLowestFNode(List<Node> nodeList)
    {
        Node best = nodeList[0];
        foreach (var node in nodeList)
        {
            if (node.F < best.F)
                best = node;
        }

        return best;
    }

    private static int GetHeuristic(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y); // Manhattan 거리
    }

    private static List<Vector2Int> ReconstructPath(Node node)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        while (node != null)
        {
            path.Add(node.Position);
            node = node.Parent;
        }

        path.Reverse();
        return path;
    }

    static bool IsDiagonalMoveBlocked(Vector2Int current, Vector2Int next)
    {
        Vector2Int delta = next - current;

        // 대각선 이동일 경우
        if (Mathf.Abs(delta.x) == 1 && Mathf.Abs(delta.y) == 1)
        {
            Vector2Int sideA = new Vector2Int(current.x + delta.x, current.y);
            Vector2Int sideB = new Vector2Int(current.x, current.y + delta.y);


            if (walls.Contains(sideA) || walls.Contains(sideB))
                return true;
        }

        return false;
    }

    private static readonly Vector2Int[] directions = new Vector2Int[]
    {
        Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right,
        new Vector2Int(-1, 1), new Vector2Int(1, 1),
        new Vector2Int(1, -1), new Vector2Int(-1, -1)
    };
}