using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Sprite emptyHeartSprite;
    [SerializeField] private Sprite fullHeartSprite;

    public void UpdateHealthUI(int currentHealth)
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Image heartImg = transform.GetChild(i).GetComponent<Image>();
            if (i < currentHealth)
            {
                heartImg.sprite = fullHeartSprite;
            }
            else
            {
                heartImg.sprite = emptyHeartSprite;
            }
        }
    }
}
