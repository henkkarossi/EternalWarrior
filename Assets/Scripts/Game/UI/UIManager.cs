using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerController playerController;
    public Meter hpBar;
    public Meter energyBar;
    public Joystick joystick;

    void Update()
    {
        if (GameManager.Instance.state == GameManager.State.level)
        {
            CheckJoystick();
            CheckHp();
            CheckCooldown();
        }
    }

    void CheckJoystick()
    {
        joystick.SetStatus(playerController.inputManager.GetJoystick());
        if (playerController.inputManager.GetJoystick())
        {
            joystick.UpdateJoystick(playerController.inputManager.stickLocation);
        }
    }

    int lastHp;
    void CheckHp()
    {
        if (playerController.player.hp != lastHp)
        {
            lastHp = playerController.player.hp;
            hpBar.UpdateMeter((float)playerController.player.hp / (float)playerController.player.maxHp * 100);
        }
    }

    float lastCooldown;
    void CheckCooldown()
    {
        if (playerController.melee.cooldownPercent != lastCooldown)
        {
            lastCooldown = playerController.melee.cooldownPercent;
            energyBar.UpdateMeter(playerController.melee.cooldownPercent);
        }
    }
}
