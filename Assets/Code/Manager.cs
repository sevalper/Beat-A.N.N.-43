using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static event Action FinishPath;

    [SerializeField] private SelectionAction action;
    [SerializeField] private Grid grid;
    [SerializeField] private GameObject bot;
    [SerializeField] private Vector3 pos;
    [SerializeField] private GameObject meta;
    [SerializeField] private GameObject ResumeMenuContainer;
    [SerializeField] private Cinemachine.CinemachineVirtualCamera followCamera, zenitalCamera;
    [SerializeField] private Cinemachine.CinemachineBrain cameraBrain;

    public Cinemachine.CinemachineBlendDefinition cameraDefinition;

    private int _initStartSelectedTimes;
    private bool _isFinished;
    private GameObject _currentBot;
    private GameObject _meta;
    private Vector3 _pointToView;

    private string circuitName;
    private List<float> _circuitPoints;

    Dictionary<string, string> _circuitsNameDictionary = new Dictionary<string, string>();
    static readonly string DICTIONARY = "Circuits";

    public bool GetFinished() => _isFinished;
    public void Clear() { _initStartSelectedTimes = 0; _isFinished = false; grid.SetInit(); Destroy(_meta); circuitName = ""; _circuitPoints = new List<float>(); }

    private void Start()
    {
        followCamera.enabled = false;
        zenitalCamera.enabled = true;

        cameraBrain.m_DefaultBlend = cameraDefinition;

        if (SaveAndLoad.SaveExists(DICTIONARY))
            _circuitsNameDictionary = SaveAndLoad.Load<Dictionary<string, string>>(DICTIONARY);
    }

    public void SaveNewDataDictionary(string name, string data)
    {
        if (!GetNameDictionary(name))
        {
            _circuitsNameDictionary.Add(name, data);
            SaveAndLoad.Save<Dictionary<string, string>>(_circuitsNameDictionary, DICTIONARY);
            Debug.Log("primera vez");
        }/*
        else
        {
            _circuitsNameDictionary.Remove(name);
            SaveAndLoad.Save<Dictionary<string, string>>(_circuitsNameDictionary, DICTIONARY);
            Debug.Log("otras");
        }*/
    }

    public bool GetDictionaryCreate()
    {
        return SaveAndLoad.SaveExists(DICTIONARY);
    }

    public string GetDataDictionary(string name)
    {
        string newValue;
        _circuitsNameDictionary.TryGetValue(name, out newValue);
        return newValue;
    }

    public bool GetNameDictionary(string name)
    {
        foreach (string a in _circuitsNameDictionary.Keys)
        {
            if (name == a)
                return true;
        }
        return false;
    }

    private void OnEnable()
    {
        Cell.StartCellNeighbourSelected += StartCellNeighbourSelected;
        Brain.Death += Respawn;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ResumeMenuContainer.SetActive(true);
            Time.timeScale = 0f;
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            if (_currentBot == null)
                return;

            followCamera.enabled = !followCamera.enabled;
            zenitalCamera.enabled = !zenitalCamera.enabled;

            if (followCamera.enabled)
                FollowInstance();
        }
    }

    public void AddCellToCircuit(string c)
    {
        circuitName += c + ";";
        float value;
        float.TryParse(c, out value);
        _circuitPoints.Add(value);
    }

    public void Resume()
    {
        Time.timeScale = 25f;
    }

    public void KillANN()
    {
        if (_currentBot != null) Destroy(_currentBot);
    }

    public void Respawn()
    {
        _pointToView.y = pos.y;
        var botInst = Instantiate(bot, pos, Quaternion.LookRotation(_pointToView - pos, Vector3.up));
        Brain brain = botInst.GetComponent<Brain>();
        brain._isAleatoryCircuit = true;
        brain.currentAleatoryCircuitName = circuitName;
        brain.pointsCircuitName = _circuitPoints;
        _currentBot = botInst;
        FollowInstance();
    }

    private void FollowInstance()
    {
        if (_currentBot == null)
            return;

        Vector3 newPosition = new Vector3(0f, 0.05f,-0.5f);

        var transposer = followCamera.GetCinemachineComponent<Cinemachine.CinemachineTransposer>();
        transposer.m_FollowOffset = newPosition;
        followCamera.m_Lens.FieldOfView = 37f;
        followCamera.m_Follow = _currentBot.transform;
        followCamera.m_LookAt = _currentBot.transform;
        
    }

    private void StartCellNeighbourSelected(Cell cellScript)
    {
        _initStartSelectedTimes++;
        if (_initStartSelectedTimes == 1)
            _pointToView = cellScript.transform.position;
        else if (_initStartSelectedTimes == 2)
        {
            var position = cellScript.GetPosition;

            float offset = 0;

            if (position.y % 2 != 0.0f)
                offset = 1.75f / 2f;
            float x = position.x * 1.75f + offset;
            float y = position.y * 2f * 0.75f;

            _meta = Instantiate(meta, new Vector3(x, 0, y), Quaternion.Euler(new Vector3(-90, 0, 0)));

            action.enabled = false;
            FinishPath();
            _isFinished = true;
        }
    }
}
