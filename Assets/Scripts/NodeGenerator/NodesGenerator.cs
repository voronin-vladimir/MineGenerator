using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace NodeGenerator
{
    public class NodesGenerator
    {
        private int _width;
        private int _depth;
        private bool[,] _visitedCellState;
        private Dictionary<Vector2Int, Node> _nodePositions;
        private List<Node> _nodes;
        private System.Random _rnd;
        private WeightedList<int> _distanceList;

        public List<Node> Generate(int width, int depth)
        {
            _width = width;
            _depth = depth;
            _visitedCellState = new bool[width, depth];
            _nodePositions = new Dictionary<Vector2Int, Node>();
            _nodes = new List<Node>();
            _rnd = new System.Random();

            _distanceList = SetupDistanceList();

            var originPointX = width / 2;
            var generationQueue = new Queue<Node>();
            var originNodePath = new List<Vector2Int>();
            var pathId = 0;
            Node lastNode = null;
            
            generationQueue.ToList().Sort();
            
            for (var i = 0; i < depth; i++)
            {
                var position = new Vector2Int(originPointX, i);
                originNodePath.Add(position);
                VisitCell(position);

                if (i % 2 != 0)
                {
                    continue;
                }

                var node = CreateNode(position);
                generationQueue.Enqueue(node);

                if (i == 0)
                {
                    lastNode = node;
                    continue;
                }

                ConnectNodes(node, lastNode, originNodePath, ref pathId);
                pathId++;
                
                lastNode = node;
            }

            while (generationQueue.Any())
            {
                var startNode = generationQueue.Dequeue();
                var path = GetPath(startNode);

                if (!path.Any())
                    continue;

                var endPoint = path.Last();
                Node newNode;

                if (IsNode(endPoint))
                {
                    newNode = _nodePositions[endPoint];
                }
                else
                {
                    newNode = CreateNode(endPoint);
                    var branchesCount = _rnd.Next(1, 4);
                    for (int i = 0; i < branchesCount; i++)
                    {
                        generationQueue.Enqueue(newNode);
                    }
                }

                if (startNode == newNode)
                    continue;

                ConnectNodes(startNode, newNode, path, ref pathId);
            }

            return _nodes;
        }

        private static WeightedList<int> SetupDistanceList()
        {
            var distanceList = new WeightedList<int>();
            distanceList.Add(1, 1);
            distanceList.Add(2, 10);
            distanceList.Add(3, 20);
            distanceList.Add(4, 30);
            distanceList.Add(5, 20);

            return distanceList;
        }

        private List<Vector2Int> GetPath(Node originNode)
        {
            var distance = _distanceList.GetRandom();
            var path = new List<Vector2Int>();
            var currentPosition = originNode.Position;

            for (int i = 0; i < distance; i++)
            {
                var directions = GetPossibleDirection(currentPosition).ToArray();
                if (!directions.Any())
                    break;

                var randomDirectionIdx = _rnd.Next(directions.Length);
                var targetDirection = directions[randomDirectionIdx];
                path.Add(targetDirection);
                currentPosition = targetDirection;

                if (IsNode(targetDirection))
                    break;

                VisitCell(targetDirection);
            }

            return path;
        }

        private Node CreateNode(Vector2Int position)
        {
            var node = new Node(position);
            _nodePositions.Add(position, node);
            _nodes.Add(node);

            return node;
        }

        private IEnumerable<Vector2Int> GetPossibleDirection(Vector2Int position)
        {
            var neighbours = new List<Vector2Int>(4);
            var posX = position.x;
            var posY = position.y;

            var center = _width / 2;
            var isOriginNode = position.x == center;
            var isLeftSideNode = position.x < center;

            //Left
            if (posX > 0)
            {
                var pos = new Vector2Int(posX - 1, posY);

                if (IsValidCell(pos) && (isOriginNode || isLeftSideNode))
                {
                    neighbours.Add(pos);
                }
            }

            //Right
            if (posX < _width - 1)
            {
                var pos = new Vector2Int(posX + 1, posY);
                if (IsValidCell(pos) && (isOriginNode || !isLeftSideNode))
                    neighbours.Add(pos);
            }

            //Bottom
            if (posY > 0)
            {
                var pos = new Vector2Int(posX, posY - 1);
                if (IsValidCell(pos))
                    neighbours.Add(pos);
            }

            //Top
            if (posY < _depth - 1)
            {
                var pos = new Vector2Int(posX, posY + 1);
                if (IsValidCell(pos))
                    neighbours.Add(pos);
            }

            return neighbours;
        }

        private bool IsValidCell(Vector2Int pos)
        {
            return !IsVisitedCell(pos) || IsNode(pos);
        }

        private bool IsVisitedCell(Vector2Int pos)
        {
            return _visitedCellState.Get(pos);
        }

        private bool IsNode(Vector2Int pos)
        {
            return _nodePositions.ContainsKey(pos);
        }

        private void VisitCell(Vector2Int pos)
        {
            _visitedCellState.Set(pos, true);
        }

        private static void ConnectNodes(Node first, Node second, List<Vector2Int> path, ref int pathId)
        {
            first.Connect(second, path, pathId);
            second.Connect(first, path, pathId);
        }
    }
}