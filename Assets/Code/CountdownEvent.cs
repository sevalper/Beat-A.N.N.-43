using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountdownEvent : MonoBehaviour
{
    [SerializeField] private GameObject[] items;
    [SerializeField] private float time;
    [SerializeField] private ManagerCircuits manager;

    private int count;


    private void Start()
    {
        count = 0;
        InvokeRepeating(nameof(NextItem), time, time);
    }

    private void NextItem()
    {
        if (count == 0)
        {
            items[count].SetActive(true);
        }
        else if (count < 4)
        {
            items[count].SetActive(true);
            items[count - 1].SetActive(false);
        }
        else
        {
            items[count - 1].SetActive(false);
            manager.GO();
            CancelInvoke(nameof(NextItem));
        }
        count++;

    }
}
