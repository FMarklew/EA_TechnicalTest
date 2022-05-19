using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Handles core logic
/// </summary>
public class GameManager : MonoBehaviour
{
    private List<List<Cell>> allCells = new List<List<Cell>>(); // all cells currently in use

    [Tooltip("Cell prefab to be organized by grid")]
    public Cell cellPrefab;
    [Tooltip("Parent to set the cells under - layout is handled by GridLayoutGroup")]
    public Transform gridLayoutParent;
    public int gridSize = 5;

    [Tooltip("How many clicks to simulate to set up the board - generally speaking, more iterations will produce a harder puzzle")]
    public int difficultyIterations = 5;

    public delegate void ClickCoordinates(int iCoord, int jCoord);
    /// <summary>
    /// Event for when a cell is clicked - called by a cell remotely
    /// </summary>
    public static ClickCoordinates OnClickedCoordinates;

    /// <summary>
    /// Unity Event for game over
    /// </summary>
    public static UnityEvent GameOverEvent = new UnityEvent();

    /// <summary>
    /// Unity Event for game Start
    /// </summary>
    public static UnityEvent GameStartEvent = new UnityEvent();

    private int[,] currentConfiguration; // current configuration so the board can be reset without starting a new one
    void Start()
    {        
        GenerateGrid();
        NewPuzzle();
    }

    private void OnEnable()
    {
        OnClickedCoordinates += ToggleCell;
    }

    private void OnDisable()
    {
        OnClickedCoordinates -= ToggleCell;
    }

    public void GenerateGrid()
    {
        for(int i = 0; i < gridSize; i++)
        {
            allCells.Add(new List<Cell>()); // new empty list to add to
            for (int j = 0; j < gridSize; j++)
            {
                Cell c = Instantiate(cellPrefab, gridLayoutParent);
                c.Init(i, j);
                allCells[i].Add(c);
                c.gameObject.name = $"({i},{j})"; // set game object name for ease of debugging
            }
        }
        ClearPuzzle();
        Debug.Log($"Generated grid {allCells[0].Count} x {allCells.Count}");
    }

    public void ToggleCell(int iCoord, int jCoord)
    {
        allCells[iCoord][jCoord].Toggle();

        // toggle left
        if (iCoord > 0)
        {
            allCells[iCoord - 1][jCoord].Toggle();
        }

        // toggle right
        if (iCoord < allCells[iCoord].Count-1)
        {
            allCells[iCoord + 1][jCoord].Toggle();
        }

        // toggle above
        if(jCoord > 0)
        {
            allCells[iCoord][jCoord-1].Toggle();
        }

        // toggle below
        if (jCoord < allCells[iCoord].Count - 1)
        {
            allCells[iCoord][jCoord+1].Toggle();
        }

        // check game over
        if (CheckComplete())
        {
            GameOver();
        }
    }

    /// <summary>
    /// Reset puzzle to most recent configuration
    /// </summary>
    public void ResetPuzzle()
    {
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                if(currentConfiguration[i, j] == 0)
                {
                    allCells[i][j].SetState(false);
                } else
                {
                    allCells[i][j].SetState(true);
                }     
            }
        }
        GameStartEvent.Invoke();
    }

    /// <summary>
    /// Set all cells to the off state
    /// </summary>
    public void ClearPuzzle()
    {
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                allCells[i][j].SetState(false);                
            }
        }
    }

    /// <summary>
    /// click some cells sequentially so we know it is solvable - more difficulty iterations should generally be more challenging
    /// </summary>
    public void NewPuzzle()
    {
        ClearPuzzle();
        for (int i = 0; i < difficultyIterations; i++)
        {
            int randI = Random.Range(0, gridSize);
            int randJ = Random.Range(0, gridSize);
            allCells[randI][randJ].OnClick();
            Debug.Log($"Clicked ({randI}, {randJ})"); // this debug is to show the clicks performed automatically when setting up the puzzle - clicking all of these again will complete the puzzle (there may be a better solution however)
        }
        StoreCurrentConfiguration();
        GameStartEvent.Invoke();
    }

    /// <summary>
    /// Store the current configuration of the board for reset purposes
    /// </summary>
    private void StoreCurrentConfiguration()
    {
        currentConfiguration = new int[gridSize, gridSize];
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                if (allCells[i][j].isActive)
                {
                    currentConfiguration[i, j] = 1;
                }
            }
        }
    }

    /// <summary>
    /// Game over, call the game over event
    /// </summary>
    public void GameOver()
    {
        GameOverEvent.Invoke();
    }

    /// <summary>
    /// Check to see if the board is completed
    /// </summary>
    /// <returns></returns>
    private bool CheckComplete()
    {
        for (int i = 0; i < gridSize; i++)
        {
            for (int j = 0; j < gridSize; j++)
            {
                if (allCells[i][j].isActive)
                {
                    return false;
                }
            }
        }
        return true;
    }

}
