using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    string tweenID;

    private void OnEnable()
    {
        transform.GetChild(0).DOShakeScale(3, 0.5f, 3).SetId(tweenID);
    }

    private void OnDisable()
    {
        DOTween.Kill(tweenID);
    }
}
