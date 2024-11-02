using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[ExecuteAlways]
public class GridMaking : MonoBehaviour
{
    [SerializeField] int columns; // Number of columns in the grid
    [SerializeField] int rows; // Number of rows in the grid
    int totalGrid; // Total number of tiles (columns * rows)
    [SerializeField] GameObject tile; // Prefab of the tile object to instantiate
    public GameObject parentObject; // Parent object for the grid tiles
    int childCount; // Current number of child objects under the parent
    List<GameObject> squares = new List<GameObject>(); // List to store the instantiated tiles

    // Start is called before the first frame update
    void Awake()
    {
        // Create the grid of tiles
        CreateGrid();
    }

    // Creates the grid of tiles
    private void CreateGrid()
    {
        // Get the current number of child objects under the parent
        childCount = parentObject.transform.childCount;

        // Calculate the total number of tiles
        totalGrid = columns * rows;

        // Check if the number of existing tiles matches the desired total
        if (childCount == totalGrid)
        {
            // If the number of tiles matches, no need to create new ones
        }
        else
        {
            // If the number of tiles doesn't match, spawn the grid
            SpawnGrid();
        }
    }

    // Spawns the grid of tiles
    private void SpawnGrid()
    {
        int columnNumber = 0; // Current column number
        int rowNumber = 0; // Current row number
        int gridSize = 10; // Size of each grid cell

        // Iterate through each column and row
        for (int column = 0; column < columns; column++)
        {
            for (int row = 0; row < rows; row++)
            {
                // If the current column number exceeds the total columns, move to the next row and reset the column number
                if (columnNumber + 1 > columns)
                {
                    rowNumber++;
                    columnNumber = 0;
                }

                // Calculate the offset positions for the tile
                int posXOffset = gridSize * columnNumber;
                int posYOffset = gridSize * rowNumber;

                // Instantiate a new tile object and add it to the squares list
                squares.Add(Instantiate(tile) as GameObject);

                // Set the parent of the new tile to this script's transform
                squares[squares.Count - 1].transform.parent = this.transform;

                // Set the position of the new tile based on the calculated offsets
                squares[squares.Count - 1].transform.position = new Vector3(posXOffset, 0, posYOffset);

                // Increment the column number for the next iteration
                columnNumber++;
            }
        }
    }
}
