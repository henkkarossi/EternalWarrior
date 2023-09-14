using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour
{
    [HideInInspector]
    public InputManager inputManager;
    [HideInInspector]
    public Player player;
    public PlayerStats playerBasicStats;
    [HideInInspector]
    public Shooting shooting;
    public ShootingStats shootingBasicStats;
    [HideInInspector]
    public Melee melee;
    public MeleeStats meleeBasicStats;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        inputManager.player = this;

        player = GetComponent<Player>();
        player.ResetStats(playerBasicStats);

        shooting = GetComponent<Shooting>();
        shooting.ResetStats(shootingBasicStats);

        melee = GetComponent<Melee>();
        melee.ResetStats(meleeBasicStats);
        melee.targetTag = "Enemy";
    }

    public List<InputLine> pendingInputList = new();
    public List<InputLine> executedInputList = new();
    public void HandleInput (InputLine inputLine)
    {
        pendingInputList.Add(inputLine);

        if (pendingInputList.Count > 0)
        {
            List<InputLine> currentInputList = new List<InputLine>(pendingInputList);
            foreach (InputLine line in currentInputList)
            {
                if (MoveCheck(line) || MeleeAttackCheck(line))
                {
                    executedInputList.Add(line);
                    pendingInputList.Remove(line);
                }
                else
                {
                    print("Unknown Input");
                    pendingInputList.Remove(line);
                }
            }
        }
    }

    private void Update()
    {
        ShootingCheck();
    }

    bool MoveCheck(InputLine line)
    {
        if (line.direction != Vector3.zero)
        {
            if (player.state != Player.State.off && player.state != Player.State.takeDamage)
            {
                player.ChangeState(Player.State.move);
                player.moveDirection = line.direction;
            }
            return true;
        }
        return false;
    }

    void ShootingCheck()
    {
        string targetTag = "Enemy";
        GameObject target = shooting.GetClosestTargetInRange(targetTag);
        if (target != null && shooting.state != Shooting.State.off && melee.state != Melee.State.attack && player.state != Player.State.takeDamage)
        {
            player.lookMoveDirection = false;

            shooting.SetTarget(target.transform.position, targetTag);
            shooting.BeginShooting();
        }
        else
            player.lookMoveDirection = true;
    }


    bool MeleeAttackCheck(InputLine line)
    {
        if (line.doubleClick)
        {
            if(melee.state == Melee.State.ready && player.state != Player.State.takeDamage)
            {
                melee.state = Melee.State.attack;
            }
            return true;
        }
        return false;
    }
}
