using System.Collections.Generic;
using UnityEngine;

namespace NodeGenerator
{
    public class ConnectedNode
    {
        public readonly Node Node;
        public readonly List<Vector2Int> Path;
        public readonly int PathId;

        public ConnectedNode(Node node, List<Vector2Int> path, int pathId)
        {
            Node = node;
            Path = path;
            PathId = pathId;
        }

        public override int GetHashCode()
        {
            return Node.GetHashCode();
        }
    }
}