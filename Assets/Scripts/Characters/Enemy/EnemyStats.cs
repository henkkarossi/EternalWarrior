using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStats : MonoBehaviour 
{
    [Header("Stats")]
    [Space(10)]
    public int maxHp;
    public float moveSpeed;
    public float invulnerabilityLength;
    public float brainLag;
}
