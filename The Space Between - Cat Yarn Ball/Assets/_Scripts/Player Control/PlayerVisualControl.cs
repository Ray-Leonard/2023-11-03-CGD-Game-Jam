using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualControl : SingletonMonoBehaviour<PlayerVisualControl>
{
    public Renderer[] charSkins;

    public SkinType startSkinType = SkinType.pixel;

    private void Start()
    {
        SetVisual((int)startSkinType);
    }

    [ContextMenu("SetRandom")]
    public void SetRandom()
    {
        SetVisual(Random.Range(0,2));
    }

    public void SetPixelArt()
    {
        SetVisual((int)SkinType.pixel);
    }

    public void Set3DModel()
    {
        SetVisual((int)SkinType.model);
    }

    public void SetVisual(int numFocus)
    {
        for (int i = 0; i < charSkins.Length; i++)
        {
            charSkins[i].enabled = (i == numFocus);

        }
    }
}

public enum SkinType
{
    pixel = 0,
    model = 1
}
