using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private Flowchart flowchart;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("RaceMode"))
        {
            flowchart.ExecuteBlock("Intro");
            PlayerPrefs.SetInt("RaceMode", 1);

        }
        else
        {
            PlayerPrefs.SetInt("RaceMode", PlayerPrefs.GetInt("RaceMode") + 1);
            ChangeRanddomScene();
        }
    }

    [ContextMenu("DeletePlayerPrefs")]
    public void DeletePlayerPrefs() { PlayerPrefs.DeleteKey("RaceMode"); }

    public void ChangeRanddomScene()
    {
        SceneManager.LoadScene(Random.Range(2, 6));
    }
}
