using UnityEngine;
using System.Collections.Generic;

public class ShadowNode
{
    public Vector3Int gridPosition;
    public Vector3Int fromDirection;
    public int maxDistance;
    public float walkCost;
    public float alpha;
    public int numberOfCorners;
    public bool isUnfolded = false;

    public ShadowNode(
        Vector3Int _previousPosition,
        Vector3Int _previousDirection,
        float _previousWalkCost,
        int _previousNumberOfCorners,
        Vector3Int _direction,
        int _maxDistance,
        float penalty
    ) {
        this.gridPosition = _previousPosition + _direction;
        this.fromDirection = _direction;
        this.maxDistance = _maxDistance;
        this.numberOfCorners = _previousNumberOfCorners +
            (_previousDirection == Vector3Int.zero || _previousDirection == _direction ? 0 : 1);
        
        if (_previousDirection == Vector3Int.zero && _previousDirection == _direction)
        {
            this.walkCost = 0;
        }
        else if (_previousDirection == Vector3Int.zero || _previousDirection == _direction)
        {
            this.walkCost = _previousWalkCost + 1;
        }
        else if (numberOfCorners >= 1)
        {
            this.walkCost = _previousWalkCost + penalty;
        }
        this.alpha = EaseOut(this.walkCost / this.maxDistance);
    }

    private float EaseOut(float value)
    {
        var ease = value * value * value;
        return Mathf.Clamp(value, 0f, 1f);

    }

    public List<ShadowNode> UnfoldNode(float penalty)
    {
        this.isUnfolded = true;

        var nodes = new List<ShadowNode>()
        {
            new ShadowNode(this.gridPosition, this.fromDirection, this.walkCost, this.numberOfCorners, Vector3Int.up, maxDistance, penalty),
            new ShadowNode(this.gridPosition, this.fromDirection, this.walkCost, this.numberOfCorners, Vector3Int.down, maxDistance, penalty),
            new ShadowNode(this.gridPosition, this.fromDirection, this.walkCost, this.numberOfCorners, Vector3Int.left, maxDistance, penalty),
            new ShadowNode(this.gridPosition, this.fromDirection, this.walkCost, this.numberOfCorners, Vector3Int.right, maxDistance, penalty)
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