using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class GridShadowController : MonoBehaviour
{
    public Tilemap shadowTilemap;
    public TileBase shadowTile;
    private BoundsInt levelBounds;
    public int radius;
    public float PENALTY;
    public List<ShadowNode> currentShadowNodes = new List<ShadowNode>();

    public void Init(BoundsInt _levelBounds)
    {
        levelBounds = _levelBounds;
        for (int i = levelBounds.xMin; i < levelBounds.xMax; i++)
        {
            for (int j = levelBounds.yMin; j < levelBounds.yMax; j++)
            {
                Vector3Int cellPosition = new Vector3Int(i, j, 0);
                // Flag the tile, inidicating that it can change colour.
                // By default it's set to "Lock Colour".
                shadowTilemap.SetTile(cellPosition, shadowTile);
                shadowTilemap.SetTileFlags(cellPosition, TileFlags.None);
                shadowTilemap.SetColor(cellPosition, Color.black);
            }
        }
        Vector3 playerPos = FindObjectOfType<PlayerLink>().transform.position;
        ApplyLight(playerPos);
    }


    public void ApplyLight(Vector3 playerPosition)
    {
        var currentPlayerPosition = shadowTilemap.WorldToCell(playerPosition);

        // find new shadow nodes
        var nextShadowNodes = new List<ShadowNode>();
        nextShadowNodes.Add(new ShadowNode(
            currentPlayerPosition, 
            Vector3Int.zero,
            0,
            0,
            Vector3Int.zero,
            radius,
            PENALTY
        ));
        var currentNode = nextShadowNodes[0];
        while (currentNode.walkCost < radius && !currentNode.isUnfolded)
        {
            var shadowNodeCandidates = nextShadowNodes[0].UnfoldNode(PENALTY);
            shadowNodeCandidates.RemoveAll(x => {
                return !GridTerrainManager.Instance.IsWalkableTile(x.gridPosition)
                    || nextShadowNodes.Exists(y => x.HasSamePosition(y) && x.walkCost >= y.walkCost);
            });
            nextShadowNodes.AddRange(shadowNodeCandidates);
            nextShadowNodes.Sort((x, y) => x.CompareWalkedDistance(y));
            currentNode = nextShadowNodes[0];
        }
        nextShadowNodes.RemoveAll(x => x.walkCost > radius);

        // sets alpha for new shadow nodes
        Color color;
        nextShadowNodes.ForEach(x =>
        {
            color = new Color(0, 0, 0, x.alpha);
            this.shadowTilemap.SetColor(x.gridPosition, color);
        });

        // paints old shadow nodes black
        this.currentShadowNodes
            .Where(x => !nextShadowNodes.Exists(y => x.gridPosition == y.gridPosition)).ToList()
            .ForEach(x => this.shadowTilemap.SetColor(x.gridPosition, Color.black));

        this.currentShadowNodes = nextShadowNodes;
    }

    public float GetAlphaAt(Vector3Int _gridPosition)
    {
        foreach (var shadowNode in this.currentShadowNodes)
        {
            if (shadowNode.gridPosition == _gridPosition)
            {
                return shadowNode.alpha;
            }
        }
        return 1f;
    }
}
