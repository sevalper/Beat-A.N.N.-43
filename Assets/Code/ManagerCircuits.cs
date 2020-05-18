using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerCircuits : MonoBehaviour
{
    Dictionary<string, string> _circuitsNameDictionary = new Dictionary<string, string>();
    public string DICTIONARY;

    [SerializeField] private List<Brain> brains;
    [SerializeField] private PlayerMovement playerMov;

    public void GO()
    {
        foreach (var brain in brains)
            brain.enabled = true;
        playerMov.enabled = true;
    }

    public void STOP()
    {
        foreach (var brain in brains)
            brain.enabled = false;
        playerMov.enabled = false;
    }

    private void Awake()
    {
        if (SaveAndLoad.SaveExists(DICTIONARY))
            _circuitsNameDictionary = SaveAndLoad.Load<Dictionary<string, string>>(DICTIONARY);
    }

    private void OnEnable()
    {
        Time.timeScale = 10f;
    }

    public void SaveNewDataDictionary(string name, string data)
    {
        if (!GetNameDictionary(name))
        {
            _circuitsNameDictionary.Add(name, data);
            SaveAndLoad.Save<Dictionary<string, string>>(_circuitsNameDictionary, DICTIONARY);
        }
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
}
