using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelEndWall : MonoBehaviour
{
    [SerializeField] Transform centerTile;
    public Transform CenterTile { get => centerTile; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StatManager.instance.death();
        }
    }

}
