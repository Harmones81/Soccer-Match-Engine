using SoccerMatchEngine.Units.Nodes;
using System.Collections.Generic;
using UnityEngine;

namespace SoccerMatchEngine.Units.Pitch
{
    public class Pitch : MonoBehaviour
    {
        [Header("Pitch Settings")]
        // Represents the size of the pitch
        [SerializeField] private Vector2 _pitchSize;

        [Header("Display Settings")]
        [SerializeField] private bool _displayGizmos;

        // Represents the amount of nodes we can fit in one row
        private int _pitchSizeX;
        // Represents the amount of nodes we can fit in one col
        private int _pitchSizeY;

        // A 2D array of all the nodes on the pitch (by row and col)
        private Node[,] _nodes;

        [Header("Node Settings")]
        // How much space each node takes up
        [SerializeField] private float _nodeRadius;
        // Represents the entire width and length of each node
        private float _nodeDiameter => _nodeRadius * 2;

        private void Awake()
        {
            _pitchSizeX = Mathf.RoundToInt(_pitchSize.x / _nodeDiameter);
            _pitchSizeY = Mathf.RoundToInt(_pitchSize.y / _nodeDiameter);

            GenerateNodes();
        }

        private void GenerateNodes()
        {
            _nodes = new Node[_pitchSizeX, _pitchSizeY];

            Vector3 pitchBottomLeft = transform.position - Vector3.right * _pitchSize.x / 2 - Vector3.forward * _pitchSize.y / 2;

            for (int x = 0; x < _pitchSizeX; x++)
            {
                for (int y = 0; y < _pitchSizeY; y++)
                {
                    Vector3 pitchLocation = pitchBottomLeft + Vector3.right * (x * _nodeDiameter + _nodeRadius) + Vector3.forward * (y * _nodeDiameter + _nodeRadius);

                    _nodes[x, y] = new Node(x, y, pitchLocation);
                }
            }
        }

        // Returns the node that is at the specified world position
        public Node NodeFromWorldPoint(Vector3 worldPos)
        {
            float percentX = (worldPos.x + _pitchSize.x / 2) / _pitchSize.x;
            float percentY = (worldPos.z + _pitchSize.y / 2) / _pitchSize.y;

            percentX = Mathf.Clamp01(percentX);
            percentY = Mathf.Clamp01(percentY);

            int x = Mathf.RoundToInt((_pitchSizeX - 1) * percentX);
            int y = Mathf.RoundToInt((_pitchSizeY - 1) * percentY);

            return _nodes[x, y];
        }

        // Gets the neighbors of the specified node
        public List<Node> GetNeighbors(Node node)
        {
            List<Node> neighbors = new List<Node>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    int checkX = node.row + x;
                    int checkY = node.col + y;

                    if (checkX >= 0 && checkX < _pitchSizeX && checkY >= 0 && checkY < _pitchSizeY)
                    {
                        neighbors.Add(_nodes[checkX, checkY]);
                    }
                }
            }

            return neighbors;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(_pitchSize.x, 0.08f, _pitchSize.y));

            if (_nodes != null && _displayGizmos)
            {
                foreach (Node node in _nodes)
                {
                    //Gizmos.color = (node.blocked) ? Color.white : Color.green;

                    Gizmos.color = Color.white;

                    Gizmos.DrawCube(node.worldPos, new Vector3((_nodeDiameter - 0.01f), 0.1f, (_nodeDiameter - 0.01f)));
                }
            }
        }
    }
}
