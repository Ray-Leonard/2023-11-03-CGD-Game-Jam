using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UnityEngine;

public class TunnelSegment : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private int childCount;
    [SerializeField] private int maxTrapCount;

    [Header("Prefabs")]
    [SerializeField] private GameObject holeColliderPrefab;
    [SerializeField] private GameObject holeSign;
    [SerializeField] private GameObject[] traps;


    public Transform MakeHole()
    {
        // choose a random child and disable that
        int holeIndex = Random.Range(0, transform.childCount);
        Transform hole = transform.GetChild(holeIndex);
        hole.GetComponent<MeshRenderer>().enabled = false;
        hole.GetComponent<Collider>().enabled = false;

        // add a trigger collider to that hole
        TunnelHole tunnelHoleScript = Instantiate(holeColliderPrefab, hole).GetComponent<TunnelHole>();
        // assign current parent to hole script
        //tunnelHoleScript.tunnelParent = transform.parent.GetComponent<TunnelParent>();
        // move the collider
        tunnelHoleScript.transform.position += tunnelHoleScript.transform.forward;

        // generate hole sign after the hole tile
        Transform holeSignTransform = Instantiate(holeSign, hole).transform;
        holeSignTransform.localRotation = Quaternion.Euler(-90f, 0, 0);
        holeSignTransform.position += (hole.up * 2f) - (0.5f * hole.forward);

        // then return the transform of that quad
        return hole;
    }


    public void GenerateYarnBall()
    {
        GameObject yarnBall = StatManager.Instance.GetYarnBallPrefab();
        // choose a random tile
        Transform tile = transform.GetChild(Random.Range(0, childCount));

        GeneratePrefab(yarnBall, tile);
    }

    public void GenerateTrap()
    {
        // query the level difficulty
        float difficulty = StatManager.Instance.GetLevelDifficulty();
        // decide if generate trap
        if(Random.Range(0f, 1f) > difficulty)
        {
            return;
        }

        int trapCount = Random.Range(1, Mathf.CeilToInt(maxTrapCount * difficulty));

        bool[] isTileOccupied = new bool[childCount];
        for(int i = 0; i < childCount; i++)
        {
            isTileOccupied[i] = false;
        }

        int trapGenerated = 0;
        while(trapGenerated < trapCount)
        {
            int tileIndex = Random.Range(0, childCount);
            if (isTileOccupied[tileIndex])
            {
                continue;
            }

            // generat a trap
            Transform tile = transform.GetChild(tileIndex);
            GeneratePrefab(traps[Random.Range(0, traps.Length)], tile);

            // update count and bool array
            isTileOccupied[tileIndex] = true;
            trapGenerated++;
            Debug.Log("Generate Trap");
        }
    }


    private void GeneratePrefab(GameObject prefab, Transform parent)
    {
        Transform objTransform = Instantiate(prefab, parent).transform;
        objTransform.localScale *= 0.5f;
        objTransform.localRotation = Quaternion.Euler(-90f, 0, 0);
        objTransform.position += objTransform.up * 0.5f;
    }

}
