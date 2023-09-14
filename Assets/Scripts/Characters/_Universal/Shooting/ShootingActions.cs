using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class ShootingActions : ShootingStats
{
    //Main Actions
    public Vector3 direction;
    public string targetTag;
    public GameObject GetClosestTargetInRange(string targetTag)
    {
        GameObject[] allTargets = GameObject.FindGameObjectsWithTag(targetTag);
        GameObject closest = null;
        foreach (GameObject target in allTargets)
        {
            if (target != null)
            {
                if (closest == null || Vector3.Distance(target.transform.position, transform.position) < Vector3.Distance(closest.transform.position, transform.position))
                {
                    closest = target;
                }
            }
        }
        return closest;
    }

    public bool RangeCheck(Vector3 a, Vector3 b, float range)
    {
        if(range > 0)
            return Vector3.Distance(a, b) < range;
        else if(range < 0)
            return Vector3.Distance(a, b) > range * -1;
        return 
            false;
    }

    public bool Aim(Vector3 position)
    {
        SetTargetDirection(position);
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
            return true;
        }
        return false;
    }
    public void RangeAttack()
    {
        if (!shootSequencePlaying)
        {
            StartCoroutine(ShootSequence());
        }
    }
    public bool shootSequencePlaying;
    IEnumerator ShootSequence()
    {
        shootSequencePlaying = true;

        Shoot(transform.rotation);
        if (multishotLevel > 0)
            MultiShoot(multishotLevel);
         
        yield return new WaitForSeconds(cooldown);
        shootSequencePlaying = false;
    }
    void Shoot(Quaternion rotation)
    {
        GameObject p = GetProjectile();

        p.SetActive(true);

        p.GetComponent<Projectile>().SetTargetTag(targetTag);

        p.GetComponent<Projectile>().SetProjectileDamage(projectileDamage);

        p.GetComponent<Projectile>().SetProjectileSpeed(projectileSpeed);

        Vector3 newPos = transform.position + transform.forward * transform.lossyScale.z;

        p.transform.SetPositionAndRotation(newPos, rotation);

        ChangeProjectileState(p, true);
    }

    void MultiShoot(int amount)
    {
        Quaternion originalRotation = transform.rotation;
        for (int i = 1; i <= multishotLevel; i++)
        {
            float rotationAmount = 10 * i * 2;

            transform.Rotate(0, rotationAmount, 0);
            Shoot(transform.rotation);
            transform.rotation = originalRotation;

            transform.Rotate(0, -rotationAmount, 0);
            Shoot(transform.rotation);
            transform.rotation = originalRotation;
        }
    }


    //Projectile Specific
    public Projectile projectile;
    public int maxProjectileAmount = 20;
    public List<GameObject> inactive, active;
    public void SetProjectile(GameObject n)
    {
        if (n != null)
        {
            projectileObject = n;
            projectile = projectileObject.GetComponent<Projectile>();
        }
    }
    public void SetUpProjectiles(int amount, GameObject projectileObject)
    {
        if(inactive.Count <= 0)
        {
            for (int i = 0; i < amount; i++)
            {
                GameObject p = Instantiate(projectileObject, new Vector3(0, 0, 0), Quaternion.identity);
                p.GetComponent<Projectile>().SetParent(gameObject);
                p.SetActive(false);
                inactive.Add(p);
            }
        }
        else
        {
            ReturnAllProjectilesToPool();
        }
    }
    public GameObject GetProjectile()
    {
        if (inactive.Count <= 0)
        {
            ChangeProjectileState(active[0], false);
        }
        return inactive[^1];
    }
    public void ChangeProjectileState(GameObject p, bool state)
    {
        if (state == true)
        {
            active.Add(p);
            inactive.Remove(p);
        }
        if (state == false)
        {
            active.Remove(p);
            inactive.Add(p);
        }
        p.SetActive(state);
    }
    public void SetTargetDirection(Vector3 target)
    {
        direction = Vector3.Normalize(target - transform.position);
    }

    public void ReturnAllProjectilesToPool()
    {
        if(active.Count > 0) 
        {
            foreach (GameObject p in active)
            {
                ChangeProjectileState(p, false);
            }
        }
    }


    //Setting Stats
    public void ResetStats(ShootingStats basic)
    {
        SetProjectile(basic.projectileObject);
        SetProjectileSpeed(basic.projectileSpeed);
        SetProjectileDamage(basic.projectileDamage);

        SetUpProjectiles(maxProjectileAmount, basic.projectileObject);

        SetFireRate(basic.fireRate);
        SetRange(basic.range);
        if(gameObject.GetComponent<NavMeshAgent>())
            gameObject.GetComponent<NavMeshAgent>().stoppingDistance = basic.range;
        SetWindupTime(basic.aimWindup);
        SetMultishotLevel(basic.multishotLevel);
    }
    public void UpgradeStats(ShootingStats upgrade)
    {
        if(upgrade.projectileObject != null)
            SetProjectile(upgrade.projectileObject);
        UpgradeProjectileSpeed(upgrade.projectileSpeed);
        UpgradeProjectileDamage(upgrade.projectileDamage);
        UpgradeFireRate(upgrade.fireRate);
        UpgradeRange(upgrade.range);
        if (gameObject.GetComponent<NavMeshAgent>())
            gameObject.GetComponent<NavMeshAgent>().stoppingDistance += upgrade.range;
        UpgradeWindupTime(upgrade.aimWindup);
        UpgradeMultishotLevel(upgrade.multishotLevel);
    }
    public void SetFireRate(float n)
    {
        fireRate = n;
        cooldown = 1 / fireRate;
    }
    public void UpgradeFireRate(float u)
    {
        fireRate += u;
        cooldown = 1 / fireRate;
    }

    public void SetRange(float n)
    {
        range = n;
    }

    public void UpgradeRange(float u)
    {
        range += u;
    }

    public void SetProjectileSpeed(float speed)
    {
        projectileSpeed = speed;
    }

    public void UpgradeProjectileSpeed(float u)
    {
        projectileSpeed += u;
    }

    public void SetProjectileDamage(int n)
    {
        projectileDamage = n;
    }

    public void UpgradeProjectileDamage(int u)
    {
        projectileDamage += u;
    }

    public void SetMultishotLevel(int n)
    {
        multishotLevel = n;
    }

    public void UpgradeMultishotLevel(int u)
    {
        multishotLevel += u;
    }

    public void SetWindupTime(float n)
    { 
        aimWindup = n; 
    }

    public void UpgradeWindupTime(float u)
    {
        aimWindup += u;
    }

    public void SetCooldown(float n)
    {
        cooldown = n;
    }

    public void UpgradeCooldown(float u)
    {
        cooldown += u;
    }
}
