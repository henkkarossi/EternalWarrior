using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [HideInInspector]
    public Vector3 stickLocation;
    Vector3 mouseDirection;

    float lastClick;
    //default value set to 0.5 might need some tweaking
    public float clickInterval = 0.5f;
    [HideInInspector]
    public bool doubleClick;
    [HideInInspector]
    public PlayerController player;

    void Update()
    {
        if(GameManager.Instance.state == GameManager.State.level)
        {
            if (DoubleClickCheck())
            {
                player.HandleInput(new InputLine(true));
            }

            if (GetJoystick())
            {
                player.HandleInput(new InputLine(GetJoystickDirection()));
            }
        }
        else
        {
            lastClick = 0; 
        }
    }

    public bool GetJoystick()
    {
        return GetJoystickDirection() != Vector3.zero;
    }

    public Vector3 GetJoystickDirection()
    {
        Vector3 direction;

        if (Input.GetMouseButtonDown(0))
        {
            stickLocation = Input.mousePosition;
        }
        if (Input.GetMouseButton(0) && Input.mousePosition != stickLocation && Input.mousePosition != Vector3.zero)
        {
            Vector3 tiltLocation = Input.mousePosition;
            mouseDirection = ((tiltLocation - stickLocation)).normalized;

            //adjust direction to Y to be Z 
            direction = new Vector3(mouseDirection.x, 0, mouseDirection.y);
        }
        else
        {
            direction = Vector3.zero;
        }

        return direction;
    }

    public bool DoubleClickCheck()
    {
        if(Time.time > clickInterval)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Time.time < lastClick + clickInterval)
                {
                    lastClick = Time.time;
                    return true;
                }
                lastClick = Time.time;
            }
        }
        return false;
    }


}
