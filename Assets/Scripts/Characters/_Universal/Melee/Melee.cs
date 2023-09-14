using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MeleeActions
{
    public enum State { ready, attack, cooldown, off};
    public State state;

    private void Update()
    {
        switch (state)
        {
            case State.ready:
                break;
            case State.attack:
                Attack();
                SetCooldown(0);
                state = State.cooldown;
                break;
            case State.cooldown:
                if(cooldown < maxCooldown)
                    ChargeEnergy();
                else
                    state = State.ready;
                break;
        }
    }
}
