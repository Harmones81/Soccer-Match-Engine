using UnityEngine;

namespace SoccerMatchEngine.Units.Nodes
{
    public class Node
    {
        // The position of the node in world-space
        public Vector3 worldPos;
        // Whether the node is occupied or not
        public bool blocked;

        // Represents the row a node is located in
        public int row;
        // Represents the column a node is located in
        public int col;

        // Represents the score of a node's value (hCost)
        public int valueScore;
        // Represents the score of a node's distance from another node (gCost)
        public int distScore;
        // Represents the total overall score of a node (fCost)
        public int totalScore => valueScore + distScore;

        // Constructer
        public Node(int row, int col, Vector3 worldPos)
        {
            this.row = row;
            this.col = col;
            this.worldPos = worldPos;
        }
    }
}
