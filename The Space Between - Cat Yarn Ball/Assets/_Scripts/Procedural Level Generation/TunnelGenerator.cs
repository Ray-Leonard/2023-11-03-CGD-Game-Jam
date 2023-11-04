using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelGenerator : MonoBehaviour
{
    [Header("Tunnel Prefabs")]
    [SerializeField] private GameObject tunnelSegmentPrefab;
    [SerializeField] private GameObject tunnelEndWallPrefab;

    [Header("Generation Rules")]
    [SerializeField] private float tunnelSegmentLength;
    [SerializeField] private int tunnelLengthMin;
    [SerializeField] private int tunnelLengthMax;

    [SerializeField] private Transform currentHole;
    [SerializeField] private Transform currentTunnelParent;

    [ContextMenu("Generate tunnel")]
    private void GenerateTunnel()
    {
        /// generate a tunnel parent to hold the tunnel, at world origin
        Transform tunnelParent = new GameObject().transform;
        // configure the tunnel parent's original position/rotation
        tunnelParent.position = Vector3.zero;
        tunnelParent.rotation = Quaternion.identity;
        tunnelParent.name = "tunnel";
        tunnelParent.parent = transform;
        currentTunnelParent = tunnelParent;

        /// tunnel generation
        // generate random length of the tunnel
        int segmentCount = Random.Range(tunnelLengthMin, tunnelLengthMax);
        // make a list to hold the tunnel segments
        List<Transform> segmentTransformList = new List<Transform>();
        // generate tunnel segments to make a tunnel
        for (int i = 0; i < segmentCount; ++i)
        {
            Transform segmentTransform = Instantiate(tunnelSegmentPrefab, tunnelParent).transform;
            segmentTransform.position = new Vector3(0, 0, tunnelSegmentLength * i);
            
            segmentTransformList.Add(segmentTransform);
        }

        // generate end wall after the loop
        Transform tunnelEndWall = Instantiate(tunnelEndWallPrefab,tunnelParent).transform;
        tunnelEndWall.position = new Vector3(0, 0, segmentCount * tunnelSegmentLength - 0.5f * tunnelSegmentLength);


        /// configure tunnel parent's new position/rotation so this tunnel sticks to the last tunnel's hole.
        if(currentHole != null)
        {
            tunnelParent.position = currentHole.position;
            tunnelParent.rotation = currentHole.rotation;
            // fine tune the position so it connects with the hole seamlessly
            tunnelParent.position += currentHole.rotation * new Vector3(0, -0.5f * tunnelSegmentLength, 0.5f * tunnelSegmentLength);
        }


        /// Make a hole on the tunnel
        // generate the index where the whole should appear, but only should be around the last 30% of the tunnel
        int holeSegmentIndex = Random.Range(Mathf.RoundToInt(0.7f * segmentCount), segmentCount);
        TunnelSegment tunnelSegment = segmentTransformList[holeSegmentIndex].GetComponent<TunnelSegment>();
        currentHole = tunnelSegment.MakeHole();
    }
}
