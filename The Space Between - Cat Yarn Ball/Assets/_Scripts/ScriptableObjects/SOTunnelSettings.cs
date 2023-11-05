using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class SOTunnelSettings : ScriptableObject
{
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
