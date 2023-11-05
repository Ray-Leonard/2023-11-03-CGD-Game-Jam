using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelRotationControl : MonoBehaviour
{
    [SerializeField] float rotationStep;
    [SerializeField] float rotationSpeed;

    private float inTunnelRotationDelta = 0;
    private float switchTunnelRotationDelta = 0;
    private Vector3 switchTunnelRotationPos;
    private Vector3 switchTunnelRotationRight;

    private PlayerControl3d playerControl3d;

    private bool isLock;

    private void Start()
    {
        playerControl3d = PlayerControl3d.Instance;
        playerControl3d.OnSwitchTunnel += PlayerControl3d_OnSwitchTunnel;

        StatManager.Instance.OnPlayerDead += LockInput;
    }

    private void OnDestroy()
    {
        playerControl3d.OnSwitchTunnel -= PlayerControl3d_OnSwitchTunnel;
        StatManager.Instance.OnPlayerDead -= LockInput;
    }


    // Update is called once per frame
    void Update()
    {
        if (!isLock)
        {
            HandleInput();
            HandleInTunnelRotation();
            HandleSwitchTunnelRotation();
        }
    }


    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            inTunnelRotationDelta += rotationStep;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            inTunnelRotationDelta -= rotationStep;
        }
    }

    private void HandleInTunnelRotation()
    {
        if(playerControl3d.CurrentTunnel != null)
        {
            Transform endWallCenterTile = playerControl3d.CurrentTunnel.endWall.CenterTile;
            float frameRotationStep = rotationSpeed * Time.deltaTime;
            if (Mathf.Abs(inTunnelRotationDelta) > frameRotationStep)
            {
                frameRotationStep = inTunnelRotationDelta > 0 ? frameRotationStep : -frameRotationStep;
                // apply rotation
                transform.RotateAround(endWallCenterTile.position, endWallCenterTile.forward, frameRotationStep);

                //  decrease targetRotationDelta by the amount we just rotated
                inTunnelRotationDelta -= frameRotationStep;
            }
        }
    }


    private void PlayerControl3d_OnSwitchTunnel(object sender, PlayerControl3d.SwitchTunnelEventArgs e)
    {
        switchTunnelRotationDelta -= rotationStep;
        switchTunnelRotationPos = e._holePos;
        switchTunnelRotationRight = e._holeRight;
    }

    private void HandleSwitchTunnelRotation()
    {
        float frameRotationStep = rotationSpeed * Time.deltaTime;
        if (Mathf.Abs(switchTunnelRotationDelta) > frameRotationStep)
        {
            frameRotationStep = switchTunnelRotationDelta > 0 ? frameRotationStep : -frameRotationStep;
            // apply rotation
            transform.RotateAround(switchTunnelRotationPos, switchTunnelRotationRight, frameRotationStep);

            //  decrease targetRotationDelta by the amount we just rotated
            switchTunnelRotationDelta -= frameRotationStep;
        }
    }


    private void LockInput()
    {
        isLock = true;
    }
}
