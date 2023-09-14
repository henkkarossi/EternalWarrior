using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shooting : ShootingActions
{
    public enum State { ready, aim, shoot, off};
    public State state;
    public Vector3 target;
    public bool aimSequencePlaying;

    private void Update()
    {
        switch (state)
        {
            case State.ready:
                break;
            case State.aim:
                Aim(target);
                if (RangeCheck(target, transform.position, range))
                    state = State.shoot;
                else
                    state = State.ready;
                break;
            case State.shoot:
                RangeAttack();
                if (RangeCheck(target, transform.position, range))
                    state = State.aim;
                else
                    state = State.ready;
                break;
        }
    }

    public void BeginShooting()
    {
        if (!aimSequencePlaying)
            StartCoroutine(AimingSequence(aimWindup));
    }

    public void SetTarget(Vector3 pos, string tag)
    {
        target = pos;
        targetTag = tag;
    }

    private void OnEnable()
    {
        ReturnAllProjectilesToPool();
    }

    IEnumerator AimingSequence(float windup)
    {
        aimSequencePlaying = true;

        yield return new WaitForSeconds(windup);

        if (Aim(target))
            state = State.aim;

        aimSequencePlaying = false;
    }
}
