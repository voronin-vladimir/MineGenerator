using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TileMap
{
    [CreateAssetMenu(menuName = "Tiles/PathTile")]
    public class PathTile : TileBase
    {
        public Sprite[] sprites = new Sprite[16];

        public static Dictionary<Vector3Int, int> PathIds = new Dictionary<Vector3Int, int>();

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            // Берём pathId текущей клетки
            int currentId = GetPathId(position);
            if (currentId == -1)
            {
                tileData.sprite = null;
                return;
            }

            // Смотрим соседей: Up=1, Right=2, Down=4, Left=8
            bool up    = GetPathId(position + Vector3Int.up)    == currentId;
            bool right = GetPathId(position + Vector3Int.right) == currentId;
            bool down  = GetPathId(position + Vector3Int.down)  == currentId;
            bool left  = GetPathId(position + Vector3Int.left)  == currentId;

            int mask = 0;
            if (up)    mask |= 1;
            if (right) mask |= 2;
            if (down)  mask |= 4;
            if (left)  mask |= 8;

            if (sprites != null && mask >= 0 && mask < sprites.Length && sprites[mask] != null)
                tileData.sprite = sprites[mask];
            else
                tileData.sprite = sprites[0]; // спрайт по умолчанию

            tileData.colliderType = Tile.ColliderType.None;
        }

        public override void RefreshTile(Vector3Int location, ITilemap tilemap)
        {
            // Перерисовать текущую клетку и её соседей
            tilemap.RefreshTile(location);
            tilemap.RefreshTile(location + Vector3Int.up);
            tilemap.RefreshTile(location + Vector3Int.right);
            tilemap.RefreshTile(location + Vector3Int.down);
            tilemap.RefreshTile(location + Vector3Int.left);
        }

        private int GetPathId(Vector3Int pos)
        {
            return PathIds.TryGetValue(pos, out int id) ? id : -1;
        }
    }
}