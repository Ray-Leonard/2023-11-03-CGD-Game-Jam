using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SO_TunnelVisualSettings : ScriptableObject
{
    public Material cellingMaterial;
    public Material wallMaterial;
    public Material floorMaterial;

    [Space]

    public bool hasFog = true;
    public Color fogColor = Color.white;
}
