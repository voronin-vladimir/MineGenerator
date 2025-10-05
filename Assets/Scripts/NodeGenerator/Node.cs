using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NodeGenerator
{
    public class Node
    {
        public readonly Vector2Int Position;
        public readonly HashSet<ConnectedNode> Connections;

        public bool IsOpen;

        public Node(Vector2Int position)
        {
            Position = position;
            Connections = new HashSet<ConnectedNode>();
        }

        public void Connect(Node node, List<Vector2Int> path, int pathId)
        {
            Connections.Add(new ConnectedNode(node, path, pathId));
        }

        public bool HasConnection(Node node)
        {
            return Connections.Any(c => c.Node == node);
        }
    }
}