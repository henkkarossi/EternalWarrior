using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingStats : MonoBehaviour
{
    [Header("Stats")]
    public float fireRate;
    public float projectileSpeed;
    public int projectileDamage;
    public int multishotLevel;
    public GameObject projectileObject;

    [Space(40)]
    public float range;
    public float aimWindup;
    public float cooldown;
}
