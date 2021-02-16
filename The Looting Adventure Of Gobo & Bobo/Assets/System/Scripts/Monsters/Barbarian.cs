using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barbarian : Monster
{
    [Header("Variables")]
    [SerializeField]
    private float movementSpeed = 100f;
    [SerializeField]
    private float movementSpeedWhenCharging = 200f;
    [SerializeField]
    private float minTimeBetweenMoving = 2f;
    [SerializeField]
    private float maxTimeBetweenMoving = 4f;
    [SerializeField]
    private float minTimeBeforeChargingTowardsPlayer = 1f;
    [SerializeField]
    private float maxTimeBeforeChargingTowardsPlayer = 3f;
    //helpers
    private Animator animator = null;
    private Vector3 movementDirection = Vector3.zero;
    private Vector3 lookDirection = Vector3.zero;
    private float timeBeforeMovingAgain = 0f;
    private float timeBeforeCharging = 0f;
    private bool canMove = false;
    private bool isCurrentlyCharging = false;
    protected override void OnEnable()
    {
        base.OnEnable();
        animator = GetComponent<Animator>();
        timeBeforeMovingAgain = Random.Range(minTimeBetweenMoving, maxTimeBetweenMoving);
    }
    protected override void Update()
    {
        base.Update();
        if (!IsAttackingPlayer)
        {
            CheckIfCanMove();
        }
        else if (isCurrentlyCharging)
        {
            monsterRigidbody.velocity = -cacheTransform.forward.normalized * movementSpeedWhenCharging;
        }
    }
    public void BarbStopped()
    {
        SoundManager.Instance.PlayBarbImpactSFX();
        animator.SetBool("Stopped", true);
            animator.SetBool("Move", false);
            animator.SetBool("DetectedPlayer", false);
            animator.SetBool("ChargePlayer", false);

                IsAttackingPlayer = false;
        isCurrentlyCharging = false;
                canMove = false;
                timeBeforeMovingAgain = Random.Range(minTimeBetweenMoving, maxTimeBetweenMoving);
        monsterRigidbody.velocity = Vector3.zero;
    }
    public void FreezeRigidbody()
    {
        monsterRigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
    }
    private void CheckIfCanMove()
    {
        timeBeforeMovingAgain -= Time.deltaTime;
        if (timeBeforeMovingAgain <= 0f && !canMove)
        {
            canMove = true;
        }
    }
    protected override void MoveMonster()
    {
        if (canMove)
        {
            canMove = false;
            timeBeforeMovingAgain = Random.Range(minTimeBetweenMoving, maxTimeBetweenMoving);
            do
            {
                movementDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            } while (IsDirectionCollideWithWall(-movementDirection));
            lookDirection = (movementDirection + cacheTransform.position) - cacheTransform.position;
            monsterRigidbody.velocity = -movementDirection * movementSpeed;
            cacheTransform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
            animator.SetBool("Move", true);
            animator.SetBool("Stopped", false);
            Invoke("FinishedMoving", Random.Range(1f, 2f));
        }
    }
    protected override void MonsterDied()
    {
        Destroy(gameObject);
        MonsterSpawnManager.Instance.MonsterDied();
    }
    protected override void AttackPlayer()
    {
        SoundManager.Instance.PlayBarbDetectSFX();
        animator.SetBool("DetectedPlayer", true);
        timeBeforeCharging = Random.Range(minTimeBeforeChargingTowardsPlayer, maxTimeBeforeChargingTowardsPlayer);
        StartCoroutine(ChargeTowardPlayer());
    }
    public void FinishedMoving()
    {
        animator.SetBool("Move", false);
        monsterRigidbody.velocity = Vector3.zero;
    }
    IEnumerator ChargeTowardPlayer(float value = 0f)
    {
        if (value / timeBeforeCharging < 1f)
        {
            value += Time.deltaTime / timeBeforeCharging;
            cacheTransform.rotation = Quaternion.LookRotation((cacheTransform.position - GoboManager.Instance.transform.position).normalized);
            yield return null;
            StartCoroutine(ChargeTowardPlayer(value));
        }
        else
        {
            SoundManager.Instance.PlayBarbChargeSFX();
            monsterRigidbody.velocity = -cacheTransform.forward.normalized * movementSpeedWhenCharging;
            animator.SetBool("ChargePlayer", true);
            isCurrentlyCharging = true;
            canMove = false;
            timeBeforeMovingAgain = Random.Range(minTimeBetweenMoving, maxTimeBetweenMoving);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (isCurrentlyCharging && !collision.collider.CompareTag("Floor"))
        {
            BarbStopped();
        }
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, new Vector3(3f, 0f, 3f));
    }
}
