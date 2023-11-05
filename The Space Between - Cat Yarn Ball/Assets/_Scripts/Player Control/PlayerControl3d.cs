using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl3d : SingletonMonoBehaviour<PlayerControl3d>
{
    [Header("Movement")]
    private float forwardSpeed;
    [SerializeField] private float minForwardSpeed;
    [SerializeField] private float maxForwardSpeed;
    [SerializeField] private float horizontalStep;
    [SerializeField] private float horizontalSmoothFactor;
    // for snapping to the lane
    private float targetXPos = 0;
    private float centerXPos;


    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private string groundLayerName;
    [SerializeField] private LayerMask groundLayerMask;
    private bool isGrounded;
    public bool IsGrounded { get { return isGrounded; } }
    private Rigidbody rb;

    // tunnel reference
    private TunnelParent currentTunnel;
    public TunnelParent CurrentTunnel { get => currentTunnel; set => currentTunnel = value; }
    public event EventHandler<SwitchTunnelEventArgs> OnSwitchTunnel;
    public class SwitchTunnelEventArgs : EventArgs
    {
        public Vector3 _holePos;
        public Vector3 _holeRight;
    }


    [Header("Controll Settings")]
    public PlayerControlSettings playerControlSettings;

    private bool isLock;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        StatManager.Instance.OnPlayerDead += LockInput;
    }

    private void OnDestroy()
    {
        StatManager.Instance.OnPlayerDead -= LockInput;
    }

    private void Update()
    {
        if(!isLock)
        {
            HandleMovementInput();
            HandleJump();
        }
    }

    private void FixedUpdate()
    {
        if (!isLock)
        {
            // update player speed
            forwardSpeed = minForwardSpeed + (maxForwardSpeed - minForwardSpeed) * StatManager.Instance.GetLevelDifficulty();
            HandleMovement();
        }

        HandleYPos();
    }

    private void HandleMovementInput()
    {
        if (playerControlSettings.canSwitchLanes)
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
    }


    private void HandleJump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.VelocityChange);
            isGrounded = false;
        }
    }


    private void HandleMovement()
    {
        // apply lane change movement
        float currXPos = Mathf.Lerp(rb.position.x, targetXPos, horizontalSmoothFactor * Time.fixedDeltaTime);

        rb.MovePosition(new Vector3(currXPos, rb.position.y, rb.position.z + forwardSpeed * Time.fixedDeltaTime));
    }

    private void HandleYPos()
    {

        //RaycastHit hitInfo;
        //// if not touching the ground
        //if (isGrounded && !Physics.Raycast(transform.position, Vector3.down, out hitInfo, Mathf.Infinity, groundLayerMask))
        //{
        //    //rb.MovePosition(new Vector3(rb.position.x, rb.position.y + forwardSpeed * Time.fixedDeltaTime, rb.position.z));

        //    transform.position += Vector3.up * Time.fixedDeltaTime;
        //}


        //Debug.DrawLine(transform.position, transform.position + Vector3.down * 10, Color.red);
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
            OnSwitchTunnel?.Invoke(this, new SwitchTunnelEventArgs { _holePos = currentTunnel.hole.position, _holeRight = currentTunnel.hole.right });

            // destroy the collider so it does not trigger multiple times
            Destroy(tunnelHole.gameObject);

            // Call exit Tunnel Actions
            currentTunnel.OnExit();

            // advance current parent to next parent
            currentTunnel = TunnelGenerator.Instance.GenerateNextTunnel();

            // Call Enter Tunnel Actions
            currentTunnel.OnEnter();

            // correction of centerXPos
            centerXPos = currentTunnel.transform.position.x;
            targetXPos = centerXPos;
        }
    }

    private void LockInput()
    {
        isLock = true;
    }
}

[System.Serializable]
public class PlayerControlSettings
{
    public bool canSwitchLanes;
    public bool canJump;
    public bool canRotateWorld;
    public bool canMoveFoward;
}
