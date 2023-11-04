using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelRotationControl : MonoBehaviour
{
    [SerializeField] float rotationStep;
    [SerializeField] float rotationSmoothFactor;

    private TunnelGenerator tunnelGenerator;
    private float targetRotationAngle = 0;

    private void Awake()
    {
        tunnelGenerator = GetComponent<TunnelGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        Transform endWallCenterTile = tunnelGenerator.CurrentTunnelParent.endWall.CenterTile;

        if (!PlayerControl3d.Instance.IsGrounded)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                transform.RotateAround(endWallCenterTile.position, endWallCenterTile.forward, rotationStep);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                transform.RotateAround(endWallCenterTile.position, endWallCenterTile.forward, -rotationStep);
            }
        }


        // apply rotation
    }
}
