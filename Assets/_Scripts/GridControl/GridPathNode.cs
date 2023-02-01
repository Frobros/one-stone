using UnityEngine;

public class GridPathNode
{
    public GridPathNode previous;
    public Vector3Int position;
    public int gCost;
    public int fCost;
    public int hCost;

    public GridPathNode(GridPathNode _previous, Vector3Int _position, int _gCost, int _fCost, int _hCost)
    {
        previous = _previous;
        position = _position;
        gCost = _gCost;
        fCost = _fCost;
        hCost = _hCost;
    }

    public static int RankForSort(GridPathNode x, GridPathNode y)
    {
        return x.fCost < y.fCost ? -1
                : x.fCost > y.fCost ? 1
                : 0;
    }
}
