using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick : MonoBehaviour
{
    public void UpdateJoystick(Vector3 location)
    {
        SetLocation(location);
        gameObject.transform.LookAt(Input.mousePosition);
    }

    public void SetLocation(Vector3 location)
    {
        gameObject.transform.position = location;
    }

    public void SetStatus(bool status)
    {
        if (status)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    
}
