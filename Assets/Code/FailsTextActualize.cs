using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FailsTextActualize : MonoBehaviour
{
    [SerializeField] private Text failText;

    private void OnEnable()
    {
        Brain.onFail += ActualizeText;
    }

    private void OnDisable()
    {
        Brain.onFail -= ActualizeText;
    }

    private void ActualizeText(int fail)
    {
        failText.text = "Fails: " + fail;
    }
}
