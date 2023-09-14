using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class EnemyController : MonoBehaviour
{
    [HideInInspector]
    public Enemy enemy;
    [HideInInspector]
    public Shooting shooting;
    [HideInInspector]
    public Melee melee;

    public EnemyUpgrade enemyBasicStats;
    public ShootingUpgrade shootingBasicStats;
    public MeleeUpgrade meleeBasicStats;

    public string targetTag = "Player";
    public Vector3 target;
    float brainTimer;

    public List<Vector3> targetHistory = new List<Vector3>();

    private void Awake()
    {
        ResetAll();
    }

    private void OnEnable()
    {
        ResetAll();
    }

    public void ResetAll()
    {
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

        enemy = GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.state = Enemy.State.idle;
            enemy.ResetStats(enemyBasicStats);
        }   

        shooting = GetComponent<Shooting>();
        if (shooting != null)
        {
            shooting.aimSequencePlaying = false;
            shooting.shootSequencePlaying = false;
            shooting.shootSequencePlaying = false;
            shooting.state = Shooting.State.ready;
            shooting.ResetStats(shootingBasicStats);
        }  

        melee = GetComponent<Melee>();
        if (melee != null)
        {
            melee.state = Melee.State.ready;
            melee.ResetStats(meleeBasicStats);
            melee.targetTag = "Player";
        }
    }

    private void FixedUpdate()
    {
        if (enemy != null)
        {
            CalculateTargetPosition(enemy.brainLag);

            LetsMove();

            if (shooting != null)
            {
                LetsShoot();
            }
            if (melee != null)
            {
                LetsMelee();
            }
        }
    }

    void CalculateTargetPosition(float brainLag) 
    {
        Vector3 newTarget = GetClosestTargetInRange(targetTag).transform.position;
        if(targetHistory.Count > 0)
        {
            if (Vector3.Distance(targetHistory[targetHistory.Count - 1], target) > brainLag * 10)
                targetHistory.Add(newTarget);
        }
        else
            targetHistory.Add(newTarget);

        if (Time.time > brainLag / 10 + brainTimer) 
        {
            int currentIndex = targetHistory.IndexOf(target);

            if (currentIndex > 0)
                target = targetHistory[currentIndex + 1];
            else
                target = targetHistory[0];

            targetHistory.Remove(targetHistory[0]);
            brainTimer = Time.time;
        }
    }

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

    void LetsMove()
    {
        if (enemy.state != Enemy.State.off && enemy.state != Enemy.State.takeDamage)
        {
            enemy.state = Enemy.State.move;
            enemy.target = target;
        }
    }

    /*
     Entinen tyranid warrior tyylinen
    bool MoveCheck()
    {
        if (enemy.state != Enemy.State.off && enemy.state != Enemy.State.takeDamage)
        {
            if (shooting == null || shooting.state == Shooting.State.off || shooting.state == Shooting.State.ready)
            {
                enemy.state = Enemy.State.run;
            }
            else
            {
                enemy.state = Enemy.State.walk;
            }
            return true;
        }
        return false;
    }
    */

    void LetsShoot()
    {
        if (target != null && shooting.state != Shooting.State.off && enemy.state != Enemy.State.takeDamage)
        {
            shooting.SetTarget(target, targetTag);
            shooting.BeginShooting();
        }
    }

    void LetsMelee()
    {
        if (melee.GetTargetsAtRange(targetTag, melee.meleeRange).Count > 0)
        {
            if (melee.state == Melee.State.ready && enemy.state != Enemy.State.takeDamage)
            {
                melee.state = Melee.State.attack;
            }
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        GameObject o = other.gameObject;

        if (o.CompareTag(targetTag))
        {
            melee.state = Melee.State.attack;
        }
    }

}