using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : PlayerActions
{
    public enum State { idle, move, takeDamage, off};
    public State state;
    State previousState;
    bool invulnerable;
    public Material blink;
    Material material;

    private void Awake()
    {
        material = gameObject.GetComponent<MeshRenderer>().material;
    }
    private void Update()
    {
        switch(state)
        {
            case State.idle:
                StopMoving();
                break;
            case State.move:
                Move(moveDirection);
                ChangeState(State.idle);
                break; 
            case State.takeDamage:
                break;
        }
    }

    public void ChangeState(State newState)
    {
        previousState = state;
        state = newState;
    }

    public void TakeDamage(int amount)
    {
        if (invulnerable == false)
        {
            StartCoroutine(InvunerabilityTime(invulnerabilityLength));
            StartCoroutine(DamageBlink(1, 0.02f));
            ChangeState(State.takeDamage);
            Damage(amount);
            ChangeState(previousState);
            if (hp <= 0)
            {
                StartCoroutine(DeathSequence(0.2f));
            }
        }
    }

    IEnumerator DamageBlink(int times, float frequency)
    {
        if (blink != null)
        {
            MeshRenderer mesh = gameObject.GetComponent<MeshRenderer>();
            Material org = mesh.material;

 
            for (int i = 0; i < times; i++)
            {
                mesh.material = blink;
                yield return new WaitForSeconds(frequency);
                mesh.material = org;
                yield return new WaitForSeconds(frequency);
            }
        }
    }

    IEnumerator InvunerabilityTime(float time)
    {
        invulnerable = true;
        yield return new WaitForSeconds(time);
        invulnerable = false;
    }

    IEnumerator DeathSequence(float time)
    {
        yield return new WaitForSeconds(time);
        state = State.off;
        MeshRenderer mesh = gameObject.GetComponent<MeshRenderer>();
        mesh.material = material;
        gameObject.SetActive(false);
        Debug.Log("GameOver!!!"); 
    }
}
