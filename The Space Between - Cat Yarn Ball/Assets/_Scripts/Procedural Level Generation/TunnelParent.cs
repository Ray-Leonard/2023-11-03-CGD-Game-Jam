using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelParent : MonoBehaviour
{
    public List<TunnelSegment> segments = new();
    public TunnelEndWall endWall;
}
