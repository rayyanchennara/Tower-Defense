using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    [SerializeField] Vector2Int startCoordinates; // Coordinates of the starting point
    public Vector2Int StartCoordinates { get { return startCoordinates; } } // Public property to access the starting coordinates
    [SerializeField] Vector2Int destinationCoordinates; // Coordinates of the destination point

    // Possible directions to explore neighboring nodes
    Vector2Int[] directions = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };

    // Dictionaries and lists for pathfinding
    Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>(); // Grid of nodes
    Dictionary<Vector2Int, Node> reached = new Dictionary<Vector2Int, Node>(); // Nodes reached during the search
    List<Node> path = new List<Node>(); // Calculated path
    Queue<Node> frontier = new Queue<Node>(); // Queue of nodes to be explored

    // Reference to the GridManager component
    GridManager gridManager;

    // Nodes representing the starting, destination, and current search node
    Node startNode;
    Node destinationNode;
    Node currentSearchNode;

    void Awake()
    {
        // Get the GridManager component
        gridManager = GetComponent<GridManager>();

        // If the GridManager is found, initialize the grid, start node, and destination node
        if (gridManager != null)
        {
            grid = gridManager.Grid; // Get the grid from the GridManager
            startNode = grid[startCoordinates]; // Set the start node
            destinationNode = grid[destinationCoordinates]; // Set the destination node
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Calculate the initial path
        GetNewPath();
    }

    // Gets a new path from the starting point
    public List<Node> GetNewPath()
    {
        return GetNewPath(startCoordinates);
    }

    // Gets a new path from the specified coordinates
    public List<Node> GetNewPath(Vector2Int coordinates)
    {
        // Reset all nodes in the grid
        gridManager.ResetNode();

        // Perform Breadth-First Search from the given coordinates
        BreadthFirstSearch(coordinates);

        // Build the path from the destination to the start
        return BuildPath();
    }

    // Performs Breadth-First Search to find the shortest path
    private void BreadthFirstSearch(Vector2Int coordinates)
    {
        // Mark the start and destination nodes as walkable
        startNode.isWalkable = true;
        destinationNode.isWalkable = true;

        // Clear the frontier and reached dictionaries
        frontier.Clear();
        reached.Clear();

        // Set the search flag to true
        bool isRunning = true;

        // Enqueue the starting node and add it to reached
        frontier.Enqueue(grid[coordinates]);
        reached.Add(coordinates, grid[coordinates]);

        // Continue searching until the frontier is empty or the destination is reached
        while (frontier.Count >= 0 && isRunning)
        {
            // Dequeue the current search node
            currentSearchNode = frontier.Dequeue();

            // Mark the current node as explored
            currentSearchNode.isExplored = true;

            // Explore neighboring nodes
            ExploreNeighbors();

            // If the current node is the destination, stop the search
            if (currentSearchNode.coordinates == destinationCoordinates)
            {
                isRunning = false;
            }
        }
    }

    // Explores neighboring nodes of the current search node
    private void ExploreNeighbors()
    {
        // Create a list to store neighboring nodes
        List<Node> neighbors = new List<Node>();

        // Iterate through all possible directions
        foreach (Vector2Int direction in directions)
        {
            // Calculate the coordinates of the neighbor
            Vector2Int neighborsCoordinates = currentSearchNode.coordinates + direction;

            // Check if the neighbor exists in the grid
            if (grid.ContainsKey(neighborsCoordinates))
            {
                // Add the neighbor to the list
                neighbors.Add(grid[neighborsCoordinates]);
            }
        }

        // Iterate through the neighboring nodes
        foreach (Node neighbor in neighbors)
        {
            // Check if the neighbor has not been reached and is walkable
            if (!reached.ContainsKey(neighbor.coordinates) && neighbor.isWalkable)
            {
                // Set the neighbor's connected node to the current node
                neighbor.connectedTo = currentSearchNode;

                // Add the neighbor to reached and frontier
                reached.Add(neighbor.coordinates, neighbor);
                frontier.Enqueue(neighbor);
            }
        }
    }

    // Builds the path from the destination to the start
    private List<Node> BuildPath()
    {
        // Start with the destination node
        Node currentNode = destinationNode;

        // Add the destination node to the path and mark it as part of the path
        path.Add(currentNode);
        currentNode.isPath = true;

        // Trace back the path using connectedTo references
        while (currentNode.connectedTo != null)
        {
            currentNode = currentNode.connectedTo;
            path.Add(currentNode);
            currentNode.isPath = true;
        }

        // Reverse the path to get the correct order from start to destination
        path.Reverse();

        // Return the calculated path
        return path;
    }

    // Checks if blocking a node at the specified coordinates will block the path
    public bool WillBlockPath(Vector2Int coordinates)
    {
        if (grid.ContainsKey(coordinates))
        {
            // Temporarily block the node
            bool previousState = grid[coordinates].isWalkable;
            grid[coordinates].isWalkable = false;

            // Calculate a new path
            List<Node> newPath = GetNewPath();

            // Restore the previous state of the node
            grid[coordinates].isWalkable = previousState;

            // If the new path is empty or has only one node, the path is blocked
            if (newPath.Count <= 1)
            {
                // Recalculate the original path
                GetNewPath();

                // Indicate that blocking the node will block the path
                return true;
            }
        }

        // If the path is not blocked, return false
        return false;
    }

    // Notifies other objects to recalculate their paths
    public void NotifyReceivers()
    {
        BroadcastMessage("RecalculatePath", false, SendMessageOptions.DontRequireReceiver);
    }
}
