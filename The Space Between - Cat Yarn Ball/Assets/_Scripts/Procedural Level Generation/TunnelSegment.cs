using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class TunnelSegment : MonoBehaviour
{
    [SerializeField] private GameObject holeColliderPrefab;

    public Transform MakeHole()
    {
        // choose a random child and disable that
        Transform hole = transform.GetChild(Random.Range(0, transform.childCount));
        hole.GetComponent<MeshRenderer>().enabled = false;
        hole.GetComponent<Collider>().enabled = false;

        // add a trigger collider to that hole
        TunnelHole tunnelHoleScript = Instantiate(holeColliderPrefab, hole).GetComponent<TunnelHole>();
        // assign current parent to hole script
        tunnelHoleScript.tunnelParent = transform.parent.GetComponent<TunnelParent>();
        tunnelHoleScript.transform.position += tunnelHoleScript.transform.forward;

        // then return the transform of that quad
        return hole;
    }

    public Transform MakeBigHole()
    {
        Transform mainHole = transform.GetChild(1); ;

        // choose a random child and disable that
        for (int i = 0; i < 3; i++)
        {
            Transform hole = transform.GetChild(i);
            //GameObject.Destroy(hole.gameObject);
            hole.GetComponent<MeshRenderer>().enabled = false;
            hole.GetComponent<Collider>().enabled = false;
            foreach (var item in hole.GetComponentsInChildren<Transform>())
            {
                item.GetComponent<MeshRenderer>().enabled = false;
                //item.GetComponent<Collider>().enabled = false;
                //item.gameObject.SetActive(false);
            }

        }

        // add a trigger collider to that hole
        var holeObj = Instantiate(holeColliderPrefab, mainHole);
        holeObj.transform.localScale = new Vector3(2.9f, 2.9f, 1);
        TunnelHole tunnelHoleScript = holeObj.GetComponent<TunnelHole>();
        // assign current parent to hole script
        tunnelHoleScript.tunnelParent = transform.parent.GetComponent<TunnelParent>();
        tunnelHoleScript.transform.position += tunnelHoleScript.transform.forward;

        // then return the transform of that quad
        return mainHole;
    }

    public void JustEraseTiles()
    {
        // choose a random child and disable that
        for (int i = 0; i < 3; i++)
        {
            Transform hole = transform.GetChild(i);
            hole.GetComponent<MeshRenderer>().enabled = false;
            hole.GetComponent<Collider>().enabled = false;
            foreach (var item in hole.GetComponentsInChildren<Transform>())
            {
                item.GetComponent<MeshRenderer>().enabled = false;
                //item.GetComponent<Collider>().enabled = false;
                //item.gameObject.SetActive(false);
            }
        }
    }


}
