using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class PathDrawer
{
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private Sprite _straight;
    [SerializeField] private Sprite _corner;
    [SerializeField] private Sprite _end;
    [SerializeField] private Sprite _cross;
    [SerializeField] private Sprite _tee;

    [System.Flags]
    public enum PathDirection
    {
        None = 0,
        Up = 1 << 0,
        Right = 1 << 1,
        Down = 1 << 2,
        Left = 1 << 3
    }

    public void DrawTile(Vector2Int tilePosition, IEnumerable<Vector2Int> connections)
    {
        var pos = new Vector3Int(tilePosition.x, tilePosition.y, 0);
        var mask = PathDirection.None;

        foreach (var connection in connections)
            mask |= ToDir(tilePosition - connection);

        var (sprite, rotation) = GetSpriteAndRotation(mask);
        PlaceTile(pos, sprite, rotation);
    }

    public void DrawPath(List<Vector2Int> path)
    {
        for (var i = 0; i < path.Count; i++)
        {
            var pos = new Vector3Int(path[i].x, path[i].y, 0);

            var prevDir = i > 0 ? path[i] - path[i - 1] : (Vector2Int?) null;
            var nextDir = i < path.Count - 1 ? path[i + 1] - path[i] : (Vector2Int?) null;

            var (sprite, rotation) = GetSpriteAndRotation(prevDir, nextDir);
            PlaceTile(pos, sprite, rotation);
        }
    }

    private (Sprite, Quaternion) GetSpriteAndRotation(Vector2Int? prevDir, Vector2Int? nextDir)
    {
        var mask = PathDirection.None;

        if (prevDir.HasValue)
            mask |= ToDir(prevDir.Value);
        if (nextDir.HasValue)
            mask |= ToDir(-nextDir.Value);

        return GetSpriteAndRotation(mask);
    }

    private (Sprite, Quaternion) GetSpriteAndRotation(PathDirection mask)
    {
        return GetTileVisual(mask);
    }

    private (Sprite, Quaternion) GetTileVisual(PathDirection mask)
    {
        switch (mask)
        {
            // Конец
            case PathDirection.Up:
            case PathDirection.Right:
            case PathDirection.Down:
            case PathDirection.Left:
                return (_end, GetRotation(mask));

            // Прямые
            case PathDirection.Up | PathDirection.Down:
                return (_straight, Quaternion.identity);
            case PathDirection.Left | PathDirection.Right:
                return (_straight, Quaternion.Euler(0, 0, 90));

            // Углы
            case PathDirection.Up | PathDirection.Right:
                return (_corner, Quaternion.identity);
            case PathDirection.Right | PathDirection.Down:
                return (_corner, Quaternion.Euler(0, 0, 270));
            case PathDirection.Down | PathDirection.Left:
                return (_corner, Quaternion.Euler(0, 0, 180));
            case PathDirection.Left | PathDirection.Up:
                return (_corner, Quaternion.Euler(0, 0, 90));

            // T-образные
            case PathDirection.Up | PathDirection.Left | PathDirection.Right:
                return (_tee, Quaternion.identity);
            case PathDirection.Right | PathDirection.Up | PathDirection.Down:
                return (_tee, Quaternion.Euler(0, 0, 270));
            case PathDirection.Down | PathDirection.Left | PathDirection.Right:
                return (_tee, Quaternion.Euler(0, 0, 180));
            case PathDirection.Left | PathDirection.Up | PathDirection.Down:
                return (_tee, Quaternion.Euler(0, 0, 90));

            // Перекрёсток
            case PathDirection.Up | PathDirection.Right | PathDirection.Down | PathDirection.Left:
                return (_cross, Quaternion.identity);
        }

        return (_end, Quaternion.identity);
    }

    private PathDirection ToDir(Vector2Int dir)
    {
        if (dir == Vector2Int.up)
            return PathDirection.Up;
        if (dir == Vector2Int.right)
            return PathDirection.Right;
        if (dir == Vector2Int.down)
            return PathDirection.Down;
        if (dir == Vector2Int.left)
            return PathDirection.Left;

        return PathDirection.None;
    }

    private Quaternion GetRotation(PathDirection dir)
    {
        return dir switch
        {
            PathDirection.Up => Quaternion.identity,
            PathDirection.Right => Quaternion.Euler(0, 0, 270),
            PathDirection.Down => Quaternion.Euler(0, 0, 180),
            PathDirection.Left => Quaternion.Euler(0, 0, 90),
            _ => Quaternion.identity
        };
    }

    private void PlaceTile(Vector3Int pos, Sprite sprite, Quaternion rotation)
    {
        var tile = ScriptableObject.CreateInstance<Tile>();
        tile.sprite = sprite;
        _tilemap.SetTile(pos, tile);
        _tilemap.SetTransformMatrix(pos, Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one));
    }
}
