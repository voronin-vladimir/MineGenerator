using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public static class CellsExtensions
    {
        public static IEnumerable<Vector2Int> GetNeighbours(Vector2Int position, int width, int height)
        {
            var neighbours = new List<Vector2Int>(4);
            var posX = position.x;
            var posY = position.y;

            //Left
            if (position.x > 0)
            {
                neighbours.Add(new Vector2Int(posX - 1, posY));
            }

            //Right
            if (position.x < width - 1)
            {
                neighbours.Add(new Vector2Int(posX + 1, posY));
            }

            //Bottom
            if (position.y > 0)
            {
                neighbours.Add(new Vector2Int(posX, posY - 1));
            }

            //Top
            if (position.y < height - 1)
            {
                neighbours.Add(new Vector2Int(posX, posY + 1));
            }

            return neighbours;
        }

        public static IEnumerable<Vector2Int> GetNeighbours(Vector2Int position,
            Vector2Int leftBottomCorner, Vector2Int rightTopCorner)
        {
            var neighbours = new List<Vector2Int>(4);
            var posX = position.x;
            var posY = position.y;

            //Left
            if (position.x > leftBottomCorner.x)
            {
                neighbours.Add(new Vector2Int(posX - 1, posY));
            }

            //Right
            if (position.x < rightTopCorner.x - 1)
            {
                neighbours.Add(new Vector2Int(posX + 1, posY));
            }

            //Bottom
            if (position.y > leftBottomCorner.y)
            {
                neighbours.Add(new Vector2Int(posX, posY - 1));
            }

            //Top
            if (position.y < rightTopCorner.y - 1)
            {
                neighbours.Add(new Vector2Int(posX, posY + 1));
            }

            return neighbours;
        }
    }
}