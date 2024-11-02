using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class CoordinatesLabel : MonoBehaviour
{
    // CASH
    TextMeshPro textMeshPro; // Reference to the TextMeshPro component for displaying coordinates
    GridManager gridManager; // Reference to the GridManager script
    Vector2Int coordinates = new Vector2Int(); // Stores the current grid coordinates

    // Colors used for different node states
    [SerializeField] Color defaultColor = Color.white; // Default color for walkable nodes
    [SerializeField] Color blockedColor = Color.gray; // Color for blocked nodes
    [SerializeField] Color pathColor = Color.blue; // Color for path nodes
    [SerializeField] Color exploredColor = Color.yellow; // Color for explored nodes

    // Start is called before the first frame update
    void Awake()
    {
        // Find the GridManager script in the scene
        gridManager = FindObjectOfType<GridManager>();

        // Get the TextMeshPro component attached to this gameObject
        textMeshPro = GetComponent<TextMeshPro>();

        // Update and display the initial coordinates
        DisplayCoordinates();
    }

    // Update is called once per frame
    void Update()
    {
        // Update coordinates and display only in editor mode
        if (!Application.isPlaying)
        {
            DisplayCoordinates();
            UpdateParentName();
        }

        // Update the text color based on the node's state
        ColorCoordinates();

        // Toggle visibility of the coordinate label on 'C' key press
        ToggleLabels();
    }

    // Toggles visibility of the coordinate label
    private void ToggleLabels()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            textMeshPro.enabled = !textMeshPro.IsActive(); // Toggle active state
        }
    }

    // Updates the color of the text based on the underlying node state
    private void ColorCoordinates()
    {
        if (gridManager == null) { return; } // Exit if GridManager not found

        // Get the node data from the GridManager using the current coordinates
        Node node = gridManager.GetNode(coordinates);
        if (node == null) { return; } // Exit if node not found

        // Set the text color based on the node isWalkable and isPath/isExplored states
        if (!node.isWalkable)
        {
            textMeshPro.color = blockedColor;
        }
        else if (node.isPath)
        {
            textMeshPro.color = pathColor;
        }
        else if (node.isExplored)
        {
            textMeshPro.color = exploredColor;
        }
        else
        {
            textMeshPro.color = defaultColor;
        }
    }

    // Updates the stored coordinates based on the parent object's position
    private void UpdateParentName()
    {
        transform.parent.name = coordinates.ToString();
    }

    // Update text with new coordinates
    private void DisplayCoordinates()
    {
        int gridSize = 10; // Assumed grid size

        // Convert parent's world position to grid coordinates based on grid size
        coordinates.x = Mathf.RoundToInt(transform.parent.position.x / gridSize);
        coordinates.y = Mathf.RoundToInt(transform.parent.position.z / gridSize);
        textMeshPro.text = coordinates.x + "," + coordinates.y; // Set text with formatted coordinates
    }
}
