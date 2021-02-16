using Pixelplacement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    [HideInInspector]
    public Transform LootSpawnPoint;
    [Header("Variables")]
    [SerializeField]
    private float timeToBlink = 1f;
    [SerializeField]
    public bool BonusLoot = false;
    [SerializeField]
    public int LootScore = 200;

    private Renderer lootRenderer; 
    private void OnEnable()
    {
        lootRenderer = (GetComponent<MeshRenderer>() != null) ? GetComponent<MeshRenderer>() : GetComponentInChildren<MeshRenderer>();
        Tween.ShaderFloat(lootRenderer.material, "_Emission",0.3f, 2f, timeToBlink, 0f, null, Tween.LoopType.PingPong);
    }
}
