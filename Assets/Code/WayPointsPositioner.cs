using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WayPointsPositioner : MonoBehaviour
{
    [SerializeField] private GameObject modularTrackContainer;
    [SerializeField] private GameObject wayPoint;
    public MeshCollider[] modularTracks;
    string meta = "Meta";

    void Start()
    {
        modularTracks = modularTrackContainer.GetComponentsInChildren<MeshCollider>();

        foreach (var piece in modularTracks)
        {

            var wayPt = Instantiate(wayPoint, piece.transform.position, Quaternion.identity, piece.transform);
            wayPt.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));

            if (piece.name.Contains(meta))
            {
                //Es meta
                var script = wayPt.AddComponent<ID>();
                script.tipo = tipoCheckpoint.Meta;
            }
        }
            
    }
}
