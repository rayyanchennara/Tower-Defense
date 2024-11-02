using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField][Range(0f, 5f)] float speed = 1f; // Movement speed of the enemy
    List<Node> path = new List<Node>(); // List of nodes representing the path to move for enemy
    Dictionary<Vector2Int, Node> grid; // Dictionary mapping grid coordinates to Node objects
    GridManager gridManager; // Reference to the GridManager script
    PathFinding pathFinding; // Reference to the PathFinding script
    Enemy enemy; // Reference to the Enemy script

    void OnEnable()
    {
        // Reset the enemy's position to the starting point
        ReturnToStart();

        // Recalculate the path from the starting point
        RecalculatePath(true);
    }

    void Awake()
    {
        // Find the GridManager and PathFinding scripts in the scene
        gridManager = FindObjectOfType<GridManager>();
        pathFinding = FindObjectOfType<PathFinding>();

        // Get the Enemy script and the grid dictionary from the GridManager
        enemy = GetComponent<Enemy>();
        grid = gridManager.Grid;
    }

    // Resets the enemy's position to the starting coordinates
    void ReturnToStart()
    {
        transform.position = gridManager.GetPositionFromCoordinates(pathFinding.StartCoordinates);
    }

    // Recalculates the path for the enemy, optionally resetting to the starting point
    private void RecalculatePath(bool resetPath)
    {
        Vector2Int coordinates = new Vector2Int();

        // If resetPath is true, use the starting coordinates
        if (resetPath)
        {
            coordinates = pathFinding.StartCoordinates;
        }
        else
        {
            // Get the current coordinates from the enemy's position
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
        }

        // Stop any existing path-following coroutines
        StopAllCoroutines();

        // Clear the existing path
        path.Clear();

        // Get a new path from the PathFinding script
        if (pathFinding != null)
        {
            if (path != null)
            {
                path = pathFinding.GetNewPath(coordinates);
            }
        }

        // Start the coroutine to follow the new path
        StartCoroutine(FollowPath());
    }

    // Coroutine to follow the calculated path
    IEnumerator FollowPath()
    {
        for (int i = 1; i < path.Count; i++)
        {
            // Get the start and end positions for the current path segment
            Vector3 startPosition = transform.position;
            Vector3 endPosition = gridManager.GetPositionFromCoordinates(path[i].coordinates);

            // Initialize the travel percentage
            float travelPercent = 0;

            // Look towards the end position
            transform.LookAt(endPosition);

            // Move towards the end position until the travel percentage reaches 1
            while (travelPercent < 1f)
            {
                // Update the travel percentage based on speed and time
                travelPercent += speed * Time.deltaTime;

                // Interpolate the position between the start and end positions
                transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);

                // Yield to the next frame to update movement
                yield return new WaitForEndOfFrame();
            }
        }

        // When the path is complete, deactivate the enemy and subtract player's point
        gameObject.SetActive(false);
        enemy.StealGold();
    }
}
