using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelRotationControl : MonoBehaviour
{
    [SerializeField] float rotationStep;
    [SerializeField] float rotationSpeed;

    private float targetRotationDelta = 0;

    private PlayerControl3d playerControl3d;

    private void Start()
    {
        playerControl3d = PlayerControl3d.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        Transform endWallCenterTile = playerControl3d.CurrentTunnel.endWall.CenterTile;

        if (!playerControl3d.IsGrounded)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                targetRotationDelta += rotationStep;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                targetRotationDelta -= rotationStep;
            }
        }

        if(!Mathf.Approximately(targetRotationDelta, 0f))
        {
            float rotationAmount = (targetRotationDelta > 0 ? rotationSpeed : -rotationSpeed) * Time.deltaTime;
            // apply rotation
            transform.RotateAround(endWallCenterTile.position, endWallCenterTile.forward, rotationAmount);

            //  decrease targetRotationDelta by the amount we just rotated
            targetRotationDelta -= rotationAmount;
        }
    }
}
