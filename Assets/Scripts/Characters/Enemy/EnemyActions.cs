using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyActions : EnemyStats
{
    public EnemiesManager manager;

    public NavMeshAgent agent;
    public Vector3 target;
    public void Move(Vector3 target, float speed) 
    {
        agent.SetDestination(target);
        agent.speed = speed;
    }

    public void Stop()
    {
        agent.SetDestination(transform.position);
        agent.speed = 0;
    }

    public void Heal(int amount)
    {
        if (hp < maxHp)
        {
            hp += amount;
        }
        else
        {
            hp = maxHp;
        }
    }

    public void Damage(int amount)
    {
        if (hp > 0)
        {
            if (amount >= hp) 
            {
                hp = 0;
            }
            else
            {
                hp = hp - amount;
            }
        }
    }

    public void KnockBack(Vector3 direction)
    {
        gameObject.GetComponent<Rigidbody>().AddForce(-direction * 10, ForceMode.Impulse);
    }

    public void Stabilize()
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void Death()
    {
        if(manager != null)
            manager.ReturnToPool(gameObject);
        else
            gameObject.SetActive(false);
    }

    //Setting Stats
    public void ResetStats(EnemyStats basicStats)
    {
        SetMoveSpeed(basicStats.moveSpeed);
        SetMaxHP(basicStats.maxHp);
        SetHP(basicStats.maxHp);
        SetInvulnerabilityLength(basicStats.invulnerabilityLength);
        SetBrainLag(basicStats.brainLag);
    }
    public void UpgradeStats(EnemyStats upgrade)
    {
        UpgradeMoveSpeed(upgrade.moveSpeed);
        UpgradeMaxHP(upgrade.maxHp);
        UpgradeInvulnerabilityLength(upgrade.invulnerabilityLength);
    }


    //Individual Stats
    public float walkSpeedPrevious;
    public Vector3 moveDirection;
    public void SetMoveSpeed(float n)
    {
        SetMoveSpeedPrevious(moveSpeed);
        moveSpeed = n;
    }

    public void SetMoveSpeedPrevious(float n)
    {
        walkSpeedPrevious = n;
    }

    public void UpgradeMoveSpeed(float u)
    {
        moveSpeed += u;
    }

    public int hp;

    public void SetHP(int n)
    {
        hp = n;
    }

    public void SetMaxHP(int n)
    {
        maxHp = n;
        hp = maxHp;
    }

    public void SetBrainLag(float n)
    {
        brainLag = n;
    }

    public void UpgradeMaxHP(int u)
    {
        int oldMaxHP = maxHp;
        maxHp = u;
        hp += maxHp - oldMaxHP;
    }

    public float HealthPercentage(int current, int max)
    {
        float percentage = (float)current / (float)max * 100;
        return percentage;
    }

    public void SetInvulnerabilityLength(float n)
    {
        invulnerabilityLength = n;
    }

    public void UpgradeInvulnerabilityLength(float u)
    {
        invulnerabilityLength += u;
    }
}
