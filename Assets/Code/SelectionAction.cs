using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionAction : MonoBehaviour
{
    [SerializeField] private Manager manager;
    [SerializeField] private GameObject wayPoint;

    private List<Cell> selectedCells = new List<Cell>();
    private Cell _actual;
    private int _wayPointCount;
    private List<GameObject> wayPoints = new List<GameObject>();

    public void SetActual(Cell c) { _actual = c; }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            int layerMask = 1 << 9;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                var cellScript = hit.transform.gameObject.GetComponent<Cell>();
                if (cellScript != null)
                {
                    if (!cellScript.CheckCell(_actual)) return;
                    else
                    {
                        string aux = cellScript.SetSelected(true);
                        manager.AddCellToCircuit(aux);
                        cellScript.gameObject.GetComponent<Renderer>().material.color = Color.blue;
                        _actual = cellScript;
                        selectedCells.Add(cellScript);
                        TryInstantiateWayPoint(cellScript);

                    }
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (!manager.GetFinished())
            {
                foreach (var cell in selectedCells)
                {
                    if(cell!= null)
                        cell.gameObject.GetComponent<Renderer>().material.color = Color.white;
                        cell.SetSelected(false);
                }
                manager.Clear();
                selectedCells.Clear();
                ClearWayPoints();
                
            }
        }
    }

    private void TryInstantiateWayPoint(Cell cellScript)
    {
        _wayPointCount++;
        if (_wayPointCount <= 3) return;

        _wayPointCount = 0;

         var position = cellScript.GetPosition;

        float offset = 0;

        if (position.y % 2 != 0.0f)
            offset = 1.75f / 2f;
        float x = position.x * 1.75f + offset;
        float y = position.y * 2f * 0.75f;

        wayPoints.Add( Instantiate(wayPoint, new Vector3(x, 0, y), Quaternion.Euler(new Vector3(-90, 0, 0))));
    }

    public void ClearWayPoints()
    {
        foreach (var wp in wayPoints)
            Destroy(wp);
        wayPoints.Clear();
    }
}
