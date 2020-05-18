using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TrainingPathCreator : MonoBehaviour
{
    [SerializeField] private Manager manager;
    [SerializeField] private GameObject wayPoint;
    [SerializeField] private Grid grid;
    [SerializeField] private GameObject meta;
    [SerializeField] private GameObject metaCollider;

    private List<Cell> selectedCells = new List<Cell>();
    private Cell _actual;
    private int _wayPointCount;
    private List<GameObject> wayPoints = new List<GameObject>();

    public void SetActual(Cell c) { _actual = c; }

    public string[] lines;

    private void Start()
    {
        ReadData();
        if (!PlayerPrefs.HasKey("Training"))
            PlayerPrefs.SetInt("Training", 0);

        Invoke(nameof(getNextCircuit),3f);
    }

    [ContextMenu("DeletePlayerPrefs")]
    private void DeletePlayerPrefs() => PlayerPrefs.DeleteKey("Training");

    private void ReadData()
    {
        var sr = new StreamReader(Application.dataPath + "/" + "DATA/def.txt");
        var fileContents = sr.ReadToEnd();
        sr.Close();
        lines = fileContents.Split("\n"[0]);
    }

    private void getNextCircuit()
    { 
        var count = PlayerPrefs.GetInt("Training");
        bool first = false;
        var name = "";
        Debug.Log(lines[count]);
        var positions = lines[count].Split(";"[0]);
        
        for (int i = 0; i< positions.Length; i++)
        {
            if (i == positions.Length - 1)//Quitando el ultimo carácter no válido de la cadena
            {
                var arrayChars = positions[i].ToCharArray();
                var res = "";
                for (int j = 0; j < arrayChars.Length - 1; j++)
                    res += arrayChars[j];

                var cell = grid.GetCell(res);
                cell.SetSelected(true);

                name += res ;

                //Crear meta
                var position = cell.GetPosition;

                float offset = 0;

                if (position.y % 2 != 0.0f)
                    offset = 1.75f / 2f;
                float x = position.x * 1.75f + offset;
                float y = position.y * 2f * 0.75f;

                wayPoints.Add(Instantiate(meta,new Vector3(x, 0, y), Quaternion.Euler(new Vector3(-90, 0, 0))));

                //Suscribo al evento de meta
                TrainingFinishEvent.onFinishTraining += FinalizadaVuelta;

                wayPoints.Add(Instantiate(metaCollider, new Vector3(x, 0, y), Quaternion.Euler(new Vector3(-90, 0, 0))));
            }
            else
            {
                name += positions[i] + ";";
                var cell = grid.GetCell(positions[i]);
                cell.SetSelected(true);
                if (i == 1) manager.SetPointToView(cell.transform.position);

                //Crear waypoints
                if (i != 0 && i % 3 == 0)
                {
                    var position = cell.GetPosition;

                    float offset = 0;

                    if (position.y % 2 != 0.0f)
                        offset = 1.75f / 2f;
                    float x = position.x * 1.75f + offset;
                    float y = position.y * 2f * 0.75f;

                    wayPoints.Add(Instantiate(wayPoint, new Vector3(x, 0, y), Quaternion.Euler(new Vector3(-90, 0, 0))));
                }

            }
        }
        
        manager.FinishedTrainingPath();
        manager.Respawn();
        

        GameObject.FindObjectOfType<Brain>().currentAleatoryCircuitName = name;
    }

    private void FinalizadaVuelta()
    {
        Debug.Log("Terminada la vuelta");

        foreach (var obj in wayPoints)
            Destroy(obj);

        wayPoints.Clear();

        TrainingFinishEvent.onFinishTraining -= FinalizadaVuelta;

        manager.KillANN();
        manager.Clear();
        grid.ResetPath();

        PlayerPrefs.SetInt("Training", PlayerPrefs.GetInt("Training") + 1);

        Invoke(nameof(getNextCircuit), 10f); 
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

        wayPoints.Add(Instantiate(wayPoint, new Vector3(x, 0, y), Quaternion.Euler(new Vector3(-90, 0, 0))));
    }

    public void ClearWayPoints()
    {
        foreach (var wp in wayPoints)
            Destroy(wp);
        wayPoints.Clear();
    }
}