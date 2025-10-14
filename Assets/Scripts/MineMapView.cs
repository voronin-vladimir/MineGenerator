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

    [SerializeField] private TileMapSettings _nodeTileMap;
    //[SerializeField] private TileMapSettings _biomeTileMap;
    [SerializeField] private PathDrawer _pathDrawer;

    public void SetupBiomes(int[,] biomes)
    {
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