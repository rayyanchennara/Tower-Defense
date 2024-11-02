using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    public Vector2Int coordinates; // Coordinates of the node on the grid
    public bool isWalkable; // Indicates if the node can place tower and move enemy
    public bool isExplored; // Indicates if the node has been explored during pathfinding
    public bool isPath; // Indicates if the node is part of the path
    public Node connectedTo; // Using for connect path nodes during pathfinding

    // Constructor for the Node class
    public Node(Vector2Int coordinates, bool isWalkable)
    {
        this.coordinates = coordinates; // Set the coordinates of the node
        this.isWalkable = isWalkable; // Set whether the node is walkable
    }
}
