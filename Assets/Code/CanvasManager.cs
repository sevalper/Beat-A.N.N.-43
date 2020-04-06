using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{


    public void NewScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
