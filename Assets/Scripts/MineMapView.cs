using System;
using System.Collections.Generic;
using NodeGenerator;
using TileMap;
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
    private struct RuleTileMapSettings
    {
        public Tilemap TargetTilemap;
        public PathTile Tile;

        public void SetTile(Vector2Int position, int roadId)
        {
            //Tile.SetPathId(roadId);
            //TargetTilemap.SetTile((Vector3Int) position, Tile);
        }

        public void SetTiles(List<Vector2Int> tiles, bool ignoreLockFlags = false)
        {
            var tileChangeData = new List<TileChangeData>();

            // foreach (var tile in tiles)
            // {
            //     tileChangeData.Add(new TileChangeData(
            //         (Vector3Int) tile,
            //         Tile,
            //         Color.white,
            //         Matrix4x4.identity));
            // }

            TargetTilemap.SetTiles(tileChangeData.ToArray(), ignoreLockFlags);
        }
    }

    [SerializeField] private TileMapSettings _nodeTileMap;
    //[SerializeField] private TileMapSettings _biomeTileMap;
    [SerializeField] private RuleTileMapSettings _roadTileMap;

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
                _roadTileMap.SetTiles(connection.Path);
            }
        }
    }

    private void SetNode(Node node)
    {
        _nodeTileMap.SetTile(node.Position, 0);
    }
}
