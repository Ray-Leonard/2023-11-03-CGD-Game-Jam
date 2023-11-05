using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SOTunnelSettings : ScriptableObject
{
    [SerializeField] private bool is2D = false;
    public bool Is2D { get { return is2D; } }

    [SerializeField] private bool makeBigHole = false;
    public bool MakeBigHole { get { return makeBigHole; } }

    [SerializeField] private bool makeHole = true;
    public bool MakeHole { get { return makeHole; } }

    [Space]
    public PlayerControlSettings playerControlSettings;
    [Space]

    [SerializeField] private GameObject tunnelSegmentPrefab;
    public GameObject TunnelSegmentPrefab { get { return tunnelSegmentPrefab; } }
    [SerializeField] private GameObject tunnelEndWallPrefab;
    public GameObject TunnelEndWallPrefab { get { return tunnelEndWallPrefab; } }

    [Space]

    [SerializeField] private int tunnelLengthMin = 20;
    public int TunnelLengthMin { get { return tunnelLengthMin;}}

    [SerializeField] private int tunnelLengthMax = 40;
    public int TunnelLengthMax { get { return tunnelLengthMax; } }
    [SerializeField] private float tunnelSegmentLength = 2;
    public float TunnelSegmentLength { get { return tunnelSegmentLength; } }
    [Space]
    public CinemachineVirtualCamera cinemachineVirtualCamera;

    public Vector3 camPos;
    public Vector3 camRot;
    public Quaternion camRot2;
}
