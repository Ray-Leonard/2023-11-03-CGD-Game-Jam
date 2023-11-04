using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl3d : SingletonMonoBehaviour<PlayerControl3d>
{
    [Header("Movement")]
    [SerializeField] private float forwardSpeed;
    [SerializeField] private float horizontalStep;
    [SerializeField] private float horizontalSmoothFactor;
    // for snapping to the lane
    private float targetXPos = 0;
    private float centerXPos;


    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private string groundLayerName;
    private bool isGrounded;
    public bool IsGrounded { get { return isGrounded; } }
    private Rigidbody rb;


    // tunnel reference
    private TunnelParent currentTunnel;
    public TunnelParent CurrentTunnel { get => currentTunnel; set => currentTunnel = value; }
    public event EventHandler<SwitchTunnelEventArgs> OnSwitchTunnel;
    public class SwitchTunnelEventArgs : EventArgs
    {
        public Transform _hole;
    }


    [Header("Zone Settings")]
    public bool canSwitchLanes = true;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        HandleMovement();
        HandleJump();
    }

    private void HandleMovement()
    {
        // move forward
        transform.position += new Vector3(0, 0, forwardSpeed * Time.deltaTime);


        if (isGrounded && canSwitchLanes)

        {
            // switch lane
            if (Input.GetKeyDown(KeyCode.A))
            {
                targetXPos -= horizontalStep;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                targetXPos += horizontalStep;
            }
        }



        // check if targetXPos is out of range
        // 0.1f is to account for float point calculation loose of accuracy.
        if (targetXPos < centerXPos - horizontalStep - 0.1f)
        {
            targetXPos = centerXPos - horizontalStep;
        }
        else if (targetXPos > centerXPos + horizontalStep + 0.1f)
        {
            targetXPos = centerXPos + horizontalStep;
        }

        // apply lane change movement
        float currXPos = Mathf.Lerp(transform.position.x, targetXPos, horizontalSmoothFactor * Time.deltaTime);
        transform.position = new Vector3(currXPos, transform.position.y, transform.position.z);
    }

    private void HandleJump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.VelocityChange);
            isGrounded = false;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer(groundLayerName))
        {
            isGrounded = true;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.TryGetComponent(out TunnelHole tunnelHole))
        {
            // trigger transition
            OnSwitchTunnel?.Invoke(this, new SwitchTunnelEventArgs { _hole = tunnelHole.transform.parent});

            // destroy the collider so it does not trigger multiple times
            Destroy(tunnelHole.gameObject);

            // advance current parent to next parent
            currentTunnel = TunnelGenerator.Instance.GenerateNextTunnel();

            // correction of centerXPos
            centerXPos = currentTunnel.transform.position.x;
            targetXPos = centerXPos;
        }
    }
}
