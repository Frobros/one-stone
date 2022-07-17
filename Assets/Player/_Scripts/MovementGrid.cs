using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using System;

public class PathNode
{
    public PathNode previous;
    public Vector3Int position;
    public int gCost;
    public int fCost;
    public int hCost;

    public PathNode(PathNode _previous, Vector3Int _position, int _gCost, int _fCost, int _hCost)
    {
        previous = _previous;
        position = _position;
        gCost = _gCost;
        fCost = _fCost;
        hCost = _hCost;
    }
}

public class MovementGrid : MonoBehaviour
{
    public TileBase tile;
    private Tilemap tilemap;
    public bool isEnemy;
    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }

    public void OnShowGrid(Vector3 position, int radius, bool isEnemy)
    {
        tilemap.gameObject.SetActive(true);
        tilemap.ClearAllTiles();
        Vector3Int playerPositionInGrid = tilemap.WorldToCell(position);
        HashSet<Vector3Int> pointSet = new HashSet<Vector3Int>();
        pointSet.Add(playerPositionInGrid);

        while (radius != 0)
        {
            List<Vector3Int> temp = new List<Vector3Int>(pointSet);
            foreach (Vector3Int point in temp)
            {
                Vector3Int pointRight = new Vector3Int(point.x + 1, point.y, point.z);
                Vector3Int pointLeft = new Vector3Int(point.x - 1, point.y, point.z);
                Vector3Int pointUp = new Vector3Int(point.x, point.y + 1, point.z);
                Vector3Int pointDown = new Vector3Int(point.x, point.y - 1, point.z);

                if (LevelManager.Instance.IsWalkableTile(pointUp, isEnemy))
                {
                    pointSet.Add(pointUp);
                }
                if (LevelManager.Instance.IsWalkableTile(pointLeft, isEnemy))
                {
                    pointSet.Add(pointLeft);
                }
                if (LevelManager.Instance.IsWalkableTile(pointDown, isEnemy))
                {
                    pointSet.Add(pointDown);
                }
                if (LevelManager.Instance.IsWalkableTile(pointRight, isEnemy))
                {
                    pointSet.Add(pointRight);
                }
            }

            radius--;
        }

        foreach (Vector3Int point in pointSet)
        {
            tilemap.SetTile(point, tile);
        }
    }

    internal void HideGrid()
    {
        tilemap.gameObject.SetActive(false);
    }

    internal List<Vector3Int> GetMovementStepsToPlayer(Vector3 position)
    {
        Vector3 from = position;
        Vector3 to = FindObjectOfType<PlayerLink>().transform.position;
        List<Vector3Int> steps = FindPath(tilemap.WorldToCell(from), tilemap.WorldToCell(to));
        return steps;
    }

    private List<Vector3Int> FindPath(Vector3Int from, Vector3Int to)
    {
        List<PathNode> nodes = new List<PathNode> ();
        Vector3Int targetDirection = (to - from);
        int hCost = Mathf.Abs(targetDirection.x) + Mathf.Abs(targetDirection.y);
        PathNode currentNode = new PathNode(null, from, 0, hCost, hCost);
        while(currentNode.hCost > 1)
        {
            ExpandNode(currentNode, nodes, to);
            nodes.Sort((n1, n2) =>
                n1.fCost < n2.fCost ? -1
                : n1.fCost == n2.fCost ? 0
                : 1
            );
            nodes.Remove(currentNode);
            if (nodes.Count == 0) return null;
            currentNode = nodes[0];
        }

        List <Vector3Int> points = new List<Vector3Int>();
        points.Add(to);
        while (currentNode.previous != null)
        {
            points.Add(currentNode.position);
            currentNode = currentNode.previous;
        }
        points.Add(from);
        points.Reverse();
        return points;
    }

    private void ExpandNode(PathNode currentNode, List<PathNode> nodes, Vector3Int to)
    {
        LookAtNeighbourAt(Vector3Int.up, currentNode, nodes, to);
        LookAtNeighbourAt(Vector3Int.down, currentNode, nodes, to);
        LookAtNeighbourAt(Vector3Int.left, currentNode, nodes, to);
        LookAtNeighbourAt(Vector3Int.right, currentNode, nodes, to);
    }

    private void LookAtNeighbourAt(Vector3Int direction, PathNode currentNode, List<PathNode> nodes, Vector3Int to)
    {
        Vector3Int neighbourAt = currentNode.position + direction;
        if (!LevelManager.Instance.IsWalkableTile(neighbourAt, isEnemy))
        {
            return;
        }
        Vector3Int targetDirection = to - neighbourAt;
        int hCost = Mathf.Abs(targetDirection.x) + Mathf.Abs(targetDirection.y);
        int gCost = currentNode.gCost + 1;
        int fCost = hCost + gCost;
        PathNode neighbourNode = new PathNode(currentNode, neighbourAt, gCost, fCost, hCost);
        PathNode other = nodes.Find(node => node.position == neighbourAt);
        if (other == null)
        {
            nodes.Add(neighbourNode);
        }
        else if (other.fCost > neighbourNode.fCost)
        {
            nodes.Remove(other);
            nodes.Add(neighbourNode);
        }
    }

    public Vector3Int GetGridCelll(Vector3 position)
    {
        return tilemap.WorldToCell(position);
    }


    public Vector3 GetGridCenterPosition(Vector3Int position)
    {
        return tilemap.CellToWorld(position) + 0.5f * tilemap.cellSize;
    }

    internal bool IsWalkable(Vector3Int tileCell)
    {
        return tilemap.GetTile(tileCell) != null;
    }
}
