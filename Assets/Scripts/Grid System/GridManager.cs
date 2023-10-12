using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public int gridSizeX;
    public int gridSizeY;
    public float cellSize = 1.0f;
    
    [SerializeField]
    private GridNode[,] grid;

    void Start()
    {
        InitializeGrid();
    }

    void InitializeGrid()
    {
        grid = new GridNode[gridSizeX, gridSizeY];
        // Initialize the grid nodes
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 nodePosition = new Vector3(x * cellSize, 0, y * cellSize);
                GridNode.GridType nodeType = GridNode.GridType.Empty;
                grid[x, y] = new GridNode(nodeType, nodePosition);
            }
        }
    }

    public GridNode GetNode(int x, int y)
    {
        if (x >= 0 && x < gridSizeX && y >= 0 && y < gridSizeY)
        {
            return grid[x, y];
        }
        return null;
    }

    public GridNode GetNodeAtPosition(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x / cellSize);
        int y = Mathf.FloorToInt(position.z / cellSize);

        return GetNode(x, y);
    }
    void OnDrawGizmos()
    {
        if (grid == null)
            return;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                GridNode node = grid[x, y];

                // Set Gizmo color based on the node type
                switch (node.type)
                {
                    case GridNode.GridType.Empty:
                        Gizmos.color = Color.white;
                        break;
                    case GridNode.GridType.Obstacle:
                        Gizmos.color = Color.red;
                        break;
                    case GridNode.GridType.FarmLand:
                        Gizmos.color = Color.green;
                        break;
                    case GridNode.GridType.Building:
                        Gizmos.color = Color.blue;
                        break;
                }

                Gizmos.DrawCube(node.position, Vector3.one * cellSize);
            }
        }
    }
}
