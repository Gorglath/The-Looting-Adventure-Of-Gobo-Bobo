using Pixelplacement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawnPoint : MonoBehaviour
{
    [Header("Events")]
    [SerializeField]
    private TimedEvent[] whenBeginningToSpawnObject;
    [SerializeField]
    private TimedEvent[] whenFinishedSpawningObject;

    [Header("References")]
    [SerializeField]
    private Transform spawnPoint = null;
    [SerializeField]
    private Transform barbPreSpawnLocation = null;
    [SerializeField]
    private Animator relatedDoor = null;
    [Header("Variables")]
    [SerializeField]
    private bool isDoorSpawnPoint = false;
    //helpers
    private GameObject objectPlaceHolder = null;
    private Transform spawnLocation = null;
    private void Start()
    {
        spawnLocation = (spawnPoint == null) ? transform : spawnPoint;
    }
    public void SpawnObjectAtLocation(Factory objectFactory, bool barb = false)
    {
        if (!barb)
        {
            objectPlaceHolder = objectFactory.SpawnObject();
            objectPlaceHolder.transform.position = spawnLocation.position;
        }
        else
        {
            objectPlaceHolder = objectFactory.SpawnObject();
            objectPlaceHolder.GetComponent<Animator>().SetBool((isDoorSpawnPoint) ? "SpawnedAtDoor" : "SpawnedAtTrap", true);
            relatedDoor.GetComponent<Animator>().SetBool("Spawn", true);
            objectPlaceHolder.transform.position = barbPreSpawnLocation.position;
            if (isDoorSpawnPoint)
            {
                objectPlaceHolder.transform.rotation = Quaternion.LookRotation(barbPreSpawnLocation.position - spawnLocation.position, Vector3.up);
                Tween.Position(objectPlaceHolder.transform, barbPreSpawnLocation.position, spawnLocation.position, 1.7f, 0f);
            }
            else
            {
                objectPlaceHolder.transform.rotation = Quaternion.LookRotation(spawnLocation.position -
                    GetComponent<Spline>().Anchors[GetComponent<Spline>().Anchors.Length - 1].transform.position, Vector3.up);
                Tween.Spline(GetComponent<Spline>(), objectPlaceHolder.transform, 0f, 1f, false, 1f, 0f);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere((spawnPoint == null) ? transform.position : spawnPoint.position, 1f);
    }
}
