using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TunnelGenerator : SingletonMonoBehaviour<TunnelGenerator>
{
    [Header("Tunnel Prefabs")]
    [SerializeField] private GameObject tunnelSegmentPrefab;
    [SerializeField] private GameObject tunnelEndWallPrefab;

    [Header("Generation Rules")]
    [SerializeField] private float tunnelSegmentLength;
    [SerializeField] private int tunnelLengthMin;
    [SerializeField] private int tunnelLengthMax;

    [SerializeField] private Transform currentHole;

    public GameObject[] interactables;
    public GameObject holeSign;

    private Queue<TunnelParent> tunnelParentQueue = new Queue<TunnelParent>();

    private void Start()
    {
        // generate first tunnel.
        tunnelParentQueue.Enqueue(GenerateTunnel());
        // assign first reference to player
        TunnelParent currentTunnel = tunnelParentQueue.Peek();
        PlayerControl3d.Instance.CurrentTunnel = currentTunnel;
        // generate second tunnel
        tunnelParentQueue.Enqueue(GenerateTunnel());
    }


    [ContextMenu("Generate Tunnel")]
    private TunnelParent GenerateTunnel()
    {
        /// generate a tunnel parent to hold the tunnel, at world origin
        Transform tunnelParent = new GameObject().transform;
        // configure the tunnel parent's original position/rotation
        tunnelParent.position = Vector3.zero;
        tunnelParent.rotation = Quaternion.identity;
        tunnelParent.name = "tunnel";
        tunnelParent.parent = transform;
        TunnelParent tunnelParentScript = tunnelParent.AddComponent<TunnelParent>();



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

            // add reference
            tunnelParentScript.segments.Add(segmentTransform.GetComponent<TunnelSegment>());

            

            if(interactables.Length!=0){
                //randomly place item
                float randomN = Random.Range(0f, 100f);
                if (randomN <= 10f) //10% chance of generate item
                {
                    int randomIndex = Random.Range(0, interactables.Length);
                    int randomPlaneIndex = Random.Range(0, 11);
                    Transform plane = segmentTransform.GetChild(randomPlaneIndex);
                    Vector3 localYAxis = plane.forward;
                    Vector3 newPosition = plane.position - localYAxis * 2f;
                    GameObject newItem = Instantiate(interactables[randomIndex], newPosition, Quaternion.identity, plane);
                }
            }


        }



        // generate end wall after the loop
        Transform tunnelEndWall = Instantiate(tunnelEndWallPrefab,tunnelParent).transform;
        tunnelEndWall.position = new Vector3(0, 0, segmentCount * tunnelSegmentLength - 0.5f * tunnelSegmentLength);
        // add reference
        tunnelParentScript.endWall = tunnelEndWall.GetComponent<TunnelEndWall>();



        /// configure tunnel parent's new position/rotation so this tunnel sticks to the last tunnel's hole.
        if(currentHole != null)
        {
            tunnelParent.position = currentHole.position - (currentHole.up * tunnelSegmentLength); //currentHole.up*2f centralizes hole with new tunnel
            tunnelParent.rotation = currentHole.rotation;
            // fine tune the position so it connects with the hole seamlessly
            tunnelParent.position += currentHole.rotation * new Vector3(0, -0.5f * tunnelSegmentLength, 0.5f * tunnelSegmentLength);
        }



        /// Make a hole on the tunnel
        // generate the index where the whole should appear, but only should be around the last 30% of the tunnel
        int holeSegmentIndex = Random.Range(Mathf.RoundToInt(0.7f * segmentCount), segmentCount);
        TunnelSegment tunnelSegment = segmentTransformList[holeSegmentIndex].GetComponent<TunnelSegment>();
        currentHole = tunnelSegment.MakeHole();
        // record the hole
        tunnelParentScript.hole = currentHole;

        
        if(holeSign!=null){
            //add a sign to the hole
            Vector3 localZ = currentHole.forward;
            Vector3 newPosition = currentHole.position - localZ * 2f;
            Instantiate(holeSign, newPosition, Quaternion.identity, currentHole);
        }

        return tunnelParentScript;
    }


    public TunnelParent GenerateNextTunnel()
    {
        // destroy the previous one
        Destroy(tunnelParentQueue.Dequeue().gameObject, 1f);

        // generate next one and enqueue
        tunnelParentQueue.Enqueue(GenerateTunnel());

        // return the top of queue
        return tunnelParentQueue.Peek();
    }

}
