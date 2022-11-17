using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPathFinding : MonoBehaviour
{

    public List<Vector3Int> positions;
    public bool isCalculating = false;
    public float repeatPerSecond = 0f;

    private List<GridPathNode> nodes = new List<GridPathNode>();
    private List<GridPathNode> expandedNodes = new List<GridPathNode>();

    public void StartPathFinding(Vector3Int fromCell, Vector3Int toCell)
    {
        if (!isCalculating)
        {
            isCalculating = true;
            StartCoroutine(FindPath(fromCell, toCell));
        }
    }

    private IEnumerator FindPath(Vector3Int fromCell, Vector3Int toCell)
    {
        GameLogic.Instance.GridTerrainManager.ClearNodes();
        positions = new List<Vector3Int>();
        Vector3Int targetDirection = (toCell - fromCell);

        nodes = new List<GridPathNode>();
        expandedNodes = new List<GridPathNode>();
        int hCost = Mathf.Abs(targetDirection.x) + Mathf.Abs(targetDirection.y);
        GridPathNode currentNode = new GridPathNode(null, fromCell, 0, hCost, hCost);
        float paintInterval = 1f / repeatPerSecond;
        float time = 0f;
        while (currentNode.hCost > 1)
        {
            ExpandNode(currentNode, toCell);
            nodes.Sort((x, y) => GridPathNode.RankForSort(x, y));
            nodes.Remove(currentNode);
            expandedNodes.Add(currentNode);
            if (nodes.Count == 0)
            {
                positions = null;
                yield break;
            }
            currentNode = nodes[0];
            time += Time.deltaTime;
            if (time >= paintInterval)
            {
                GameLogic.Instance.PaintPath(currentNode);
                time %= paintInterval;
            }
            yield return null;
        }

        positions.Add(toCell);
        while (currentNode.previous != null)
        {
            positions.Add(currentNode.position);
            currentNode = currentNode.previous;
            yield return null;
        }
        positions.Add(fromCell);
        positions.Reverse();

        isCalculating = false;
    }

    private void ExpandNode(GridPathNode currentNode, Vector3Int to)
    {
        CheckNeighbour(currentNode, Vector3Int.up, to);
        CheckNeighbour(currentNode, Vector3Int.down, to);
        CheckNeighbour(currentNode, Vector3Int.left, to);
        CheckNeighbour(currentNode, Vector3Int.right, to);
    }

    private void CheckNeighbour(GridPathNode currentNode, Vector3Int direction, Vector3Int to)
    {
        Vector3Int neighbourAt = currentNode.position + direction;
        if (!GameLogic.Instance.IsWalkableTile(neighbourAt, true))
        {
            return;
        }
        Vector3Int targetDirection = to - neighbourAt;
        int hCost = Mathf.Abs(targetDirection.x) + Mathf.Abs(targetDirection.y);
        int gCost = currentNode.gCost + 1;
        int fCost = hCost + gCost;
        GridPathNode neighbourNode = new GridPathNode(currentNode, neighbourAt, gCost, fCost, hCost);
        GridPathNode other = nodes.Find(node => node.position == neighbourAt);
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
