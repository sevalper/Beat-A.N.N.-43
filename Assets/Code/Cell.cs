using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public static event Action<Cell> StartCellNeighbourSelected;

    [SerializeField]private int ID;
    private bool _selected;
    private bool _start;
    private List<Cell> _neighbourCells;
    private Vector2 _position;

    public bool GetStart() => _start;
    public void SetStart(bool s)
    {

        _start = s;
    }

    public Vector2 GetPosition => _position;
    public void SetPosition(Vector2 pos, int id)
    {
        ID = id;

        _position = pos;

        float offset = 0;

        if (pos.y % 2 != 0.0f)
            offset = 1.75f / 2f;
        float x = pos.x * 1.75f + offset;
        float y = pos.y * 2f * 0.75f;

        transform.SetPositionAndRotation(new Vector3(x,0,y),Quaternion.Euler(new Vector3(-90,0,0)));
    }

    public bool GetSelected() => _selected;
    public string SetSelected(bool s) { _selected = s;  return (ID + ""); }
    public string GetID() => ID.ToString();

    public List<Cell> GetNeighbourCell() => _neighbourCells;
    public void SetNeighbourCells(List<Cell> cells)
    {
        _neighbourCells = cells;
    }

    public bool CheckCell(Cell _actual)
    {
        if (_selected) return false;

        var encontrada = false;
        var start = false;

        var count = 0;
        foreach (var cell in _neighbourCells)
        {
            if (cell == _actual) encontrada = true;

            if (cell.GetStart())
            {
                start = true;
                
            }
            if (cell.GetSelected())
            {
                count++;
            }
        }

        if (start && encontrada)
        {
            Invoke(nameof(ThrowStartCellNeighbourSelectedEvent),0.01f);
            return true;
        }
        return count == 1 && encontrada;
    }

    private void ThrowStartCellNeighbourSelectedEvent()
    {
        StartCellNeighbourSelected(this);
    }
}
