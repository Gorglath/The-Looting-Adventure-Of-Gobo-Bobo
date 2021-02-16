using Pixelplacement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallFade : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Material[] materialsToFade;
    [Header("Variables")]
    [SerializeField]
    private float timeToFade = 1f;

    //helpers
    private Color newColor = Color.white;
    private void Start()
    {
        FadeOutWall();
    }
    public void FadeInWall()
    {
        FadeWall(1f);
    }

    public void FadeOutWall()
    {
        FadeWall(0f);
    }

    private void FadeWall(float alpha)
    {
        foreach (Material material in materialsToFade)
        {
            newColor = material.GetColor("_Color");
            newColor.a = alpha;
            Tween.ShaderColor(material, "_Color", newColor, timeToFade, 0f);
        }
    }
}
