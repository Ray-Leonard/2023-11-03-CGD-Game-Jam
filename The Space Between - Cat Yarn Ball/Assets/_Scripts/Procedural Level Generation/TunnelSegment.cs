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

    [SerializeField] private int difficultyMultiplier = 1;

    public bool generateTraps = true;

    public Transform MakeHole()
    {
        // choose a random child and disable that
        Transform hole = transform.GetChild(Random.Range(0, transform.childCount));
        hole.GetComponent<MeshRenderer>().enabled = false;
        hole.GetComponent<Collider>().enabled = false;

        // add a trigger collider to that hole
        TunnelHole tunnelHoleScript = Instantiate(holeColliderPrefab, hole).GetComponent<TunnelHole>();
        // assign current parent to hole script
        //tunnelHoleScript.tunnelParent = transform.parent.GetComponent<TunnelParent>();
        tunnelHoleScript.transform.position += tunnelHoleScript.transform.forward;

        // generate hole sign
        Instantiate(holeSign, hole.position - 2 * hole.forward, hole.rotation, hole);

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


    public void GenerateYarnBall()
    {
        GameObject yarnBall = StatManager.Instance.GetYarnBallPrefab();
        // choose a random tile
        Transform tile = transform.GetChild(Random.Range(0, childCount));

        GeneratePrefab(yarnBall, tile);
    }

    public void GenerateTrap()
    {
        if (!generateTraps)
            return;

        // query the level difficulty
        float difficulty = StatManager.Instance.GetLevelDifficulty();
        // decide if generate trap
        if(Random.Range(0f, 1f) > difficulty)
        {
            return;
        }

        int trapCount = Random.Range(1, Mathf.CeilToInt(maxTrapCount * difficulty * difficultyMultiplier));

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
