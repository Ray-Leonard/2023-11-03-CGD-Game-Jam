using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class TunnelSegment : MonoBehaviour
{
    [SerializeField] private GameObject holeColliderPrefab;

    public Transform MakeHole()
    {
        // choose a random child and disable that
        Transform hole = transform.GetChild(Random.Range(0, transform.childCount));
        hole.GetComponent<MeshRenderer>().enabled = false;
        hole.GetComponent<Collider>().enabled = false;

        // add a trigger collider to that hole
        TunnelHole tunnelHoleScript = Instantiate(holeColliderPrefab, hole).GetComponent<TunnelHole>();
        // assign current parent to hole script
        tunnelHoleScript.tunnelParent = transform.parent.GetComponent<TunnelParent>();
        tunnelHoleScript.transform.position += tunnelHoleScript.transform.forward;

        // then return the transform of that quad
        return hole;
    }


}
