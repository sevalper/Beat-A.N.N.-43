using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallState : MonoBehaviour {

    public bool dropped = false;
    public bool point = false;
    public bool meta = false;

	void OnCollisionEnter(Collision col)
	{
		if(col.gameObject.tag == "terrain")
		{
			dropped = true;
            
		}

        

    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "point")
        {
            ID id = col.gameObject.GetComponent<ID>();
            if (id != null && id.tipo == tipoCheckpoint.Meta) { meta = true; }
            point = true;
            

        }

        
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "point")
        {
            point = false;
        }
    }
}
