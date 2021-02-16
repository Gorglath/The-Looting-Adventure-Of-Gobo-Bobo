using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoboManager : Singleton<GoboManager>
{
    [Header("Events")]
    [SerializeField]
    private TimedEvent[] whenSpawned;
    [SerializeField]
    private TimedEvent[] whenCollidingWithEnemy;
    [SerializeField]
    private TimedEvent[] whenCollidingWithTrap;
    [SerializeField]
    private TimedEvent[] whenCollidingWithLoot;
    [SerializeField]
    private TimedEvent[] whenCollidingWithVendor;
    
    [Header("Variables")]
    [SerializeField]
    private float movementSpeed = 5f;
    [SerializeField]
    private float rotationSpeed = 5f;
    [SerializeField]
    private float minMagnitudeToRotate = 0.2f;
    //helpers
    private Rigidbody goboRigidbody = null;
    private Transform cacheTransform = null;
    private Animator animator = null;
    private Vector3 newVelocity = Vector3.zero;
    private Quaternion newRotation = Quaternion.identity;
    private Vector3 monsterVelocity = Vector3.zero;
    private float movementX = 0f, movementZ = 0f;
    private float initialMovementSpeed = 0f;
    private float counter = 0f;
    private bool alreadyDied = false;
    private void OnEnable()
    {
        initialMovementSpeed = movementSpeed;
        movementSpeed /= 2f;
        goboRigidbody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        cacheTransform = transform;
    }
    private void Update()
    {
        CheckInput();
    }
    private void FixedUpdate()
    {
        RotateGobo();
        MoveGobo();
    }


    private void CheckInput()
    {
        movementX = Input.GetAxis("Horizontal");
        movementZ = Input.GetAxis("Vertical");
    }
    private void OnDestroy()
    {
        _instance = null;
    }
    public void EnableRagdoll()
    {
        foreach (Rigidbody rigidbody in GetComponentsInChildren<Rigidbody>())
        {
            rigidbody.isKinematic = false;
            rigidbody.velocity = (monsterVelocity == Vector3.zero) ? goboRigidbody.velocity : monsterVelocity * 50f;
            if (rigidbody.GetComponent<Collider>() != null)
            {
                rigidbody.GetComponent<Collider>().isTrigger = false;
            }
        }
        enabled = false;
    }
    private void RotateGobo()
    {
        if (goboRigidbody.velocity.sqrMagnitude > minMagnitudeToRotate)
        {
            counter += Time.deltaTime;
            if(counter >= 1f)
            {
                counter = 0f;
                BagManager.Instance.AddToAmountToReduce();
            }
            animator.SetBool("Move", true);
            newRotation = Quaternion.Slerp(cacheTransform.rotation, Quaternion.LookRotation(goboRigidbody.velocity), rotationSpeed);
            cacheTransform.rotation = newRotation;
        }
        else
        {
            counter = 0f;
            animator.SetBool("Move", false);
        }
       
    }
    private void MoveGobo()
    {
        newVelocity = new Vector3(movementX,0f,movementZ);
        newVelocity = Vector3.ClampMagnitude(newVelocity,1f) * movementSpeed;
        newVelocity = new Vector3(newVelocity.x, goboRigidbody.velocity.y, newVelocity.z);
        goboRigidbody.velocity = newVelocity;
    }

    public void GoboDied()
    {
        //GameOver
    }

    public void InVendorArea(bool isTrue)
    {
        animator.SetBool("InVendorArea", isTrue);
        movementSpeed = (isTrue) ? initialMovementSpeed / 2f : initialMovementSpeed;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Loot") && BagManager.Instance.CanTakeLoot)
        {
            EventManager.Instance.InvokeTimedEvents(whenCollidingWithLoot);
            LootManager.Instance.LootTaken(other.GetComponent<Loot>());
        }
        else if (other.CompareTag("Vendor") && BagManager.Instance.HaveLoot)
        {
            BagManager.Instance.GiveLootToVendor();
        }
        else if (other.CompareTag("Monster"))
        {
            if (other.GetComponent<Monster>().IsAttackingPlayer)
            {
                other.GetComponent<Monster>().StopMonster();
                monsterVelocity = (cacheTransform.position - other.transform.position).normalized;
                EventManager.Instance.InvokeTimedEvents(whenCollidingWithEnemy);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Spike") && other.GetComponent<Trap>().IsTrapActive && !alreadyDied)
        {
            alreadyDied = true;
            EventManager.Instance.InvokeTimedEvents(whenCollidingWithEnemy);
            monsterVelocity = (cacheTransform.position - other.transform.position).normalized;
        }
    }
}
