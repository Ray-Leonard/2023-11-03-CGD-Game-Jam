using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelRotationControl : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(!PlayerControl3d.Instance.IsGrounded)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {

            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {

            }
        }
    }
}
