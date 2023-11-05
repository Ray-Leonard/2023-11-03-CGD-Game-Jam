using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TunnelGenerator : SingletonMonoBehaviour<TunnelGenerator>
{

    [Header("Generation Rules")]
    [SerializeField] private SOTunnelSettings baseTunnelSettings;
    [SerializeField] private SOTunnelSettings[] multiTunnelSettings;
    [SerializeField] private Transform currentHole;

    private Queue<TunnelParent> tunnelParentQueue = new Queue<TunnelParent>();

    private int tunnelIndex = 0;

    private void Start()
    {
        // generate first tunnel.
        tunnelParentQueue.Enqueue(GenerateTunnel(GetNextTunnelSettings()));
        // assign first reference to player
        TunnelParent currentTunnel = tunnelParentQueue.Peek();
        PlayerControl3d.Instance.CurrentTunnel = currentTunnel;
        // generate second tunnel
        tunnelParentQueue.Enqueue(GenerateTunnel(GetNextTunnelSettings()));
    }


    [ContextMenu("Generate Tunnel")]
    private TunnelParent GenerateTunnel(SOTunnelSettings tunnelSettings)
    {
        float segmentLength = tunnelSettings.TunnelSegmentLength;
        int lengthMin = tunnelSettings.TunnelLengthMin;
        int lengthMax = tunnelSettings.TunnelLengthMax;


        /// generate a tunnel parent to hold the tunnel, at world origin
        Transform tunnelParent = new GameObject().transform;
        // configure the tunnel parent's original position/rotation
        tunnelParent.position = Vector3.zero;
        tunnelParent.rotation = Quaternion.identity;
        tunnelParent.name = "tunnel " + tunnelIndex;
        tunnelParent.parent = transform;
        TunnelParent tunnelParentScript = tunnelParent.AddComponent<TunnelParent>();



        /// tunnel generation
        // generate random length of the tunnel
        int segmentCount = Random.Range(lengthMin, lengthMax);
        // generate tunnel segments to make a tunnel
        for (int i = 0; i < segmentCount; ++i)
        {
            Transform segmentTransform = Instantiate(tunnelSettings.TunnelSegmentPrefab, tunnelParent).transform;
            segmentTransform.position = new Vector3(0, 0, segmentLength * i);

            // add reference
            tunnelParentScript.segments.Add(segmentTransform.GetComponent<TunnelSegment>());
        }

	

        // Generate Camera
        if(tunnelSettings.cinemachineVirtualCamera != null)
        {
            /*
            tunnelParentScript.enterEvent.AddListener(() =>
            {
                Debug.LogError(string.Format("Enter tunnel {0}", tunnelParent.name), tunnelParentScript.transform);
            });
            */

            // Add Spawn Camera Event
            
            
            tunnelParentScript.enterEvent.AddListener(() =>
            {

                Transform player = PlayerControl3d.Instance.transform;
                Transform camPos = player.GetComponentInChildren<CamPos>().transform;

                CinemachineVirtualCamera virtualCamera = Instantiate(tunnelSettings.cinemachineVirtualCamera, null);
                virtualCamera.transform.localPosition = Vector3.zero;
                //virtualCamera.transform.rotation = camPos.rotation;
                //CinemachineVirtualCamera virtualCamera = Instantiate(tunnelSettings.cinemachineVirtualCamera, camPos.position, camPos.rotation, null);
                //CinemachineVirtualCamera virtualCamera = Instantiate(tunnelSettings.cinemachineVirtualCamera, camPos.transform);

                tunnelParentScript.cam = virtualCamera.transform;

                Debug.LogError("Activate Camera", virtualCamera.transform);

                virtualCamera.LookAt = player;
                virtualCamera.Follow = player;
            });
        }

        // generate end wall after the loop
        Transform tunnelEndWall = Instantiate(tunnelSettings.TunnelEndWallPrefab, tunnelParent).transform;
        tunnelEndWall.position = new Vector3(0, 0, segmentCount * segmentLength - 0.5f * segmentLength);
        // add reference
        tunnelParentScript.endWall = tunnelEndWall.GetComponent<TunnelEndWall>();



        /// configure tunnel parent's new position/rotation so this tunnel sticks to the last tunnel's hole.
        if(currentHole != null)
        {
            tunnelParent.position = currentHole.position - (currentHole.up * segmentLength); //currentHole.up*2f centralizes hole with new tunnel
            tunnelParent.rotation = currentHole.rotation;
            // fine tune the position so it connects with the hole seamlessly
            tunnelParent.position += currentHole.rotation * new Vector3(0, -0.5f * segmentLength, 0.5f * segmentLength);
        }


        /// Make a hole on the tunnel


        // Big Hole

        int holeSegmentIndex = 0;
        if (tunnelSettings.MakeBigHole) //Make big Hole
        {
            holeSegmentIndex = Random.Range(Mathf.RoundToInt(0.7f * segmentCount), segmentCount);
            int numRows = Mathf.Min(2, segmentCount - holeSegmentIndex);

            TunnelSegment tunnelSegment = tunnelParentScript.segments[holeSegmentIndex];
            currentHole = tunnelSegment.MakeBigHole();
            // record the hole
            tunnelParentScript.hole = currentHole;

            // previous tile
            tunnelParentScript.segments[holeSegmentIndex -1].GetComponent<TunnelSegment>().JustEraseTiles();
            if(holeSegmentIndex+1 < segmentCount)
                tunnelParentScript.segments[holeSegmentIndex + 1].GetComponent<TunnelSegment>().JustEraseTiles();
        }
        else
        {
            // Small Hole
            // generate the index where the whole should appear, but only should be around the last 30% and not the last tile of the tunnel
            holeSegmentIndex = Random.Range(Mathf.RoundToInt(0.7f * segmentCount), segmentCount - 1);
            TunnelSegment tunnelSegment = tunnelParentScript.segments[holeSegmentIndex];
            currentHole = tunnelSegment.MakeHole();
            // record the hole
            tunnelParentScript.hole = currentHole;
        }

        /// generate traps and collectiables
        // traps and collectiables should only appear from 30% of the segmentCount before the holeSegmentIndex
        int itemGenerationStartIndex = Mathf.RoundToInt(0.3f * segmentCount);
        int itemGenerationEndIndex = holeSegmentIndex - 1;
        int yarnBallGenerationIndex = Random.Range(itemGenerationStartIndex, itemGenerationEndIndex + 1);
        for (int i = itemGenerationStartIndex; i < itemGenerationEndIndex; i++)
        {
            if (StatManager.Instance.CanGenerateYarnBall() && i == yarnBallGenerationIndex)
            {
                tunnelParentScript.segments[i].GenerateYarnBall();
            }
            else
            {
                tunnelParentScript.segments[i].GenerateTrap();
            }
        }

        /* Hole sign
        if (holeSign!=null){
            //add a sign to the hole
            Vector3 localZ = currentHole.forward;
            Vector3 newPosition = currentHole.position - localZ * 2f;
            Instantiate(holeSign, newPosition, currentHole.rotation, currentHole);
        }
        /*/


        return tunnelParentScript;
    }


    public TunnelParent GenerateNextTunnel()
    {
        // destroy the previous one
        Destroy(tunnelParentQueue.Dequeue().gameObject, 1f);

        // generate next one and enqueue
        tunnelParentQueue.Enqueue(GenerateTunnel(GetNextTunnelSettings()));

        // return the top of queue
        return tunnelParentQueue.Peek();
    }

    private SOTunnelSettings GetNextTunnelSettings()
    {
        //return baseTunnelSettings;

        var tunnel = multiTunnelSettings[tunnelIndex];

        tunnelIndex = (tunnelIndex + 1) % multiTunnelSettings.Length;

        return tunnel;
    }

}
