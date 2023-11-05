using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TunnelParent : MonoBehaviour
{
    public List<TunnelSegment> segments = new();
    public TunnelEndWall endWall;
    public Transform hole;

    public Transform cam;

    public Action OnTunnelEnter;
    public UnityEvent enterEvent;

    private void Awake()
    {
        //pick random 
        enterEvent = new UnityEvent();
    }

    public void ActivateAction()
    {
        //OnTunnelEnter?.Invoke();
        enterEvent.Invoke();
    }

    public void OnDestroy()
    {
        if(cam != null)
            Destroy(cam.gameObject);
    }

}
