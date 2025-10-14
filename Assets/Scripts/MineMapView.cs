using System;
using System.Collections.Generic;
using System.Linq;
using NodeGenerator;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MineMapView : MonoBehaviour
{
    [Serializable]
    private struct TileMapSettings
    {
        public Tilemap TargetTilemap;
        public Tile[] Tiles;

        public void SetTile(Vector2Int position, int type)
        {
            TargetTilemap.SetTile((Vector3Int) position, Tiles[type]);
        }
    }

    [Serializable]
    private struct BiomeTileSettings
    {
        public Tilemap Tilemap;
        public RuleTile[] RuleTile;

        public RuleTile GetTile(int idx) => RuleTile[idx];
    }

    [SerializeField] private TileMapSettings _nodeTileMap;
    [SerializeField] private BiomeTileSettings _biomeTileSettings;
    [SerializeField] private PathDrawer _pathDrawer;

    public void SetupBiomes(int[,] biomes)
    {
        var tileMap = _biomeTileSettings.Tilemap;
        var maxWidth = biomes.GetLength(0);
        var maxDepth = biomes.GetLength(1);
        
        
        for (var width = 0; width < maxWidth; width++)
        {
            for (var depth = 0; depth < maxDepth; depth++)
            {
                var position = new Vector3Int(width, depth);
                var tile = _biomeTileSettings.GetTile(biomes[width, depth]);
                tileMap.SetTile(position, tile);
            }
        }
    }

    public void SetupNodes(List<Node> nodes)
    {
        foreach (var node in nodes)
        {
            var connections = node.Connections;
            SetNode(node);
            foreach (var connection in connections)
            {
                _pathDrawer.DrawPath(connection.Path);
            }
        }

        foreach (var node in nodes)
        {
            _pathDrawer.DrawTile(node.Position, node.Connections.Select(c => c.Path[1]));
        }
    }

    private void SetNode(Node node)
    {
        _nodeTileMap.SetTile(node.Position, 0);
    }
}