using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPos
{
    public string name;
    public int pos;

    public FinishPos(string n, int p) { name = n; pos = p; }
}

public class Meta : MonoBehaviour
{
    private List<FinishPos> positions = new List<FinishPos>();
    public int bots;
    public ManagerCircuits manager;
    public ManagerCanvas manCanvas;

    public Flowchart flowchart; 

    private int count = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Brain>() != null || other.GetComponent<PlayerMovement>() != null)
        {
            count++;
            positions.Add(new FinishPos(other.gameObject.name, count));
        }
        if (count == 4)
        {
            manCanvas.Finish(positions);

            if (Win())
                flowchart.ExecuteBlock("WIN");
            else
                flowchart.ExecuteBlock("LOSE");
            

            manager.STOP();
        }
    }

    private bool Win()
    {
        foreach (var p in positions)
            if (p.name == "ANNKart" && p.pos == 1)
                return true;
        return false;
    }
}
