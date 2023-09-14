using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : PlayerStats
{
    //Main Actions
    public bool lookMoveDirection = true; 
    public void Move(Vector3 direction)
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = direction * walkSpeed;
        if(lookMoveDirection)
            Look(direction);
    }
    public void Look(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    public void StopMoving()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = new Vector3(0, rigidbody.velocity.y, 0);
    }

    public void KnockBack(Vector3 direction)
    {
        gameObject.GetComponent<Rigidbody>().AddForce(-direction * 10, ForceMode.Impulse);
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
                hp = 0;
            else
                hp -= amount;
        }
    }


    //Setting Stats
    public void ResetStats(PlayerStats basicStats)
    {
        SetWalkSpeed(basicStats.walkSpeed);
        SetMaxHP(basicStats.maxHp);
        SetHP(basicStats.maxHp);
    }
    public void UpgradeStats(PlayerStats upgrade)
    {
        UpgradeWalkSpeed(upgrade.walkSpeed);
        UpgradeMaxHP(upgrade.maxHp);
    }

    
    //Individual Stats
    public float walkSpeedPrevious;
    public Vector3 moveDirection;
    public void SetWalkSpeed(float n)
    {
        SetWalkSpeedPrevious(walkSpeed);
        walkSpeed = n;
    }

    public void SetWalkSpeedPrevious(float n)
    {
        walkSpeedPrevious = n;
    }

    public void UpgradeWalkSpeed(float u)
    {
        walkSpeed += u;
    }

    public int hp;
    
    public void SetHP(int n)
    {
        hp = n;
    }

    public void SetMaxHP(int n)
    {
        maxHp = n;
    }

    public void UpgradeMaxHP(int u)
    {
        int oldMaxHP = maxHp;
        maxHp += u;
        hp += maxHp - oldMaxHP;
    }
}
