using UnityEngine;
using System.Collections.Generic;

public class ShadowNode
{
    public Vector3Int gridPosition;
    public Vector3Int fromDirection;
    public int maxDistance;
    public int walkCost;
    public float alpha;
    public bool isUnfolded = false;
    private static int PENALTY = 2;

    public ShadowNode(Vector3Int _previousPosition, Vector3Int _previousDirection, int _previousWalkCost, Vector3Int _direction, int _maxDistance)
    {
        this.gridPosition = _previousPosition + _direction;
        this.fromDirection = _direction;
        this.maxDistance = _maxDistance;
        if (_previousDirection == Vector3Int.zero && _previousDirection == _direction)
        {
            this.walkCost = 0;
        }
        else if (_previousDirection == Vector3Int.zero || _previousDirection == _direction)
        {
            this.walkCost = _previousWalkCost + 1;
        }
        else
        {
            this.walkCost = _previousWalkCost + PENALTY;
        }
        this.alpha = (float)this.walkCost / this.maxDistance;
    }

    public List<ShadowNode> UnfoldNode()
    {
        this.isUnfolded = true;

        var nodes = new List<ShadowNode>()
        {
            new ShadowNode(this.gridPosition, this.fromDirection, this.walkCost, Vector3Int.up, maxDistance),
            new ShadowNode(this.gridPosition, this.fromDirection, this.walkCost, Vector3Int.down, maxDistance),
            new ShadowNode(this.gridPosition, this.fromDirection, this.walkCost, Vector3Int.left, maxDistance),
            new ShadowNode(this.gridPosition, this.fromDirection, this.walkCost, Vector3Int.right, maxDistance)
        };
        return nodes;
    }

    public int CompareWalkedDistance(ShadowNode other)
    {
        if (this.isUnfolded && other.isUnfolded)
        {
            return 0;
        }
        else if (!this.isUnfolded && other.isUnfolded)
        {
            return -1;
        }
        else if (this.isUnfolded && !other.isUnfolded)
        {
            return 1;
        }
        else if (this.walkCost < other.walkCost)
        {
            return -1;
        }
        else if (this.walkCost > other.walkCost)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    public bool HasSamePosition(ShadowNode other)
    {
        return this.gridPosition == other.gridPosition;
    }
}