using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField]
    private RobotUpgrade[,] grid;

    [SerializeField]
    private GridScript gridScript;

    private void Start()
    {
        grid = new RobotUpgrade[gridScript.GetWidth(), gridScript.getHeight()];

        for (int i = 0; i < gridScript.GetWidth(); i++)
        {
            for (int j = 0; j < gridScript.getHeight(); j++)
            {
                grid[i, j] = null;
            }
        }

    }

    /// <summary>
    /// Adds a new shape in the new position specified by upgrade.
    /// Checks if the new position is occupied or partially out of bounds. If so, returns false.
    /// </summary>
    /// <param name="upgrade"></param>
    /// <param name="initialPosition"></param>
    /// <param name="lastCells"></param>
    /// <returns>
    /// True if the shape can be added, 
    /// False: otherwhise</returns>
    public bool AddShape(RobotUpgrade upgrade, Vector3 initialPosition, Vector2[] lastCells)
    {
        Vector2[] cells = upgrade.getCells();
        Vector2 pointOfOrigin = upgrade.getPOI();
        foreach (Vector2 cell in cells)
        {
            Vector2 realPosition = cell + pointOfOrigin;

            int x = (int)Mathf.Floor(realPosition.x);
            int y = (int)Mathf.Floor(realPosition.y);
            Debug.Log(x + "," + y);
            if (x >= gridScript.GetWidth() || y >= gridScript.getHeight() || x < 0 || y < 0)
                return false;

            if (grid[x, y] != null)
                if (grid[x, y] != upgrade)      //La cella è piena e non è occupata
                    return false;               //Dalla stessa forma

        }
        for (int i = 0; i < cells.Length; i++)
        {
            Vector2 cell = cells[i];
            Vector2 lastCell = lastCells[i];
            Vector2 realPosition = cell + pointOfOrigin;

            Vector2 oldPosition = initialPosition;

            Vector2 removePosition = oldPosition + lastCell;

            int x = (int)Mathf.Floor(removePosition.x);
            int y = (int)Mathf.Floor(removePosition.y);
            grid[x, y] = null;

            x = (int)Mathf.Floor(realPosition.x);
            y = (int)Mathf.Floor(realPosition.y);

            grid[x, y] = upgrade;


        }



        return true;

    }
    /// <summary>
    /// Returns true if the shape is completely out of bounds.
    /// </summary>
    /// <param name="upgrade">The shape to check</param>
    /// <returns></returns>
    public bool IsOutOfBounds(RobotUpgrade upgrade)
    {
        Vector2[] cells = upgrade.getCells();
        Vector2 pointOfOrigin = upgrade.getPOI();
        int outOfBoundsCounter = 0;
        foreach (Vector2 cell in cells)
        {
            Vector2 realPosition = cell + pointOfOrigin;

            int x = (int)Mathf.Floor(realPosition.x);
            int y = (int)Mathf.Floor(realPosition.y);
            Debug.Log(x + "," + y);
            if (x >= gridScript.GetWidth() || y >= gridScript.getHeight() || x < 0 || y < 0)
                outOfBoundsCounter++;
        }
        if (outOfBoundsCounter == cells.Length)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


}
