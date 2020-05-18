using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingFinishEvent : MonoBehaviour
{
    public static Action onFinishTraining;

    private bool IN = false;
    private int count = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (IN) return;

        if (other.GetComponent<Brain>() != null)
        {
            count++;
            IN = true;
            if (count == 1)
            {
                onFinishTraining();
                count = 0;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IN) return;
        IN = false;
    }
}
