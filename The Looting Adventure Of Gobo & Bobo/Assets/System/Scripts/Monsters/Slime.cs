using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Monster
{
    [Header("References")]
    [SerializeField]
    private PhysicMaterial chargingMaterial = null;
    [SerializeField]
    private PhysicMaterial defaultMaterial = null;
    [Header("Variables")]
    [SerializeField]
    private float forceToAddWhenMoving = 100f;
    [SerializeField]
    private float forceToAddWhenCharging = 200f;
    [SerializeField]
    private float minTimeBetweenMovingSlime = 2f;
    [SerializeField]
    private float maxTimeBetweenMovingSlime = 4f;
    [SerializeField]
    private float minTimeBeforeChargingTowardsPlayer = 1f;
    [SerializeField]
    private float maxTimeBeforeChargingTowardsPlayer = 3f;
    //helpers
    private Animator animator = null;
    private Ray ray;
    private RaycastHit hit;
    private Vector3 movementDirection = Vector3.zero;
    private Vector3 lookDirection = Vector3.zero;
    private float timeBeforeMovingAgain = 0f;
    private float timeBeforeCharging = 0f;
    private bool canSlimeMove = false;
    private bool isCurrentlyCharging = false;
    private bool didAlreadyConstraintedRigidbody = false;
    protected override void OnEnable()
    {
        base.OnEnable();
        animator = GetComponent<Animator>();
        timeBeforeMovingAgain = Random.Range(minTimeBetweenMovingSlime, maxTimeBetweenMovingSlime);
    }
    protected override void Update()
    {
        base.Update();
        if (!IsAttackingPlayer)
        {
            CheckIfCanMoveSlime();   
        }
        else
        {
            RotateSlimeTowardVelocity();
        }
    }
    private void RotateSlimeTowardVelocity()
    {
        if (monsterRigidbody.velocity.magnitude > 0.1f)
        {
            cacheTransform.rotation = Quaternion.LookRotation(monsterRigidbody.velocity, Vector3.up);
        }
        else if(isCurrentlyCharging)
        {
            animator.SetBool("Stopped", true);
            animator.SetBool("Move", false);
            animator.SetBool("DetectedPlayer", false);
            animator.SetBool("ChargePlayer", false); 
            foreach (Collider collider in GetComponents<Collider>())
            {
                collider.material = defaultMaterial;
            }
            if (!GetComponentInChildren<MeshRenderer>().isVisible)
            {
                MonsterDied();
            }
            else
            {
                IsAttackingPlayer = false;
                isCurrentlyCharging = false;
                canSlimeMove = false;
                timeBeforeMovingAgain = Random.Range(minTimeBetweenMovingSlime, maxTimeBetweenMovingSlime);
            }
        }
    }
    public void FreezeRigidbody()
    {
        monsterRigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
    }
    private void CheckIfCanMoveSlime()
    {
        timeBeforeMovingAgain -= Time.deltaTime;
        if (timeBeforeMovingAgain <= 0f && !canSlimeMove)
        {
            canSlimeMove = true;
        }
    }
    protected override void MoveMonster()
    {
        if (canSlimeMove)
        {
            canSlimeMove = false;
            timeBeforeMovingAgain = Random.Range(minTimeBetweenMovingSlime, maxTimeBetweenMovingSlime);
            do
            {
                movementDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            } while (IsDirectionCollideWithWall(movementDirection));
            lookDirection = (movementDirection + cacheTransform.position) - cacheTransform.position;
            cacheTransform.rotation = Quaternion.LookRotation(lookDirection,Vector3.up);
            animator.SetBool("Move", true);
            animator.SetBool("Stopped", false);
        }
    }
    protected override void MonsterDied()
    {
        Destroy(gameObject);
        MonsterSpawnManager.Instance.MonsterDied();
    }
    protected override void AttackPlayer()
    {
        SoundManager.Instance.PlaySlimeDetectionSFX();
        animator.SetBool("DetectedPlayer", true);
        timeBeforeCharging = Random.Range(minTimeBeforeChargingTowardsPlayer, maxTimeBeforeChargingTowardsPlayer);
        StartCoroutine(ChargeTowardPlayer());
    }
    public void Move()
    {
        monsterRigidbody.AddForce(movementDirection * forceToAddWhenMoving, ForceMode.Impulse);
    }
    public void FinishedMoving()
    {
        animator.SetBool("Move", false);
    }
    IEnumerator ChargeTowardPlayer(float value = 0f)
    {
        if(value/timeBeforeCharging < 1f)
        {
            value += Time.deltaTime / timeBeforeCharging;
            cacheTransform.rotation = Quaternion.LookRotation((GoboManager.Instance.transform.position - cacheTransform.position).normalized);
            yield return null;
            StartCoroutine(ChargeTowardPlayer(value));
        }
        else
        {
            SoundManager.Instance.PlaySlimeChargeSFX();
            monsterRigidbody.AddForce(cacheTransform.forward.normalized * forceToAddWhenCharging, ForceMode.Impulse);
            foreach (Collider collider in GetComponents<Collider>())
            {
                collider.material = chargingMaterial;
            }
            animator.SetBool("ChargePlayer", true);
            isCurrentlyCharging = true;
            canSlimeMove = false;
            timeBeforeMovingAgain = Random.Range(minTimeBetweenMovingSlime, maxTimeBetweenMovingSlime);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (isCurrentlyCharging && !collision.collider.CompareTag("Floor"))
        {
            SoundManager.Instance.PlaySlimeImpactSFX();
        }
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, new Vector3(3f,0f,3f));
    }
}
