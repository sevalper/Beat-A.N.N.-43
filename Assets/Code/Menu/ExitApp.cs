using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitApp : MonoBehaviour
{
    public void AppQuit()
    {
        if (!Application.isEditor)
            Application.Quit();
        else
            UnityEditor.EditorApplication.isPlaying = false;

    }
}
