using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObject objectToSpawn = null;

    public virtual GameObject SpawnObject()
    {
        return Instantiate(objectToSpawn);
    }
}
