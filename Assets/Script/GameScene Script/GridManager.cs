using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    int unityGridSize = 10; // Size of each grid cell in Unity units
    [SerializeField] int gridSizeX; // Number of columns in the grid
    [SerializeField] int gridSizeY; // Number of rows in the grid
    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>(); // Dictionary to store nodes with their corresponding coordinates
    public Dictionary<Vector2Int, Node> Grid { get { return grid; } } // Public property to access the grid dictionary

    // Start is called before the first frame update
    void Awake()
    {
        // Create the grid of nodes
        CreateGrid();
    }

    // Converts grid coordinates to a Unity world position
    public Vector3 GetPositionFromCoordinates(Vector2Int coordinates)
    {
        Vector3 position = new Vector3();
        position.x = coordinates.x * unityGridSize;
        position.z = coordinates.y * unityGridSize;
        return position;

    }

    // Converts a Unity world position to grid coordinates
    public Vector2Int GetCoordinatesFromPosition(Vector3 position)
    {
        Vector2Int coordinates = new Vector2Int();
        coordinates.x = Mathf.RoundToInt(position.x / unityGridSize);
        coordinates.y = Mathf.RoundToInt(position.z / unityGridSize);
        return coordinates;

    }

    // Resets the properties of all nodes in the grid
    public void ResetNode()
    {
        foreach (KeyValuePair<Vector2Int, Node> entry in grid)
        {
            entry.Value.connectedTo = null;
            entry.Value.isExplored = false;
            entry.Value.isPath = false;
        }
    }

    // Creates the grid of nodes
    private void CreateGrid()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector2Int coordinates = new Vector2Int(x, y);
                grid.Add(coordinates, new Node(coordinates, true));
            }
        }
    }

    // Blocks a node at the specified coordinates
    public void BlockNode(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            grid[coordinates].isWalkable = false;

        }
    }

    // Gets a node at the specified coordinates
    public Node GetNode(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            return grid[coordinates];
        }
        return null;

    }
}
