using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class TunnelParent : MonoBehaviour
{
    public List<TunnelSegment> segments = new();
    public TunnelEndWall endWall { get; set; }
    public Transform hole { get; set; }


    public Action OnTunnelEnter;
    public UnityEvent enterEvent;

    public UnityEvent exitEvent;

    public CinemachineVirtualCamera virtualCamera;

    private void Awake()
    {
        //pick random 
        enterEvent = new UnityEvent();
        exitEvent = new UnityEvent();
    }

    public void OnEnter()
    {
        //OnTunnelEnter?.Invoke();
        enterEvent?.Invoke();
    }

    public void OnExit()
    {
        //OnTunnelEnter?.Invoke();
        exitEvent?.Invoke();

        if(virtualCamera != null)
        {
            virtualCamera.Priority = 0;
        }

    }

    public void OnDestroy()
    {
        if(virtualCamera != null)
            Destroy(virtualCamera.gameObject);
    }

}
