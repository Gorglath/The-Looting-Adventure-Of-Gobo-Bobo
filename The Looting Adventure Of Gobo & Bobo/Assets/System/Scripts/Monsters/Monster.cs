using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [HideInInspector]
    public bool IsAttackingPlayer = false;
    [Header("Base Variables")]
    [SerializeField]
    private int monsterHealth = 1;
    [SerializeField]
    protected int maxNumberOfBounces = 2;
    [SerializeField]
    private float timeOutOfCameraBeforeDespawning = 1f;
    [SerializeField]
    private float radiusToStartDetectingPlayer = 1f;
    [SerializeField]
    private string playerLayerName = "";
    //helpers
    protected Transform cacheTransform = null;
    protected Rigidbody monsterRigidbody = null;
    protected int currentNumberOfBounces = 0;
    private float counter = 0f;
    protected bool isPlayerDead = false;
    protected virtual void OnEnable()
    {
        cacheTransform = transform;
        monsterRigidbody = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
        if (BagManager.Instance.HaveLoot)
        {
            //Detected player.
            if (Physics.CheckSphere(cacheTransform.position, radiusToStartDetectingPlayer, 1 << LayerMask.NameToLayer(playerLayerName)))
            {
                if (!IsAttackingPlayer && !isPlayerDead)
                {
                    if (counter <= 0f)
                    {
                        IsAttackingPlayer = true;
                        counter = 2f;
                        AttackPlayer();
                    }
                    else
                    {
                        counter -= Time.deltaTime;
                    }
                }
            }

        }
    }

    private void FixedUpdate()
    {
        if (!IsAttackingPlayer)
        {
            MoveMonster();
        }
    }
    protected virtual void MoveMonster() { }
    protected virtual void AttackPlayer() { }
    protected virtual void MonsterDied() { }
    public virtual void StopMonster()
    {
        monsterRigidbody.velocity = Vector3.zero;
        monsterRigidbody.angularVelocity = Vector3.zero;
        isPlayerDead = true;
        IsAttackingPlayer = false;
    }
    protected bool IsDirectionCollideWithWall(Vector3 movementDirection)
    {
        return Physics.Raycast(cacheTransform.position, movementDirection, 10f, LayerMask.NameToLayer("Wall"));
    }
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, radiusToStartDetectingPlayer);
    }
}
