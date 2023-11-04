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

    private void Start()
    {
        playerControl3d = PlayerControl3d.Instance;
        playerControl3d.OnSwitchTunnel += PlayerControl3d_OnSwitchTunnel;
    }


    // Update is called once per frame
    void Update()
    {
        HandleInput();
        HandleInTunnelRotation();
        HandleSwitchTunnelRotation();
    }


    private void HandleInput()
    {

        if (!playerControl3d.IsGrounded)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                inTunnelRotationDelta += rotationStep;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                inTunnelRotationDelta -= rotationStep;
            }
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
}
