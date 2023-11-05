using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YarnBallBar : MonoBehaviour
{
    public void UpdateScoreUI(int currentScore)
    {
        if(currentScore < 1 || currentScore > transform.childCount) return;

        transform.GetChild(currentScore - 1).GetChild(0).gameObject.SetActive(true);
    }
}
