using System.Collections.Generic;
using System.Linq;

namespace MazeGenerator
{
    public class MazeGenerator
    {
        private MazeCell[,] _mazeGrid;
        private int _width;
        private int _depth;
    
        public MazeCell[,] Generate(int width, int depth)
        {
            _mazeGrid = new MazeCell[width, depth];
            _width = width;
            _depth = depth;
        
            var cells = new Stack<MazeCell>();

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < depth; y++)
                {
                    _mazeGrid[x, y] = new MazeCell(x, y);
                }
            }

            var initialCell = _mazeGrid[0, 0];
            initialCell.Visit();
            _mazeGrid[0, 0] = initialCell;

            cells.Push(initialCell);

            var rnd = new System.Random();

            while (cells.Any())
            {
                var currentCell = cells.Pop();
                var neighbours = GetNeighbours(currentCell).Where(n => !n.IsVisited).ToArray();

                if (!neighbours.Any())
                {
                    continue;
                }

                var randomNeighbourCellIdx = rnd.Next(neighbours.Length);
                var randomNeighbourCell = neighbours[randomNeighbourCellIdx];
                ConnectTwoCells(ref currentCell, ref randomNeighbourCell);
                randomNeighbourCell.Visit();

                cells.Push(currentCell);
                cells.Push(randomNeighbourCell);

                _mazeGrid[currentCell.X, currentCell.Y] = currentCell;
                _mazeGrid[randomNeighbourCell.X, randomNeighbourCell.Y] = randomNeighbourCell;
            }

            return _mazeGrid;
        }

        private IEnumerable<MazeCell> GetNeighbours(MazeCell targetCell)
        {
            var neighbours = new List<MazeCell>(4);
            var posX = targetCell.X;
            var posY = targetCell.Y;

            //Left
            if (targetCell.X > 0)
            {
                neighbours.Add(_mazeGrid[posX - 1, posY]);
            }

            //Right
            if (targetCell.X < _width - 1)
            {
                neighbours.Add(_mazeGrid[posX + 1, posY]);
            }

            //Bottom
            if (targetCell.Y > 0)
            {
                neighbours.Add(_mazeGrid[posX, posY - 1]);
            }

            //Top
            if (targetCell.Y < _depth - 1)
            {
                neighbours.Add(_mazeGrid[posX, posY + 1]);
            }

            return neighbours;
        }

        private static void ConnectTwoCells(ref MazeCell firstCell, ref MazeCell secondCell)
        {
            var isVerticalWallToRemove = firstCell.Y != secondCell.Y;

            if (isVerticalWallToRemove)
            {
                if (firstCell.Y > secondCell.Y)
                {
                    firstCell.RemoveWall(WallPosition.Top);
                    secondCell.RemoveWall(WallPosition.Bottom);
                }
                else
                {
                    firstCell.RemoveWall(WallPosition.Bottom);
                    secondCell.RemoveWall(WallPosition.Top);
                }
            }
            else
            {
                if (firstCell.X > secondCell.X)
                {
                    firstCell.RemoveWall(WallPosition.Left);
                    secondCell.RemoveWall(WallPosition.Right);
                }
                else
                {
                    firstCell.RemoveWall(WallPosition.Right);
                    secondCell.RemoveWall(WallPosition.Left);
                }
            }
        }
    }
}