using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : EnemyActions
{
    public enum State { idle, move, takeDamage, off};
    public State state;
    State previousState;
    public bool immortal;
    bool invulnerable;
    Material material;
    public Material blink;

    void ChangeState(State newState)
    {
        previousState = state;
        state = newState;
    }
    private void Awake()
    {
        hp = maxHp;
        material = gameObject.GetComponent<MeshRenderer>().material;
    }

    void Update()
    {
        switch (state)
        {
            case State.idle:
                break;
            case State.move:
                Move(target, moveSpeed);
                break;
            case State.takeDamage:
                break;
        }
    }

    public void TakeDamage(int amount)
    {
        if (invulnerable == false)
        {
            StartCoroutine(InvunerabilityTime(invulnerabilityLength));
            StartCoroutine(DamageBlink(1, 0.02f));
            ChangeState(State.takeDamage);
            Damage(amount);
            if (hp <= 0 && !immortal)
            {
                StartCoroutine(DeathSequence(0.2f));
                ChangeState(State.idle);
            }
            else
            {
                ChangeState(previousState);
            }
        }
    }

    IEnumerator DamageBlink(int times, float frequency)
    {
        if (blink != null)
        {
            MeshRenderer mesh = gameObject.GetComponent<MeshRenderer>();

            for (int i = 0; i < times; i++)
            {
                mesh.material = blink;
                yield return new WaitForSeconds(frequency);
                mesh.material = material;
                yield return new WaitForSeconds(frequency);
            }
        }
    }

    IEnumerator InvunerabilityTime(float time)
    {
        invulnerable = true;
        yield return new WaitForSeconds(time);
        Stabilize();
        invulnerable = false;
    }

    IEnumerator DeathSequence(float time)
    {
        yield return new WaitForSeconds(time);
        Stabilize();
        invulnerable = false;
        MeshRenderer mesh = gameObject.GetComponent<MeshRenderer>();
        mesh.material = material;
        Death();
    }
}
