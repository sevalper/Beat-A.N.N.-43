using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private int gridSizeX;
    [SerializeField] private int gridSizeY;
    [SerializeField] private GameObject cell;
    [SerializeField] private SelectionAction selAct;
    [SerializeField] private Manager manager;
    [SerializeField] private GameObject buttonContainer;
    [SerializeField] private GameObject floor;

    private int min = 1;

    private Cell[,] grid;

    public void SetInit() { selAct.SetActual(grid[3, 3]); }

    public Cell GetCell(string id)
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                if (grid[x, y].GetID().Equals(id))
                {
                    return grid[x, y];
                }
            }
        }
        return null;
    }

    private void OnEnable()
    {
        grid = new Cell[gridSizeX,gridSizeY];
        Manager.FinishPath += UP;
    }

    private void Start()
    {
        buttonContainer.SetActive(false);
        selAct.enabled = true;
        manager.Clear();
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                var newCell = Instantiate(cell);
                grid[x, y] = newCell.GetComponent<Cell>();
                grid[x, y].SetPosition(new Vector2(x,y), y + (x * gridSizeX));
                
            }
        }

        List<Cell> neighbourCells = new List<Cell>();

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                if (OnBounds(x, y))
                {
                    continue;
                }
                if (y % 2 == 0)
                {
                    if (InRange(min, gridSizeX, x) && InRange(min, gridSizeY, y - 1))
                    {
                        neighbourCells.Add(grid[x, y - 1]);
                    }
                    if (InRange(min, gridSizeX, x - 1) && InRange(min, gridSizeY, y - 1))
                    {
                        neighbourCells.Add(grid[x - 1, y - 1]);
                    }
                    if (InRange(min, gridSizeX, x - 1) && InRange(min, gridSizeY, y))
                    {
                        neighbourCells.Add(grid[x - 1, y]);
                    }
                    if (InRange(min, gridSizeX, x + 1) && InRange(min, gridSizeY, y))
                    {
                        neighbourCells.Add(grid[x + 1, y]);
                    }
                    if (InRange(min, gridSizeX, x - 1) && InRange(min, gridSizeY, y + 1))
                    {
                        neighbourCells.Add(grid[x - 1, y + 1]);
                    }
                    if (InRange(min, gridSizeX, x) && InRange(min, gridSizeY, y + 1))
                    {
                        neighbourCells.Add(grid[x, y + 1]);
                    }
                }
                else
                {
                    if (InRange(min, gridSizeX, x + 1) && InRange(min, gridSizeY, y - 1))
                    {
                        neighbourCells.Add(grid[x + 1, y - 1]);
                    }
                    if (InRange(min, gridSizeX, x ) && InRange(min, gridSizeY, y - 1))
                    {
                        neighbourCells.Add(grid[x, y - 1]);
                    }
                    if (InRange(min, gridSizeX, x - 1) && InRange(min, gridSizeY, y))
                    {
                        neighbourCells.Add(grid[x - 1, y]);
                    }
                    if (InRange(min, gridSizeX, x + 1) && InRange(min, gridSizeY, y))
                    {
                        neighbourCells.Add(grid[x + 1, y]);
                    }
                    if (InRange(min, gridSizeX, x) && InRange(min, gridSizeY, y + 1))
                    {
                        neighbourCells.Add(grid[x , y + 1]);
                    }
                    if (InRange(min, gridSizeX, x +1) && InRange(min, gridSizeY, y + 1))
                    {
                        neighbourCells.Add(grid[x +1, y + 1]);
                    }
                }
                grid[x, y].SetNeighbourCells(neighbourCells);

                neighbourCells = new List<Cell>();
            }
        }
        selAct.SetActual(grid[3,3]);
        grid[3, 3].SetSelected(true);
        grid[3, 3].SetStart(true);
        grid[3, 3].gameObject.GetComponent<Renderer>().material.color = Color.red;
    }

    private bool OnBounds(int x, int y)
    {
        if (x == 0 || y == 0 || x == gridSizeX - 1 || y == gridSizeY - 1)
        {
            grid[x, y].gameObject.GetComponent<Renderer>().material.color = Color.black;//Esto es estética
            Vector3 scale = new Vector3(100,100,500);
            grid[x, y].gameObject.tag = "terrain";
            grid[x, y].gameObject.layer = 8;
            grid[x, y].transform.localScale = scale;
            return true;
        }
        return false;
    }

    private void UP()
    {
        floor.SetActive(true);

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                if (!grid[x, y].GetSelected())
                {
                    Vector3 scale = new Vector3(100, 100, 500);
                    grid[x, y].gameObject.tag = "terrain";
                    grid[x, y].gameObject.layer = 8;
                    grid[x, y].transform.localScale = scale;
                }
                else
                {
                   // grid[x, y].gameObject.GetComponent<Renderer>().material.color = Color.white;
                }
            }
        }
        buttonContainer.SetActive(true);
    }

    private bool InRange(int min, int max, int number) => min <= number && max - 1  > number;

    public void ResetPath()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Destroy(grid[x, y].gameObject);
            }
        }
        enabled = false;
        grid = new Cell[gridSizeX, gridSizeY];
        Start();
    }
}
