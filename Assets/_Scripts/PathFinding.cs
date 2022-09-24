using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

public class PathFinding : MonoBehaviour
{
    private static PathFinding _instance;
    public static PathFinding Instance { get { return _instance; } }

    public List<Vector3Int> positions;
    public bool isCalculating = false;
    List<PathNode> nodes = new List<PathNode>();
    List<PathNode> expandedNodes = new List<PathNode>();

    private void Awake()
    {
        _instance = this;
    }

    public void StartPathFinding(Vector3Int from, Vector3Int to)
    {
        if (!isCalculating)
        {
            isCalculating = true;
            StartCoroutine(FindPath(from, to));
        }
    }

    private IEnumerator FindPath(Vector3Int from, Vector3Int to)
    {
        LevelManager.Instance.ClearNodes();
        positions = new List<Vector3Int>();
        Vector3Int targetDirection = (to - from);

        nodes = new List<PathNode>();
        expandedNodes = new List<PathNode>();
        int hCost = Mathf.Abs(targetDirection.x) + Mathf.Abs(targetDirection.y);
        PathNode currentNode = new PathNode(null, from, 0, hCost, hCost);
        while (currentNode.hCost > 1)
        {
            // LevelManager.Instance.PaintNode(currentNode.position);
            LevelManager.Instance.PaintPath(currentNode);
            ExpandNode(currentNode, to);
            nodes.Sort((n1, n2) =>
                n1.fCost < n2.fCost ? -1
                : n1.fCost > n2.fCost ? 1
                : 0
            );
            nodes.Remove(currentNode);
            expandedNodes.Add(currentNode);
            if (nodes.Count == 0)
            {
                positions = null;
                yield break;
            }
            currentNode = nodes[0];
            yield return new WaitForSeconds(0.2f);
        }

        positions.Add(to);
        while (currentNode.previous != null)
        {
            positions.Add(currentNode.position);
            currentNode = currentNode.previous;
            yield return null;
        }
        positions.Add(from);
        positions.Reverse();

        isCalculating = false;
    }

    private void ExpandNode(PathNode currentNode, Vector3Int to)
    {
        LookAtNeighbourAt(Vector3Int.up, currentNode, to);
        LookAtNeighbourAt(Vector3Int.down, currentNode, to);
        LookAtNeighbourAt(Vector3Int.left, currentNode, to);
        LookAtNeighbourAt(Vector3Int.right, currentNode, to);
    }

    private void LookAtNeighbourAt(Vector3Int direction, PathNode currentNode, Vector3Int to)
    {
        Vector3Int neighbourAt = currentNode.position + direction;
        if (!LevelManager.Instance.IsWalkableTile(neighbourAt, true))
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
            other = expandedNodes.Find(node => node.position == neighbourAt);
        }

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
}
