using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerMainMenu : MonoBehaviour
{ 
    [SerializeField] private Animator animatorMenuButt;
    [SerializeField] private Animator animatorStartButt;
    [SerializeField] private GameObject poplar;
    [SerializeField] private Flowchart flowchart;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("START"))
        {
            flowchart.ExecuteBlock("Intro");
            PlayerPrefs.SetInt("START",1);
            
        }
        else
        {
            PlayerPrefs.SetInt("START",PlayerPrefs.GetInt("START")+1);
            OnFinishFungus();
        }

        Time.timeScale = 1.0f;
    }

    [ContextMenu("DeletePlayerPrefs")]
    public void Delete() { PlayerPrefs.DeleteKey("START"); }

    public void OnFinishFungus()
    {
        poplar.SetActive(true);
        animatorMenuButt.SetBool("Start", true);
    }

    public void OnClickStart()
    {
        animatorMenuButt.SetBool("Finished", true);
        animatorStartButt.SetBool("Initied", true);
        animatorStartButt.SetBool("Finish", false);
    }

    public void OnClickBack()
    {
        animatorMenuButt.SetBool("Finished", false);
        animatorStartButt.SetBool("Initied", false);
        animatorStartButt.SetBool("Finish", true);
    }

    public void OnClickGameMode(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
