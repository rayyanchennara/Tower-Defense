using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    Vector2Int coordinates = new Vector2Int(); // Coordinates of the placement position on the grid
    [SerializeField] bool isPlaceable; // Indicates if the tower can be placed at this position
    [SerializeField] GameObject towerPrefabe; // Prefab of the tower object to instantiate

    public bool IsPlaceable { get { return isPlaceable; } } // Public property to access the isPlaceable flag

    // References to other components
    GridManager gridManager;
    PathFinding pathFinding;
    Tower tower;

    void Awake()
    {
        // Find the GridManager and PathFinding components in the scene
        gridManager = FindObjectOfType<GridManager>();
        pathFinding = FindObjectOfType<PathFinding>();

        // Get the Tower component from the tower prefab
        tower = towerPrefabe.GetComponent<Tower>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // If the GridManager is found, get the coordinates from the transform's position and block the node if necessary
        if (gridManager != null)
        {
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);

            if (!isPlaceable)
            {
                gridManager.BlockNode(coordinates);

            }
        }
    }

    // Called when the object is clicked
    private void OnMouseDown()
    {
        // Check if PathFinding component is found
        if (pathFinding == null)
        {
            return;
        }

        // Check if the node at the coordinates is walkable and won't block the path
        if (gridManager.GetNode(coordinates).isWalkable && !pathFinding.WillBlockPath(coordinates))
        {
            // If the tower prefab and Tower component are valid, create the tower
            if (towerPrefabe != null && tower != null)
            {
                bool isPlaced = tower.CreateTower(towerPrefabe, transform.position);

                // If the tower was successfully placed, block the node and notify other objects to recalculate their paths
                if (isPlaced)
                {
                    gridManager.BlockNode(coordinates);
                    pathFinding.NotifyReceivers();
                }
            }
        }
    }
}
