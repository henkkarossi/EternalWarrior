using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeActions : MeleeStats
{
    //Main Actions
    public string targetTag;
    public void Attack()
    {
        if(!attackSequencePlaying)
            StartCoroutine(AttackSequence());
    }

    public GameObject rangeMarker;
    public float cooldown;
    public float cooldownPercent;
    [HideInInspector]
    public bool attackSequencePlaying;
    IEnumerator AttackSequence()
    {
        attackSequencePlaying = true;

        if(rangeMarker != null)
            rangeMarker.SetActive(true);

        foreach (GameObject target in GetTargetsAtRange(targetTag, meleeRange))
        {
            Vector3 knockback = (gameObject.transform.position - target.transform.position).normalized * meleeDamage * 0.5f;

            if (target.GetComponent<Enemy>())
            {
                target.GetComponent<Enemy>().TakeDamage(meleeDamage);
                target.GetComponent<Enemy>().KnockBack(knockback);
            }
                
            if (target.GetComponent<Player>())
            {
                target.GetComponent<Player>().TakeDamage(meleeDamage);
                target.GetComponent<Player>().KnockBack(knockback);
            }
        }

        yield return new WaitForSeconds(.5f);

        if(rangeMarker != null)
            rangeMarker.SetActive(false);

        attackSequencePlaying = false;
    }

    public void ChargeEnergy()
    {
        IncreaseCooldown(Time.deltaTime * cooldownSpeed);
        SetCooldownPercent(cooldown);
    }

    public List<GameObject> GetTargetsAtRange(string targetTag, float range)
    {
        List<GameObject> enemiesAtRange = new();
        foreach(GameObject enemy in GameObject.FindGameObjectsWithTag(targetTag))
        {
            if(Vector3.Distance(enemy.transform.position, transform.position) <= range)
            {
                enemiesAtRange.Add(enemy);
            }
        }
        return enemiesAtRange;
    }

    //Setting Stats
    public void ResetStats(MeleeStats basicStats)
    {
        rangeMarker.SetActive(false);
        SetMaxCooldown(basicStats.maxCooldown);
        SetCooldownSpeed(basicStats.cooldownSpeed);
        SetMeleeDamage(basicStats.meleeDamage);
        SetMeleeRange(basicStats.meleeRange);
    }
    public void UpgradeStats(MeleeStats upgrade)
    {
        UpgradeCooldownSpeed(upgrade.maxCooldown);
        UpgradeMeleeDamage(upgrade.meleeDamage);
        UpgradeMeleeRange(upgrade.meleeRange);
    }
    void SetMaxCooldown(float n)
    {
        maxCooldown = n;
    }
    void SetCooldownSpeed(float n)
    {
        cooldownSpeed = n;
    }
    void UpgradeCooldownSpeed(float u)
    {
        cooldownSpeed += u;
    }
    public void SetCooldown(float value)
    {
        cooldown = value;
        SetCooldownPercent(cooldown);
    }
    public void IncreaseCooldown(float amount)
    {
        cooldown += amount;
        SetCooldownPercent(cooldown);
    }
    void SetCooldownPercent(float value)
    {
        cooldownPercent = Mathf.Round((value / maxCooldown * 100) * 1f);
    }
    void SetMeleeDamage(int n)
    {
        meleeDamage = n;
    }
    void UpgradeMeleeDamage(int u)
    {
        meleeDamage += u;
    }
    void SetMeleeRange(float n)
    {
        meleeRange = n;
        RescaleRangeMarker(meleeRange);
    }
    void UpgradeMeleeRange(float u) 
    {
        meleeRange += u;
        RescaleRangeMarker(meleeRange);
    }
    void RescaleRangeMarker(float amount)
    {
        Vector3 newScale = rangeMarker.transform.localScale;
        newScale.x = amount * 1.7f;
        newScale.z = amount * 1.7f;
        rangeMarker.transform.localScale = newScale;
    }
}
