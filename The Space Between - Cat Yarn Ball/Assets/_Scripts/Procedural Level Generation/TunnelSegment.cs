using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class TunnelSegment : MonoBehaviour
{

    public Transform MakeHole()
    {
        // choose a random child and disable that
        Transform hole = transform.GetChild(Random.Range(0, transform.childCount));
        hole.gameObject.SetActive(false);
        // then return the transform of that quad
        return hole;
    }
}
